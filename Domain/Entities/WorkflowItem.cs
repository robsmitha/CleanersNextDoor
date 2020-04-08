using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class WorkflowItem : BaseEntity
    {
        /// <summary>
        /// Associated workflow
        /// </summary>
        public int WorkflowID { get; set; }
        [ForeignKey("WorkflowID")]
        public Workflow Workflow { get; set; }

        /// <summary>
        /// Current Correspondence in workflow
        /// </summary>
        public int CorrespondenceTypeID { get; set; }
        [ForeignKey("CorrespondenceTypeID")]
        public CorrespondenceType CorrespondenceType { get; set; }

        /// <summary>
        /// Order of correspondence in workflow
        /// </summary>
        public int WorkflowOrder { get; set; }

    }
}
