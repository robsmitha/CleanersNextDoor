using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class PaymentMethod : BaseEntity
    {
        public int CustomerID { get; set; }

        [ForeignKey("CustomerID")]
        public Customer Customer { get; set; }
        public string NameOnCard { get; set; }
        public string StripePaymentMethodID { get; set; }
        public bool IsDefault { get; set; }
        public long ExpMonth { get; set; }
        public long ExpYear { get; set; }
        public string CardBrand { get; set; }
        public string Last4 { get; set; }
    }
}
