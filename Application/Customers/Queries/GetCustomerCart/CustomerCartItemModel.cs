using Application.LineItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Customers.Queries.GetCustomerCart
{
    public class CustomerCartItemModel
    {
        public int ID { get; set; }
        public decimal ItemAmount { get; set; }
        public int ItemID { get; set; }
        public int OrderID { get; set; }
        public string ItemName { get; set; }
        public int? ItemMaxAllowed { get; set; }
        public int CurrentQuantity { get; set; }
        public string DisplayPrice => (ItemAmount * CurrentQuantity).ToString("C");
        public CustomerCartItemModel(LineItemModel lineItem, int qty)
        {
            ID = lineItem.ID;
            ItemAmount = lineItem.ItemAmount;
            ItemID = lineItem.ItemID;
            OrderID = lineItem.OrderID;
            ItemName = lineItem.ItemName;
            ItemMaxAllowed = lineItem.ItemMaxAllowed;
            CurrentQuantity = qty;
        }
    }
}
