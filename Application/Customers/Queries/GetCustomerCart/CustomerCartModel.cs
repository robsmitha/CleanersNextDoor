using Domain.Entities;
using System.Collections.Generic;

namespace Application.Customers.Queries.GetCustomerCart
{
    public class CustomerCartModel
    {
        public string ClientSecret { get; set; }
        public string DisplayPrice { get; set; }
        public List<CustomerCartItemModel> CartItems { get; set; }
        public CustomerCartModel(List<CustomerCartItemModel> cartItems = null, string displayPrice = null, string clientSecret = null)
        {
            CartItems = cartItems ?? new List<CustomerCartItemModel>();
            DisplayPrice = displayPrice ?? 0.ToString("C");
            ClientSecret = clientSecret;
        }
    }
}
