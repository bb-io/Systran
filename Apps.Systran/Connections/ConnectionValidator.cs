using Apps.App.Api;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;
using RestSharp;

namespace Apps.App.Connections;

public class ConnectionValidator: IConnectionValidator
{
    public async ValueTask<ConnectionValidationResponse> ValidateConnection(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        CancellationToken cancellationToken)
    {
        try
        {
            var request = new SystranRequest("/translation/apiVersion", Method.Get);
            var response = await new SystranClient(authenticationCredentialsProviders).ExecuteWithErrorHandling(request);
            return new()
            {
                IsValid = true,
            };
        }
        catch(Exception ex)
        {
            return new()
            {
                IsValid = false,
                Message = ex.Message
            };
        }
    }
}