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
        public string Last4 { get; set; }
        public string Token { get; set; }
        public bool IsDefault { get; set; }
        public string Expires { get; set; }
        public string CardType { get; set; }
        public CreatePaymentMethodCommand(int customerId, CreatePaymentMethodModel model)
        {
            CustomerID = customerId;
            NameOnCard = model.NameOnCard;
            Last4 = model.Last4;
            Token = model.Token;
            IsDefault = model.IsDefault;
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
                if (request.IsDefault)
                {
                    //set other customer addresses to not default
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

                //TODO: call stripe paymentService api

                var paymentMethod = new PaymentMethod
                {
                    CustomerID = request.CustomerID,
                    IsDefault = request.IsDefault,
                    NameOnCard = request.NameOnCard,
                    ExpirationDate = DateTime.Now,
                    CardTypeID = _context.CardTypes.First().ID,
                    StripePaymentMethodID = string.Empty,
                    Last4 = "1234",
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
