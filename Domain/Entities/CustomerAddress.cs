﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class CustomerAddress : BaseAddress
    {
        public int CustomerID { get; set; }

        [ForeignKey("CustomerID")]
        public Customer Customer { get; set; }
        public bool IsDefault { get; set; }

        /// <summary>
        /// The type of correspondence the address should default for (i.e. Customer dropoff/pickup)
        /// </summary>
        public int CorrespondenceTypeID { get; set; }
        [ForeignKey("CorrespondenceTypeID")]
        public CorrespondenceType CorrespondenceType { get; set; }
    }
}
