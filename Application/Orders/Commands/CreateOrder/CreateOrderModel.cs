using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Orders.Commands.CreateOrder
{
    public class CreateOrderModel
    {
        public int MerchantID { get; set; }
        public int CustomerID { get; set; }
    }
}
