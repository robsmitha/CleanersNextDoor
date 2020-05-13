using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Application.Customers.Queries.GetCustomerOrder
{
    public class GetCustomerOrderModel
    {
        public OrderModel Order { get; set; }
        public GetCustomerOrderModel(OrderModel order = null)
        {
            Order = order ?? new OrderModel();
        }
    }
    public class OrderModel : IMapFrom<Order>
    {
        public OrderModel()
        {
            LineItems = new List<LineItemModel>();
            ServiceRequest = new ServiceRequestModel();
            Payments = new List<PaymentModel>();
        }
        public int ID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public string DisplayUpdated => ModifiedTime.HasValue
            ? ModifiedTime.Value.ToString("MM/dd/yyyy hh:mm tt")
            : CreatedAt.ToString("MM/dd/yyyy hh:mm tt");
        public string Note { get; set; }
        public int OrderStatusTypeID { get; set; }
        public string OrderStatusTypeName { get; set; }
        public string OrderStatusTypeDescription { get; set; }
        public int MerchantID { get; set; }
        public string MerchantName { get; set; }
        public string MerchantMerchantTypeName { get; set; }
        public string MerchantDefaultImageUrl { get; set; }
        public bool IsOpenOrder { get; set; }
        public bool IsUpcomingOrder { get; set; }
        public string DisplayOrderTotal => LineItems.Sum(l => l.ItemAmount).ToString("C");
        public string DisplayPaymentTotal => Payments.Sum(l => l.Amount).ToString("C");
        public ServiceRequestModel ServiceRequest { get; set; }
        public List<LineItemModel> LineItems { get; private set; }
        public List<PaymentModel> Payments { get; private set; }
    }

    public class ServiceRequestModel : IMapFrom<ServiceRequest>
    {
        public ServiceRequestModel() : this(correspondences: null) { }
        public ServiceRequestModel(List<CorrespondenceModel> correspondences = null)
        {
            Correspondences = correspondences ?? new List<CorrespondenceModel>();
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ServiceRequestStatusTypeName { get; set; }
        public string ServiceRequestStatusTypeDescription { get; set; }
        public string WorkflowName { get; set; }
        public List<CorrespondenceModel> Correspondences { get; private set; }
    }

    public class CorrespondenceModel : IMapFrom<Correspondence>
    {
        public int ID { get; set; }
        public DateTime ScheduledAt { get; set; }
        public string MonthAbbrevation => ScheduledAt.ToString("MMM", CultureInfo.InvariantCulture);
        public int DayOfMonth => ScheduledAt.Day;
        public string Name { get; set; }
        public string Note { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string StateAbbreviation { get; set; }
        public string Zip { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public int? UserID { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserDisplayName { get; set; }
        public string UserImageUrl { get; set; }

        public int CorrespondenceTypeID { get; set; }
        public string CorrespondenceTypeName { get; set; }
        public string CorrespondenceTypeDescription { get; set; }
        public bool CorrespondenceTypeCustomerConfigures { get; set; }
        public int CorrespondenceStatusTypeID { get; set; }
        public string CorrespondenceStatusTypeName { get; set; }

        public string Location
        {
            get
            {
                var fields = new[] { Street1, Street2, City, StateAbbreviation, Zip };
                var sb = new StringBuilder();
                for (var i = 0; i < fields.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(fields[i]))
                        continue;

                    sb.Append($"{fields[i]}, ");
                }
                var location = sb.ToString().Trim();
                return location.EndsWith(",")
                    ? location[0..^1]
                    : location;
            }
        }
    }

    public class LineItemModel : IMapFrom<LineItem>
    {
        public int ID { get; set; }
        public decimal ItemAmount { get; set; }
        public int ItemID { get; set; }
        public int OrderID { get; set; }
        public string ItemName { get; set; }
    }

    public class PaymentModel : IMapFrom<Payment>
    {
        public int ID { get; set; }
        public decimal Amount { get; set; }
        public int OrderID { get; set; }
        public DateTime ChargedAt { get; set; }
        public string PaymentStatusTypeName { get; set; }
        public string PaymentTypeName { get; set; }
    }
}
