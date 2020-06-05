using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IApplicationDbContext _context;
        public CreateServiceRequestCommandHandler(IApplicationDbContext context)
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
                       join mw in _context.MerchantWorkflows.AsEnumerable() on new { o.MerchantID, request.ServiceRequest.WorkflowID, HasDefaultSRST = true } equals new { mw.MerchantID, mw.WorkflowID, HasDefaultSRST = mw.DefaultServiceRequestStatusTypeID != null } into tmp_mw
                       from mw in tmp_mw.DefaultIfEmpty()
                       join srst in _context.ServiceRequestStatusTypes.AsEnumerable() on mw?.DefaultServiceRequestStatusTypeID equals srst.ID into tmp_srst
                       from srst in tmp_srst.DefaultIfEmpty()
                       where o.ID == request.OrderID
                       select new { o, li, srst };
            var rows = data.ToList();
            var order = data.First().o;
            var serviceRequestStatusType = data.First().srst;

            #region Create Payment record
            var paymentStatusTypePaid = _context.PaymentStatusTypes
                                .FirstOrDefault(o => o.Name.ToUpper() == "PAID");
            var paymentTypeID = _context.PaymentTypes
                .FirstOrDefault(o => o.Name.ToUpper() == "CREDIT CARD MANUAL");
            //pull price from server
            var price = rows.Sum(r => r.li.ItemAmount);
            var payment = new Payment
            {
                OrderID = order.ID,
                Amount = price, //request.Payment.DecimalAmount,
                PaymentTypeID = paymentTypeID.ID,
                PaymentStatusTypeID = paymentStatusTypePaid.ID,
                StripePaymentMethodID = request.Payment.StripePaymentMethodID,
                ChargedAt = request.Payment.ChargedAt
            };
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync(cancellationToken);
            #endregion

            #region Create Service Request record
            serviceRequestStatusType ??= await _context.ServiceRequestStatusTypes.FirstOrDefaultAsync(o => o.Name.ToUpper() == "CREATED");
            var serviceRequest = new ServiceRequest
            {
                OrderID = request.OrderID,
                WorkflowID = request.ServiceRequest.WorkflowID,
                ServiceRequestStatusTypeID = serviceRequestStatusType.ID,
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
