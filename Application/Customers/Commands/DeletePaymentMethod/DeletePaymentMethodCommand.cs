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

        public DeletePaymentMethodCommandHandler(ICleanersNextDoorContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeletePaymentMethodCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var record = await _context.PaymentMethods
                    .SingleOrDefaultAsync(a => a.ID == request.PaymentMethodID && a.CustomerID == request.CustomerID);
                if (record != null)
                {
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
