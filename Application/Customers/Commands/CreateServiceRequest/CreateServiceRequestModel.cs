using Application.Customers.Commands.CreateServiceRequest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Customers.Commands.CreateServiceRequest
{
    public class CreateServiceRequestModel
    {
        public int OrderID { get; set; }
        public int MerchantID { get; set; }
        public ServiceRequestModel ServiceRequest { get; set; }
        public PaymentModel Payment { get; set; }
        public List<CorrespondenceAddressModel> CorrespondenceAddresses { get; set; }
    }
}
