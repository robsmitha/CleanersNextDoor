using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Customers.Commands.RemoveCartItem
{
    public class RemoveCartItemModel
    {
        public int ItemID { get; set; }
        public int OrderID { get; set; }
    }
}
