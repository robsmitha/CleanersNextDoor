using Application.Common.Interfaces;

namespace Infrastructure.Identity
{
    public class AppUser : IAppUser
    {
        public bool authenticated { get; set; }
        public AppUser() { }
        public AppUser(bool auth)
        {
            authenticated = auth;
        }
    }
}
