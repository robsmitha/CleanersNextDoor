using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customers.Commands.CreatePaymentMethod
{
    public class CreatePaymentMethodCommand : IRequest<CreatePaymentMethodModel>
    {
        public int CustomerID { get; set; }
        public string NameOnCard { get; set; }
        public string StripePaymentMethodID { get; set; }
        public bool IsDefault { get; set; }
        public CreatePaymentMethodCommand(int customerId, CreatePaymentMethodModel model)
        {
            CustomerID = customerId;
            NameOnCard = model.NameOnCard;
            StripePaymentMethodID = model.StripePaymentMethodID;
            IsDefault = model.IsDefault;
        }
    }
    public class CreatePaymentMethodCommandHandler : IRequestHandler<CreatePaymentMethodCommand, CreatePaymentMethodModel>
    {

        private readonly ICleanersNextDoorContext _context;
        private readonly IMapper _mapper;
        private readonly IStripeService _stripe;
        public CreatePaymentMethodCommandHandler(
            ICleanersNextDoorContext context,
            IMapper mapper,
            IStripeService stripe
            )
        {
            _context = context;
            _mapper = mapper;
            _stripe = stripe;
        }
        public async Task<CreatePaymentMethodModel> Handle(CreatePaymentMethodCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.IsDefault)
                {
                    //set other customer payment methods to not default
                    var entities = _context.PaymentMethods
                        .Where(a => a.CustomerID == request.CustomerID && a.IsDefault)
                        .ToList();
                    if (entities.Count != 0)
                    {
                        entities.ForEach(e => e.IsDefault = false);
                        _context.PaymentMethods.UpdateRange(entities);
                        await _context.SaveChangesAsync(cancellationToken);
                    }
                }

                var sPaymentMethod = _stripe
                    .GetPaymentMethod(request.StripePaymentMethodID);

                var paymentMethod = new PaymentMethod
                {
                    CustomerID = request.CustomerID,
                    IsDefault = request.IsDefault,
                    NameOnCard = request.NameOnCard,
                    StripePaymentMethodID = request.StripePaymentMethodID,
                    CardBrand = sPaymentMethod.Card.Brand,
                    Last4 = sPaymentMethod.Card.Last4,
                    ExpMonth = sPaymentMethod.Card.ExpMonth,
                    ExpYear = sPaymentMethod.Card.ExpYear,
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
