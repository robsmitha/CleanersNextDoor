using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Merchants.Queries.GetMerchantWorkflow
{
    public class WorkflowCustomerModel : IMapFrom<Customer>
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}