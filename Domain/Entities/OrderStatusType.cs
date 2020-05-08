using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OrderStatusType : BaseType
    {
        /// <summary>
        /// Indicates the system allows for payments to be added to orders in the given status
        /// </summary>
        public bool CanAddPayment { get; set; }

        /// <summary>
        /// Indicates the system allows for lineitems to be added to orders in the given status
        /// </summary>
        public bool CanAddLineItem { get; set; }
    }
    public static class OrderStatusTypeExtensions
    {
        public static bool IsOpenOrderStatus(this OrderStatusType @this)
        {
            if (@this == null) return false;

            return @this.CanAddLineItem && @this.CanAddPayment;
        }
    }
}
