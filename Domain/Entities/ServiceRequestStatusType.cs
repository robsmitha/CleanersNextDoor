using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class ServiceRequestStatusType : BaseType
    {
        public bool IsActiveServiceRequest { get; set; }
        public bool IsCompleteServiceRequest { get; set; }
    }
}
