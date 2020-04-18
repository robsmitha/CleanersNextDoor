using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Customers.Queries.GetPaymentMethods
{
   public class PaymentMethodModel : IMapFrom<PaymentMethod>
    {
        public int ID { get; set; }
        public string NameOnCard { get; set; }
        public string Last4 { get; set; }
        public string CardTypeName { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsDefault { get; set; }
        public string StripePaymentMethodID { get; set; }
    }
}
