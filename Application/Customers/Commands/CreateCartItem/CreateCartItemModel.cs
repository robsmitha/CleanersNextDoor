using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Customers.Commands.CreateCartItem
{
    public class CreateCartItemModel
    {
        public int ItemID { get; set; }
        public int OrderID { get; set; }
        public int NewQty { get; set; }
        public CreateCartItemModel() { }
        public CreateCartItemModel(int orderId)
        {
            OrderID = orderId;
        }
    }
}
