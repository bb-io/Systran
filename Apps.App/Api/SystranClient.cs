using Apps.App.Constants;
using Apps.App.Dto;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using Newtonsoft.Json;
using RestSharp;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Apps.App.Api;

public class SystranClient : BlackBirdRestClient
{
    public SystranClient(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders):
        base(new RestSharp.RestClientOptions { BaseUrl = GetUri(authenticationCredentialsProviders)})
    {
        var apiKey = authenticationCredentialsProviders
             .First(x => x.KeyName == CredsNames.ApiKey).Value;

        this.AddDefaultHeader("Authorization", $"Key {apiKey}");
    }

    public static Uri GetUri(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentials)
    {
        return new Uri($"{authenticationCredentials.First(x => x.KeyName == CredsNames.Url).Value}");
    }

    protected override Exception ConfigureErrorException(RestResponse response)
    {
        var errors = JsonConvert.DeserializeObject<SystranError>(response.Content!)!;

        if (errors != null && !string.IsNullOrEmpty(errors.Message))
        {
            return new($"Systran API Error: {errors.Message}");
        }

        return new("Unknown error");
    }

    public override async Task<RestResponse> ExecuteWithErrorHandling(RestRequest request)
    {
        var restResponse = await ExecuteAsync(request);
        if (!restResponse.IsSuccessStatusCode)
            throw ConfigureErrorException(restResponse);

        try
        {
            var error = JsonConvert.DeserializeObject<SystranError>(restResponse.Content!);
            if (error is { HasError: true })
                throw new PluginMisconfigurationException($"Systran error: {error.Message}");
        }
        catch (PluginMisconfigurationException)
        {
            throw;
        }

        return restResponse;
    }
}