using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order : BaseEntity
    {
        public string Note { get; set; }
        public int OrderStatusTypeID { get; set; }

        [ForeignKey("OrderStatusTypeID")]
        public OrderStatusType OrderStatusType { get; set; }

        public int MerchantID { get; set; }

        [ForeignKey("MerchantID")]
        public Merchant Merchant { get; set; }

        /// <summary>
        /// Customer who placed the service request if logged in
        /// </summary>
        public int CustomerID { get; set; }

        [ForeignKey("CustomerID")]
        public Customer Customer { get; set; }
    }
    public static class OrderExtensions
    {
        /// <summary>
        /// indicates order is open and needs to be paid for
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool IsOpenOrder(this Order @this)
        {
            if (@this == null) return false;
            if (@this?.OrderStatusType == null) return false;
            return @this.OrderStatusType.IsOpenOrderStatus();
        }
    }
}
