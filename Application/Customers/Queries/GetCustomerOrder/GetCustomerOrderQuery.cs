using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customers.Queries.GetCustomerOrder
{
    public class GetCustomerOrderQuery : IRequest<GetCustomerOrderModel>
    {
        public int OrderID { get; set; }
        public GetCustomerOrderQuery(int orderId)
        {
            OrderID = orderId;
        }
    }

    public class GetOrderQueryHandler : IRequestHandler<GetCustomerOrderQuery, GetCustomerOrderModel>
    {
        private readonly IApplicationDbContext _context;
        private IMapper _mapper;

        public GetOrderQueryHandler(
            IApplicationDbContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<GetCustomerOrderModel> Handle(GetCustomerOrderQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Orders.FindAsync(request.OrderID);
            return entity != null
                ? _mapper.Map<GetCustomerOrderModel>(entity)
                : new GetCustomerOrderModel();
        }
    }
}
