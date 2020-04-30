using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customers.Commands.CreateServiceRequest
{
    public class CreateServiceRequestCommand : IRequest<int>
    {
        public int CustomerID { get; set; }
        public int OrderID { get; set; }
        public PaymentModel Payment { get; set; }
        public ServiceRequestModel ServiceRequest { get; set; }
        public List<CorrespondenceAddressModel> CorrespondenceAddresses { get; set; }
        public CreateServiceRequestCommand(CreateServiceRequestModel model, int customerId)
        {
            CustomerID = customerId;
            OrderID = model.OrderID;
            Payment = model.Payment;
            CorrespondenceAddresses = model.CorrespondenceAddresses;
            ServiceRequest = model.ServiceRequest;
        }
    }
    public class CreateServiceRequestCommandHandler : IRequestHandler<CreateServiceRequestCommand, int>
    {
        private readonly ICleanersNextDoorContext _context;
        public CreateServiceRequestCommandHandler(ICleanersNextDoorContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Checkout workflow to complete a customer order
        /// Step 1: Create Payment
        /// Step 2: Create Service Request
        /// Step 3: Create Correspondence records for given workflow 
        /// Step 4: Update Order to paid
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> Handle(CreateServiceRequestCommand request, CancellationToken cancellationToken)
        {
            var data = from o in _context.Orders.AsEnumerable()
                       join li in _context.LineItems.AsEnumerable() on o.ID equals li.OrderID
                       where o.ID == request.OrderID
                       select new { o, li };

            var order = data.First().o;

            #region Create Payment record
            var paymentStatusTypePaid = _context.PaymentStatusTypes
                                .FirstOrDefault(o => o.Name.ToUpper() == "PAID");
            var paymentTypeID = _context.PaymentTypes
                .FirstOrDefault(o => o.Name.ToUpper() == "CREDIT CARD MANUAL");
            var payment = new Payment
            {
                OrderID = order.ID,
                Amount = request.Payment.DecimalAmount,
                PaymentTypeID = paymentTypeID.ID,
                PaymentStatusTypeID = paymentStatusTypePaid.ID,
                StripePaymentMethodID = request.Payment.StripePaymentMethodID,
                ChargedAt = request.Payment.ChargedAt
            };
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync(cancellationToken);
            #endregion

            #region Create Service Request record
            var created = _context.ServiceRequestStatusTypes
                .FirstOrDefault(o => o.Name.ToUpper() == "CREATED");
            var serviceRequest = new ServiceRequest
            {
                OrderID = request.OrderID,
                WorkflowID = request.ServiceRequest.WorkflowID,
                ServiceRequestStatusTypeID = created.ID,
                Name = request.ServiceRequest.Name,
                Phone = request.ServiceRequest.Phone,
                Email = request.ServiceRequest.Email
            };
            _context.ServiceRequests.Add(serviceRequest);
            await _context.SaveChangesAsync(cancellationToken);
            #endregion

            #region Create Correspondence records for given workflow
            var needConfirmation = _context.CorrespondenceStatusTypes
                .FirstOrDefault(s => s.Name.ToUpper() == "NEEDS CONFIRMATION");
            foreach (var correspondence in request.CorrespondenceAddresses)
            {
                _context.Correspondences.Add(new Correspondence 
                { 
                    ScheduledAt = correspondence.ScheduledAt,
                    ServiceRequestID = serviceRequest.ID,
                    CorrespondenceTypeID = correspondence.CorrespondenceTypeID,
                    CorrespondenceStatusTypeID = needConfirmation.ID,
                    Note = correspondence.Note,
                    Street1 = correspondence.Street1,
                    Street2 = correspondence.Street2,
                    City = correspondence.City,
                    StateAbbreviation = correspondence.StateAbbreviation,
                    Zip = correspondence.Zip,
                    Latitude = correspondence.Latitude,
                    Longitude = correspondence.Longitude,
                });
            }
            #endregion

            #region Update Order to paid

            var remaining = data.Sum(l => l.li.ItemAmount) - payment.Amount;

            var orderStatus = remaining > 0
                ? _context.OrderStatusTypes.FirstOrDefault(o => o.Name.ToUpper() == "PARTIALLY PAID")
                : _context.OrderStatusTypes.FirstOrDefault(o => o.Name.ToUpper() == "PAID");
            order.OrderStatusTypeID = orderStatus.ID;
            order.ModifiedTime = DateTime.Now;
            _context.Orders.Update(order);
            #endregion

            await _context.SaveChangesAsync(cancellationToken);

            return order.ID;
        }
    }
}
