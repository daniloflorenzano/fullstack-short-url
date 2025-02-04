namespace ShortUrlApi.Payments;

public class StripeOptions
{
    public string SecretKey { get; set; } = string.Empty;
    public string FrontEndDomainUrl { get; set; } = string.Empty;
}