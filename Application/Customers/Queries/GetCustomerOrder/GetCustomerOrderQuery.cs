using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customers.Queries.GetCustomerOrder
{
    public class GetCustomerOrderQuery : IRequest<GetCustomerOrderModel>
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public GetCustomerOrderQuery(int orderId, int customerId)
        {
            OrderID = orderId;
            CustomerID = customerId;
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
            var data = await Task.FromResult(from o in _context.Orders.AsEnumerable()
                                             join m in _context.Merchants.AsEnumerable() on o.MerchantID equals m.ID
                                             join mi in _context.MerchantImages.AsEnumerable() on new { MerchantID = m.ID, IsDefault = true } equals new { mi.MerchantID, mi.IsDefault } into tmp_mi
                                             from mi in tmp_mi.DefaultIfEmpty()
                                             join ost in _context.OrderStatusTypes.AsEnumerable() on o.OrderStatusTypeID equals ost.ID
                                             join li in _context.LineItems.AsEnumerable() on o.ID equals li.OrderID
                                             join i in _context.Items.AsEnumerable() on li?.ItemID equals i.ID
                                             join p in _context.Payments.AsEnumerable() on o.ID equals p.OrderID
                                             join pst in _context.PaymentStatusTypes.AsEnumerable() on p.PaymentStatusTypeID equals pst.ID
                                             join pt in _context.PaymentTypes.AsEnumerable() on p.PaymentTypeID equals pt.ID
                                             join sr in _context.ServiceRequests.AsEnumerable() on o.ID equals sr.OrderID
                                             join w in _context.Workflows.AsEnumerable() on sr.WorkflowID equals w.ID
                                             join srst in _context.ServiceRequestStatusTypes.AsEnumerable() on sr.ServiceRequestStatusTypeID equals srst.ID
                                             join c in _context.Correspondences.AsEnumerable() on sr.ID equals c.ServiceRequestID
                                             join u in _context.Users.AsEnumerable() on c.UserID equals u.ID into tmp_u
                                             from u in tmp_u.DefaultIfEmpty()
                                             join ct in _context.CorrespondenceTypes.AsEnumerable() on c.CorrespondenceTypeID equals ct.ID
                                             join cst in _context.CorrespondenceStatusTypes.AsEnumerable() on c.CorrespondenceStatusTypeID equals cst.ID
                                             where o.ID == request.OrderID && o.CustomerID == o.CustomerID && !o.IsOpenOrder()
                                             select new { o, li, sr, c,  p, mi });
            var rows = data?.ToList();
            if (rows == null || rows.Count == 0) return new GetCustomerOrderModel();
            
            OrderModel order = null;
            var dict_li = new Dictionary<int, LineItemModel>();
            var dict_p = new Dictionary<int, PaymentModel>();
            var dict_c = new Dictionary<int, CorrespondenceModel>();
            foreach (var row in rows)
            {
                if (order == null)
                    order = _mapper.Map<OrderModel>(row.o);

                if(row.mi != null && string.IsNullOrEmpty(order.MerchantDefaultImageUrl))
                    order.MerchantDefaultImageUrl = row.mi.ImageUrl;

                if (order.ServiceRequest.ID == 0)
                    order.ServiceRequest = _mapper.Map<ServiceRequestModel>(row.sr);

                if (!dict_c.ContainsKey(row.c.ID))
                {
                    dict_c.Add(row.c.ID, _mapper.Map<CorrespondenceModel>(row.c));
                    order.ServiceRequest.Correspondences.Add(dict_c[row.c.ID]);
                }

                if (!dict_li.ContainsKey(row.li.ID))
                {
                    dict_li.Add(row.li.ID, _mapper.Map<LineItemModel>(row.li));
                    order.LineItems.Add(dict_li[row.li.ID]);
                }

                if (!dict_p.ContainsKey(row.p.ID))
                {
                    dict_p.Add(row.p.ID, _mapper.Map<PaymentModel>(row.p));
                    order.Payments.Add(dict_p[row.p.ID]);
                }

            }

            return new GetCustomerOrderModel(order);
        }
    }
}
