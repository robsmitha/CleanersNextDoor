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

namespace Application.Customers.Commands.RemoveCartItem
{
    public class RemoveCartItemCommand : IRequest<bool>
    {
        public int ItemID { get; set; }
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public RemoveCartItemCommand(RemoveCartItemModel model, int customerId)
        {
            ItemID = model.ItemID;
            OrderID = model.OrderID;
            CustomerID = customerId;
        }
        public RemoveCartItemCommand(int itemId, int orderId)
        {
            ItemID = itemId;
            OrderID = orderId;
        }
    }
    public class RemoveCartItemCommandHandler : IRequestHandler<RemoveCartItemCommand, bool>
    {
        private readonly ICleanersNextDoorContext _context;
        private IMapper _mapper;

        public RemoveCartItemCommandHandler(
            ICleanersNextDoorContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var lineItems = _context.LineItems
                    .Include(o => o.Order)
                    .Where(l => l.OrderID == request.OrderID && l.ItemID == request.ItemID && l.Order.CustomerID == request.CustomerID);
                _context.LineItems.RemoveRange(lineItems);
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
    } 

}
