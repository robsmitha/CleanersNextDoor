using Application.Customers;
using AutoMapper;
using Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Orders.Queries.GetOrder
{
    public class GetOrderQuery : IRequest<OrderModel>
    {
        public int OrderID { get; set; }
        public GetOrderQuery(int orderId)
        {
            OrderID = orderId;
        }
    }

    public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, OrderModel>
    {
        private readonly ICleanersNextDoorContext _context;
        private IMapper _mapper;

        public GetOrderQueryHandler(
            ICleanersNextDoorContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<OrderModel> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Orders.FindAsync(request.OrderID);
            return entity != null
                ? _mapper.Map<OrderModel>(entity)
                : new OrderModel();
        }
    }
}
