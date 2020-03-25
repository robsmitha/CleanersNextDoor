using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class LineItem : BaseEntity
    {
        public decimal ItemAmount { get; set; }
        public int ItemID { get; set; }
        public int OrderID { get; set; }

        [ForeignKey("ItemID")]
        public Item Item { get; set; }

        [ForeignKey("OrderID")]
        public Order Order { get; set; }
    }
}
