using Domain.Entities;
using System.Collections.Generic;

namespace Application.Customers.Queries.GetCustomerCart
{
    public class CustomerCartModel
    {
        public int OrderID { get; set; }
        public string ClientSecret { get; set; }
        public string DisplayPrice { get; set; }
        public string MerchantName { get; set; }
        public int MerchantID { get; set; }
        public decimal Total { get; set; }
        public List<CustomerCartItemModel> CartItems { get; set; }
        public CustomerCartModel(List<CustomerCartItemModel> cartItems = null, 
            string displayPrice = null, 
            string clientSecret = null, 
            int orderId = 0,
            int merchantId = 0,
            string merchantName = null,
            decimal total = 0)
        {
            CartItems = cartItems ?? new List<CustomerCartItemModel>();
            DisplayPrice = displayPrice ?? 0.ToString("C");
            ClientSecret = clientSecret;
            OrderID = orderId;
            MerchantID = orderId;
            MerchantName = merchantName;
            Total = total;
        }
    }
}
