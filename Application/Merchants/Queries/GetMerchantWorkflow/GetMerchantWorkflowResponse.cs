using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Merchants.Queries.GetMerchantWorkflow
{
    public class GetMerchantWorkflowResponse
    {
        public GetMerchantWorkflowResponse()
        {
            Steps = new List<WorkflowStepModel>();
        }
        public MerchantWorkflowModel Workflow { get; set; }
        public WorkflowCustomerModel Customer { get; set; }
        public List<WorkflowStepModel> Steps { get; set; }
    }
}
