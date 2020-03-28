using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Orders.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<OrderModel>
    {
        public int MerchantID { get; set; }
        public int CustomerID { get; set; }
        public CreateOrderCommand(CreateOrderModel model)
        {
            MerchantID = model.MerchantID;
            CustomerID = model.CustomerID;
        }
    }

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderModel>
    {
        private readonly ICleanersNextDoorContext _context;
        private IMapper _mapper;

        public CreateOrderCommandHandler(
            ICleanersNextDoorContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OrderModel> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var entity = new Order
            {
                Active = true,
                CreatedAt = DateTime.Now,
                OrderStatusTypeID = 1,
                MerchantID = request.MerchantID,
                CustomerID = request.CustomerID
            };
            _context.Orders.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<OrderModel>(entity);
        }
    }
}
