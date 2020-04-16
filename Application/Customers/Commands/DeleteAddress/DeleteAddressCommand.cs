using AutoMapper;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customers.Commands.DeleteAddress
{
    public class DeleteAddressCommand : IRequest<bool>
    {
        public int CustomerAddressID { get; set; }
        public int CustomerID { get; set; }
        public DeleteAddressCommand(int id, int customerId)
        {
            CustomerAddressID = id;
            CustomerID = customerId;
        }
    }

    public class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand, bool>
    {
        private readonly ICleanersNextDoorContext _context;
        private readonly IMapper _mapper;

        public DeleteAddressCommandHandler(
            ICleanersNextDoorContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var record = await _context.CustomerAddresses
                    .SingleOrDefaultAsync(a => a.ID == request.CustomerAddressID && a.CustomerID == request.CustomerID);
                if (record != null)
                {
                    _context.CustomerAddresses.Remove(record);
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
