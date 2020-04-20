using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Customers.Commands.CreatePaymentMethod
{
    public class CreatePaymentMethodModel : IMapFrom<PaymentMethod>
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public string NameOnCard { get; set; }
        public string StripePaymentMethodID { get; set; }
        public bool IsDefault { get; set; }
    }
}
