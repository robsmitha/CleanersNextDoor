using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Payment : BaseType
    {
        public decimal Amount { get; set; }
        public decimal? CashTendered { get; set; }
        public int PaymentTypeID { get; set; }
        public int PaymentStatusTypeID { get; set; }
        public int? AuthorizationID { get; set; }
        public int OrderID { get; set; }

        [ForeignKey("PaymentTypeID")]
        public PaymentType PaymentType { get; set; }

        [ForeignKey("PaymentStatusTypeID")]
        public PaymentStatusType PaymentStatusType { get; set; }

        [ForeignKey("AuthorizationID")]
        public Authorization Authorization { get; set; }

        [ForeignKey("OrderID")]
        public Order Order { get; set; }
    }
}
