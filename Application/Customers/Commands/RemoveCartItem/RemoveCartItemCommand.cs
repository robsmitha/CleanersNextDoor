using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System.Linq;
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
        private readonly IApplicationDbContext _context;
        private IMapper _mapper;

        public RemoveCartItemCommandHandler(
            IApplicationDbContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
        {
            var removeLineItems = _context.LineItems.Where(l => l.OrderID == request.OrderID && l.ItemID == request.ItemID);
            _context.LineItems.RemoveRange(removeLineItems);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    } 

}
