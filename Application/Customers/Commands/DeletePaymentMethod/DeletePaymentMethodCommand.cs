using Application.Common.Interfaces;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customers.Commands.DeletePaymentMethod
{
    public class DeletePaymentMethodCommand : IRequest<bool>
    {
        public int PaymentMethodID { get; set; }
        public int CustomerID { get; set; }
        public DeletePaymentMethodCommand(int id, int customerId)
        {
            PaymentMethodID = id;
            CustomerID = customerId;
        }
    }

    public class DeletePaymentMethodCommandHandler : IRequestHandler<DeletePaymentMethodCommand, bool>
    {
        private readonly ICleanersNextDoorContext _context;
        private readonly IStripeService _stripe;

        public DeletePaymentMethodCommandHandler(ICleanersNextDoorContext context, IStripeService stripe)
        {
            _context = context;
            _stripe = stripe;
        }

        public async Task<bool> Handle(DeletePaymentMethodCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var record = await _context.PaymentMethods
                    .SingleOrDefaultAsync(a => a.ID == request.PaymentMethodID && a.CustomerID == request.CustomerID);
                if (record != null)
                {
                    _stripe.DetachPaymentMethod(record.StripePaymentMethodID);
                    _context.PaymentMethods.Remove(record);
                    await _context.SaveChangesAsync(cancellationToken);
                    return true;
                }
            }
            catch (Exception)
            {

            }
            return true;
        }
    }
}
