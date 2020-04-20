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
        public string StripePaymentMethodID { get; set; }
        public bool IsDefault { get; set; }
        public long ExpMonth { get; set; }
        public long ExpYear { get; set; }
        public string CardBrand { get; set; }
        public string Last4 { get; set; }
    }
}
