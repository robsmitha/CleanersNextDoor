using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
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
        private readonly ICleanersNextDoorContext _context;
        private readonly IStripeService _stripe;
        private IMapper _mapper;

        public GetCustomerCartQueryHandler(
            ICleanersNextDoorContext context,
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
            var data = from o in _context.Orders.AsEnumerable()
                       join ost in _context.OrderStatusTypes.AsEnumerable() on o.OrderStatusTypeID equals ost.ID
                       join li in _context.LineItems.AsEnumerable() on o.ID equals li.OrderID into tmp_li
                       from li in tmp_li.DefaultIfEmpty()
                       join i in _context.Items.AsEnumerable() on li?.ItemID equals i.ID into tmp_i
                       from i in tmp_i.DefaultIfEmpty()
                       where o.CustomerID == request.CustomerID
                       && o.MerchantID == request.MerchantID
                       && o.IsOpenOrder()
                       select new { o, li };

            if (data == null || data.FirstOrDefault() == null) return new CustomerCartModel();
            var lineItems = data.First().li != null
                ? _mapper.Map<List<LineItemModel>>(data.Select(row => row.li))
                : null;
            if (lineItems == null) return new CustomerCartModel();

            var cartItems = new Dictionary<int, CustomerCartItemModel>();
            foreach (var lineItem in lineItems)
            {
                if(!cartItems.ContainsKey(lineItem.ItemID))
                    cartItems.Add(lineItem.ItemID, _mapper.Map<CustomerCartItemModel>(lineItem));
                
                cartItems[lineItem.ItemID].CurrentQuantity++;
            }

            var distinctCartItems = cartItems.Values.ToList();
            var total = distinctCartItems.Sum(li => li.Price);
            var paymentIntent = request.AllowCheckout
                ? _stripe.CreatePaymentIntent(orderId: data.First().o.ID, centAmount: Convert.ToInt64(total * 100))
                : null;
            await Task.FromResult(0);
            return new CustomerCartModel(
                cartItems: distinctCartItems, 
                displayPrice: total.ToString("C"), 
                clientSecret: paymentIntent?.ClientSecret);
        }
    }
}
