using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class MerchantWorkflow : BaseEntity
    {
        /// <summary>
        /// Associated Workflow
        /// </summary>
        public int WorkflowID { get; set; }
        [ForeignKey("WorkflowID")]
        public Workflow Workflow { get; set; }
        /// <summary>
        /// Associated Merchant
        /// </summary>
        public int MerchantID { get; set; }
        [ForeignKey("MerchantID")]
        public Merchant Merchant { get; set; }
        /// <summary>
        /// Indicates the default merchant workflow to use at checkout
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
