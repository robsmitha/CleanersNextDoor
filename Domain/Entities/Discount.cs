using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Discount : BaseType
    {
        public decimal Percentage { get; set; }
        public decimal? Amount { get; set; }
    }
}
