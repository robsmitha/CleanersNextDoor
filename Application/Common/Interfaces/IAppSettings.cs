namespace Application.Common.Interfaces
{
    public interface IAppSettings
    {
        string JwtSecret { get; set; }
        string JwtIssuer { get; set; }
        string GoogleApiKey { get; set; }
        string StripeSecretKey { get; set; }
        string StripePublicKey { get; set; }
    }
}