using Stripe;
using Stripe.Checkout;

namespace ShortUrlApi.Payments;

public class PaymentProvider(StripeOptions stripeOptions)
{
    /// <summary>
    /// Create a new checkout session for a especific product
    /// </summary>
    /// <param name="productModel"></param>
    /// <returns>Session URL</returns>
    public string CreateCheckoutSession(ProductModel productModel)
    {
        // Stripe docs: https://docs.stripe.com/checkout/quickstart?lang=dotnet
        StripeConfiguration.ApiKey = stripeOptions.SecretKey;
        
        var domain = stripeOptions.FrontEndDomainUrl;
        var options = new SessionCreateOptions
        {
            LineItems =
            [
                new SessionLineItemOptions
                {
                    // Provide the exact Price ID (for example, pr_1234) of the product you want to sell
                    Price = productModel.VendorId,
                    Quantity = 1,
                }
            ],
            Mode = "payment",
            SuccessUrl = domain + "/success", // Stripe will redirect to this URL after payment
            CancelUrl = domain + "/cancel", // Stripe will redirect to this URL if the payment is canceled
        };
        var service = new SessionService();
        var session = service.Create(options);
        return session.Url;
    }
}