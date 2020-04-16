using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Customers.Commands.CreatePaymentMethod
{
    public class CreatePaymentMethodModel : IMapFrom<PaymentMethod>
    {
        public int CustomerID { get; set; }
        public string NameOnCard { get; set; }
        public string Last4 { get; set; }
        public string Token { get; set; }
        public int CardTypeID { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
