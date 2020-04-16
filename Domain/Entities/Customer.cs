namespace Domain.Entities
{
    public class Customer : BaseEntity
    {
        public string Name { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Secret { get; set; }
        public string StripeCustomerID { get; set; }
        public bool EmailVerified { get; set; }
        public bool PhoneVerified { get; set; }
    }
}
