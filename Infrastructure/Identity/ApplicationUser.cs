using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Identity
{
    public class ApplicationUser
    {
        public int ID { get; set; }
        public string Token { get; set; }
    }
}
