using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Customers
{
    public class CustomerModel : IMapFrom<Customer>
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Secret { get; set; }
        public string StripeCustomerID { get; set; }
        public bool EmailVerified { get; set; }
        public bool PhoneVerified { get; set; }
        public bool HasAddresses { get; set; }
        public bool HasPaymentMethods { get; set; }
    }
}
