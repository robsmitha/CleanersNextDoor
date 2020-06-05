using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customers.Queries.GetCustomerCart
{
    public class GetCustomerCartQuery : IRequest<CustomerCartModel>
    {
        public int CustomerID { get; set; }
        public int MerchantID { get; set; }
        public bool AllowCheckout { get; set; }
        public GetCustomerCartQuery(int customerId, int merchantId, bool allowCheckout = false)
        {
            CustomerID = customerId;
            MerchantID = merchantId;
            AllowCheckout = allowCheckout;
        }
    }
    public class GetCustomerCartQueryHandler : IRequestHandler<GetCustomerCartQuery, CustomerCartModel>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStripeService _stripe;
        private IMapper _mapper;

        public GetCustomerCartQueryHandler(
            IApplicationDbContext context,
            IMapper mapper, 
            IStripeService stripe
            )
        {
            _context = context;
            _mapper = mapper;
            _stripe = stripe;
        }
        public async Task<CustomerCartModel> Handle(GetCustomerCartQuery request, CancellationToken cancellationToken)
        {
            var data = await Task.FromResult(from o in _context.Orders.AsEnumerable()
                                             join m in _context.Merchants.AsEnumerable() on o.MerchantID equals m.ID
                                             join ost in _context.OrderStatusTypes.AsEnumerable() on o.OrderStatusTypeID equals ost.ID
                                             join li in _context.LineItems.AsEnumerable() on o.ID equals li.OrderID into tmp_li
                                             from li in tmp_li.DefaultIfEmpty()
                                             join i in _context.Items.AsEnumerable() on li?.ItemID equals i.ID into tmp_i
                                             from i in tmp_i.DefaultIfEmpty()
                                             where o.CustomerID == request.CustomerID
                                             && o.MerchantID == request.MerchantID
                                             && o.IsOpenOrder()
                                             select new { o, li });
            var rows = data?.ToList();
            if (rows == null || rows.Count == 0) return new CustomerCartModel();
            var dict_ci = new Dictionary<int, CustomerCartItemModel>();
            var total = 0M;
            foreach (var row in rows)
            {
                if (row.li != null)
                {
                    if (!dict_ci.ContainsKey(row.li.ItemID))
                        dict_ci.Add(row.li.ItemID, _mapper.Map<CustomerCartItemModel>(row.li));

                    dict_ci[row.li.ItemID].CurrentQuantity++;
                    total += row.li.ItemAmount;
                }
            }
            var order = rows[0].o;
            var paymentIntent = request.AllowCheckout && total > 0
                ? _stripe.CreatePaymentIntent(orderId: order.ID, centAmount: Convert.ToInt64(total * 100))
                : null;
            return new CustomerCartModel(
                cartItems: dict_ci.Values.ToList(), 
                displayPrice: total.ToString("C"), 
                clientSecret: paymentIntent?.ClientSecret,
                orderId: order.ID,
                merchantId: order.MerchantID,
                merchantName: order.Merchant.Name,
                total: total);
        }
    }
}
