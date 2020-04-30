using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class CorrespondenceType : BaseType
    {
        public bool CustomerConfigures { get; set; }
        public bool MerchantConfigures { get; set; }
    }
}
