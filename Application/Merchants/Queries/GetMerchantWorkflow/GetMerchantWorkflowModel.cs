using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Merchants.Queries.GetMerchantWorkflow
{
    public class GetMerchantWorkflowModel : IMapFrom<MerchantWorkflow>
    {
        public GetMerchantWorkflowModel()
        {
            Steps = new List<WorkflowStepModel>();
        }
        public int WorkflowID { get; set; }
        public string MerchantName { get; set; }
        public WorkflowCustomerModel Customer { get; set; }
        public List<WorkflowStepModel> Steps { get; set; }
    }
}
