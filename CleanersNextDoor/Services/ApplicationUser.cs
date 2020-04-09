using Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanersNextDoor.Services
{
    public class ApplicationUser : IApplicationUser
    {
        public bool authenticated { get; set; }
        public ApplicationUser() { }
        public ApplicationUser(bool auth)
        {
            authenticated = auth;
        }
    }
}
