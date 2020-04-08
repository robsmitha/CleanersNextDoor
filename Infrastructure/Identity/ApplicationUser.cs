using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Identity
{
    public class ApplicationUser
    {
        /// <summary>
        /// Indicate client's jwt token httpOnlyCookie is valid if exists
        /// </summary>
        public bool authenticated { get; set; }
    }
}
