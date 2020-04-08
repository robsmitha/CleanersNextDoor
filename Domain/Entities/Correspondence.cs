using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class Correspondence : BaseEntity
    {
        /// <summary>
        /// Scheduled correspondence time
        /// </summary>
        public DateTime ScheduledAt { get; set; }

        /// <summary>
        /// Service request associated to the correspondence
        /// </summary>
        public int ServiceRequestID { get; set; }
        [ForeignKey("ServiceRequestID")]
        public ServiceRequest ServiceRequest { get; set; }

        /// <summary>
        /// User making the correspondence with the nested address
        /// </summary>
        public int? UserID { get; set; }
        [ForeignKey("UserID")]
        public User User { get; set; }

        /// <summary>
        /// The type of correspondence that is needed
        /// </summary>
        public int CorrespondenceTypeID { get; set; }
        [ForeignKey("CorrespondenceTypeID")]
        public CorrespondenceType CorrespondenceType { get; set; }

        /// <summary>
        /// The current status of the correspondence
        /// </summary>
        public int CorrespondenceStatusTypeID { get; set; }
        [ForeignKey("CorrespondenceStatusTypeID")]
        public CorrespondenceStatusType CorrespondenceStatusType { get; set; }

        public string Note { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string StateAbbreviation { get; set; }
        public string Zip { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
