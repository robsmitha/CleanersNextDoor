using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class ServiceRequest : BaseEntity
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        /// <summary>
        /// The order that paid for the service request
        /// </summary>
        public int OrderID { get; set; }
        [ForeignKey("OrderID")]
        public Order Order { get; set; }

        /// <summary>
        /// The overall status of the service request
        /// </summary>
        public int ServiceRequestStatusTypeID { get; set; }
        [ForeignKey("ServiceRequestStatusTypeID")]
        public ServiceRequestStatusType ServiceRequestStatusType { get; set; }

        /// <summary>
        /// the correspondence workflow to follow for this service request (i.e. food delivery, laundry)
        /// </summary>
        public int WorkflowID { get; set; }
        [ForeignKey("WorkflowID")]
        public Workflow Workflow { get; set; }
    }
    public static class ServiceRequsetExtensions
    {
        public static bool IsUpcoming(this ServiceRequest @this)
        {
            if (@this?.ServiceRequestStatusType == null) return false;
            return @this.ServiceRequestStatusType.IsActiveServiceRequest;
        }
    }
}
