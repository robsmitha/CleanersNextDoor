using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.LineItems
{
    public class LineItemModel : IMapFrom<LineItem>
    {
        public int ID { get; set; }
        public decimal ItemAmount { get; set; }
        public int ItemID { get; set; }
        public int OrderID { get; set; }
        public string ItemName { get; set; }
        public int? ItemMaxAllowed { get; set; }
    }
}
