using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customers.Commands.SetDefaultPaymentMethod
{
    public class SetDefaultPaymentMethodCommand : IRequest<bool>
    {
        public int CustomerPaymentID { get; set; }
        public int CustomerID { get; set; }
        public SetDefaultPaymentMethodCommand(int id, int customerId)
        {
            CustomerPaymentID = id;
            CustomerID = customerId;
        }
    }
    public class SetDefaultPaymentMethodCommandHandler : IRequestHandler<SetDefaultPaymentMethodCommand, bool>
    {
        private readonly ICleanersNextDoorContext _context;
        public SetDefaultPaymentMethodCommandHandler(ICleanersNextDoorContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(SetDefaultPaymentMethodCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var record = await _context.PaymentMethods
                    .SingleOrDefaultAsync(a => a.ID == request.CustomerPaymentID && a.CustomerID == request.CustomerID);
                if (record != null)
                {
                    //turn off previous default 
                    var entities = _context.PaymentMethods
                        .Where(a => a.CustomerID == request.CustomerID && a.IsDefault)
                        .ToList();
                    if (entities.Count != 0)
                    {
                        entities.ForEach(e => e.IsDefault = false);
                        _context.PaymentMethods.UpdateRange(entities);
                    }

                    record.IsDefault = true;
                    _context.PaymentMethods.Update(record);
                    await _context.SaveChangesAsync(cancellationToken);
                    return true;
                }
            }
            catch (Exception)
            {

            }
            return false;
        }
    }
}
