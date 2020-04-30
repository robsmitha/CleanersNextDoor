using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Payment : BaseEntity
    {
        public decimal Amount { get; set; }
        public int PaymentTypeID { get; set; }

        [ForeignKey("PaymentTypeID")]
        public PaymentType PaymentType { get; set; }
        public int PaymentStatusTypeID { get; set; }

        [ForeignKey("PaymentStatusTypeID")]
        public PaymentStatusType PaymentStatusType { get; set; }
        public int OrderID { get; set; }

        [ForeignKey("OrderID")]
        public Order Order { get; set; }

        public string StripePaymentMethodID { get; set; }
        public DateTime ChargedAt { get; set; }
    }
}
