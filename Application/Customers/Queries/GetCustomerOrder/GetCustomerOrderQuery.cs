using Application.Customers;
using AutoMapper;
using Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
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
        public async Task<GetCustomerOrderModel> Handle(GetCustomerOrderQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Orders.FindAsync(request.OrderID);
            return entity != null
                ? _mapper.Map<GetCustomerOrderModel>(entity)
                : new GetCustomerOrderModel();
        }
    }
}
