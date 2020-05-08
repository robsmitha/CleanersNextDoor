using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PriceType : BaseType
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsVariableCost { get; set; }

        /// <summary>
        /// TODO: implment PricingModel that maps variable cost logic 
        /// (i.e buy one get one, custom pricing model flows )
        /// </summary>
        //public int PricingModelID { get; set; }

    }
}
