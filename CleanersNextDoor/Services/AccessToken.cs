using Infrastructure.Identity;

namespace CleanersNextDoor.Services
{
    public class AccessToken : IAccessToken
    {
        public string token_type { get; set; }
        public string access_token { get; set; }
        public string expires_in { get; set; }
    }

}
