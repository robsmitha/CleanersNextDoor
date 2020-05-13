using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customers.Queries.GetOrderHistory
{
    public class GetOrderHistoryQuery : IRequest<OrderHistoryModel>
    {
        public int CustomerID { get; set; }
        public GetOrderHistoryQuery(int customerId)
        {
            CustomerID = customerId;
        }
    }
    public class GetOrderHistoryQueryHandler : IRequestHandler<GetOrderHistoryQuery, OrderHistoryModel>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GetOrderHistoryQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OrderHistoryModel> Handle(GetOrderHistoryQuery request, CancellationToken cancellationToken)
        {
            var data = from o in _context.Orders.AsEnumerable()
                       join m in _context.Merchants.AsEnumerable() on o.MerchantID equals m.ID
                       join mt in _context.MerchantTypes.AsEnumerable() on m.MerchantTypeID equals mt.ID
                       join ost in _context.OrderStatusTypes.AsEnumerable() on o.OrderStatusTypeID equals ost.ID
                       join li in _context.LineItems.AsEnumerable() on o.ID equals li.OrderID
                       join sr in _context.ServiceRequests.AsEnumerable() on o.ID equals sr.OrderID into tmp_sr
                       from sr in tmp_sr.DefaultIfEmpty()
                       join srst in _context.ServiceRequestStatusTypes.AsEnumerable() on sr?.ServiceRequestStatusTypeID equals srst.ID into tmp_srst
                       from srst in tmp_srst.DefaultIfEmpty()
                       join w in _context.Workflows.AsEnumerable() on sr?.WorkflowID equals w.ID into tmp_w
                       from w in tmp_w.DefaultIfEmpty()
                       join ws in _context.WorkflowSteps.AsEnumerable() on new { WorkflowID = w?.ID ?? 0, Step = 1 } equals new { ws.WorkflowID, ws.Step } into tmp_ws
                       from ws in tmp_ws.DefaultIfEmpty()
                       join c in _context.Correspondences.AsEnumerable() on new { ServiceRequestID = sr?.ID ?? 0, CorrespondenceTypeID = ws?.CorrespondenceTypeID ?? 0 } equals new { c.ServiceRequestID, c.CorrespondenceTypeID } into tmp_c
                       from c in tmp_c.DefaultIfEmpty()
                       where o.CustomerID == request.CustomerID
                       orderby o.ModifiedTime ?? o.CreatedAt descending
                       select new { o, sr, li, c };

            if (data == null || data.FirstOrDefault() == null) return new OrderHistoryModel();
            var map = new Dictionary<int, OrderModel>();
            foreach (var row in data)
            {
                if(!map.TryGetValue(row.o.ID, out var order))
                {
                    order = _mapper.Map<OrderModel>(row.o);
                    order.IsOpenOrder = row.o.IsOpenOrder();
                    
                    //TODO: filter past and upcoming orders
                    if(row.sr != null && row.c != null)
                    {
                        order.ServiceRequest = _mapper.Map<ServiceRequestModel>(row.sr);
                        order.IsUpcomingOrder = row.sr.IsUpcoming() && row.c.ScheduledAt > DateTime.Now;
                    }

                    map.Add(row.o.ID, order);
                }
                order.LineItems.Add(_mapper.Map<LineItemModel>(row.li));
                map[row.o.ID] = order;
            }

            return await Task.FromResult(new OrderHistoryModel(map.Values.ToList()));
        }
    }
}
