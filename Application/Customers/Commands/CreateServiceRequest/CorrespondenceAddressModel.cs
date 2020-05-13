using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Customers.Commands.CreateServiceRequest
{
    public class CorrespondenceAddressModel
    {
        public int CorrespondenceTypeID { get; set; }
        public DateTime ScheduledAt { get; set; }
        public string Note { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string StateAbbreviation { get; set; }
        public string Zip { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
