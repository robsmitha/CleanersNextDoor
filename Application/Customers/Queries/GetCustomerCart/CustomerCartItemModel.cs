using Application.Common.Mappings;

namespace Application.Customers.Queries.GetCustomerCart
{
    public class CustomerCartItemModel : IMapFrom<LineItemModel>
    {
        public int ID { get; set; }
        public decimal ItemAmount { get; set; }
        public int ItemID { get; set; }
        public int OrderID { get; set; }
        public string ItemName { get; set; }
        public int? ItemMaxAllowed { get; set; }
        public int CurrentQuantity { get; set; }
        public decimal Price => ItemAmount * CurrentQuantity;
        public string DisplayPrice => Price.ToString("C");
    }
}
