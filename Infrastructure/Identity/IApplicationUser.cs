using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Identity
{
    public interface IApplicationUser
    {
        /// <summary>
        /// Indicate client's jwt token httpOnlyCookie is valid if exists
        /// </summary>
        public bool authenticated { get; set; }
    }
}
