using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Merchants.Queries.GetMerchantWorkflow
{
    public class WorkflowStepModel : IMapFrom<WorkflowStep>
    {
        public WorkflowStepModel()
        {
            Address = new WorkflowStepAddressModel();
        }
        public DateTime ScheduledAt { get; set; }
        public int Step { get; set; }
        public int CorrespondenceTypeID { get; set; }
        public string CorrespondenceTypeName { get; set; }
        public string CorrespondenceTypeDescription { get; set; }
        public bool CorrespondenceTypeCustomerConfigures { get; set; }
        public bool CorrespondenceTypeMerchantConfigures { get; set; }
        public WorkflowStepAddressModel Address { get; set; }
    }
}
