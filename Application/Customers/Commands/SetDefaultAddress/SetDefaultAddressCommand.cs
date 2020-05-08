using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customers.Commands.SetDefaultAddress
{
    public class SetDefaultAddressCommand : IRequest<bool>
    {
        public int CustomerAddressID { get; set; }
        public int CustomerID { get; set; }
        public SetDefaultAddressCommand(int id, int customerId)
        {
            CustomerAddressID = id;
            CustomerID= customerId;
        }
    }
    public class SetDefaultAddressCommandHandler : IRequestHandler<SetDefaultAddressCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        public SetDefaultAddressCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(SetDefaultAddressCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var record = await _context.CustomerAddresses
                    .SingleOrDefaultAsync(a => a.ID == request.CustomerAddressID && a.CustomerID == request.CustomerID);
                if (record != null)
                {
                    //turn off previous default 
                    var entities = _context.CustomerAddresses
                        .Where(a => a.CustomerID == request.CustomerID && a.IsDefault)
                        .ToList();
                    if (entities.Count != 0)
                    {
                        entities.ForEach(e => e.IsDefault = false);
                        _context.CustomerAddresses.UpdateRange(entities);
                    }

                    record.IsDefault = true;
                    _context.CustomerAddresses.Update(record);
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
