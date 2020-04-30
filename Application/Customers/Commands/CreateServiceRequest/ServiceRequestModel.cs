using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Customers.Commands.CreateServiceRequest
{
    public class ServiceRequestModel
    {
        public int WorkflowID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
