﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class ServiceRequest : BaseEntity
    {
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }

        /// <summary>
        /// Customer who placed the service request
        /// </summary>
        public int CustomerID { get; set; }
        [ForeignKey("CustomerID")]
        public Customer Customer { get; set; }

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
    }
}
