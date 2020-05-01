using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Customers.Queries.GetOrderHistory
{
    public class OrderHistoryModel
    {
        public OrderHistoryModel(List<OrderModel> orders = null)
        {
            Orders = orders ?? new List<OrderModel>();
        }
        public List<OrderModel> Orders { get; set; }
    }
    public class OrderModel : IMapFrom<Order>
    {
        public OrderModel()
        {
            LineItems = new List<LineItemModel>();
            ServiceRequest = new ServiceRequestModel();
        }
        public int ID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public string DisplayCreated => CreatedAt.ToString("MM/dd/yyyy hh:mm tt");
        public string DisplayUpdated => ModifiedTime.HasValue 
            ? ModifiedTime.Value.ToString("MM/dd/yyyy hh:mm tt") 
            : null;
        public string Note { get; set; }
        public int OrderStatusTypeID { get; set; }
        public string OrderStatusTypeName { get; set; }
        public int MerchantID { get; set; }
        public string MerchantName { get; set; }
        public bool IsOpenOrder { get; set; }
        public bool IsUpcomingOrder { get; set; }
        public string DisplayOrderTotal => LineItems.Sum(l => l.ItemAmount).ToString("C");
        public ServiceRequestModel ServiceRequest { get; set; }
        public List<LineItemModel> LineItems { get; set; }
    }
    public class LineItemModel : IMapFrom<LineItem>
    {
        public int ID { get; set; }
        public decimal ItemAmount { get; set; }
        public int ItemID { get; set; }
        public int OrderID { get; set; }
        public string ItemName { get; set; }
    }
    public class ServiceRequestModel : IMapFrom<ServiceRequest>
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ServiceRequestStatusTypeName { get; set; }
        public string WorkflowName { get; set; }
    }
}
