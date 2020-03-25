using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class ServiceRequest : BaseEntity
    {
        public int OrderID { get; set; }
        [ForeignKey("OrderID")]
        public Order Order { get; set; }
        public string Note { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string StateAbbreviation { get; set; }
        public string Zip { get; set; }
        public int ServiceRequestStatusTypeID { get; set; }
        [ForeignKey("ServiceRequestStatusTypeID")]
        public ServiceRequestStatusType ServiceRequestStatusType { get; set; }
        public DateTime Pickup { get; set; }
        public DateTime DropOff { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int PickupDrivingSessionID { get; set; }
        [ForeignKey("PickupDrivingSessionID")]
        public DrivingSession PickupDrivingSession { get; set; }
        public int DropOffDrivingSessionID { get; set; }
        [ForeignKey("DropOffDrivingSessionID")]
        public DrivingSession DropOffDrivingSession { get; set; }
    }
}
