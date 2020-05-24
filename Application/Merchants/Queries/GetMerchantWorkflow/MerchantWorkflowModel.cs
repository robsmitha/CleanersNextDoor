using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Merchants.Queries.GetMerchantWorkflow
{
    public class MerchantWorkflowModel : IMapFrom<MerchantWorkflow>
    {
        public int WorkflowID { get; set; }
        public int MerchantID { get; set; }
        public string MerchantName { get; set; }
        public string MerchantDefaultImageUrl { get; set; }
    }
}
