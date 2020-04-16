using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customers.Commands.CreatePaymentMethod
{
    public class CreatePaymentMethodCommand : IRequest<CreatePaymentMethodModel>
    {
        public int CustomerID { get; set; }
        public string NameOnCard { get; set; }
        public string Last4 { get; set; }
        public string Token { get; set; }
        public int CardTypeID { get; set; }
        public DateTime ExpirationDate { get; set; }
        public CreatePaymentMethodCommand(int customerId, CreatePaymentMethodModel model)
        {
            CustomerID = customerId;
        }
    }
    public class CreatePaymentMethodCommandHandler : IRequestHandler<CreatePaymentMethodCommand, CreatePaymentMethodModel>
    {

        private readonly ICleanersNextDoorContext _context;
        private readonly IMapper _mapper;

        public CreatePaymentMethodCommandHandler(
            ICleanersNextDoorContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CreatePaymentMethodModel> Handle(CreatePaymentMethodCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //call stripe api
                var paymentMethod = new PaymentMethod
                {
                    CustomerID = request.CustomerID,
                    //TODO: add info
                };
                _context.PaymentMethods.Add(paymentMethod);
                await _context.SaveChangesAsync(cancellationToken);
                return _mapper.Map<CreatePaymentMethodModel>(paymentMethod);
            }
            catch (Exception e)
            {
                return new CreatePaymentMethodModel();
            }
        }
    }
}
