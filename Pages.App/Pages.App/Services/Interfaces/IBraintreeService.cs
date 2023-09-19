using Braintree;

namespace Pages.App.Services.Interfaces
{
    public interface IBraintreeService
    {
        IBraintreeGateway CreateGateway();
        IBraintreeGateway GetGateway();
    }
}
