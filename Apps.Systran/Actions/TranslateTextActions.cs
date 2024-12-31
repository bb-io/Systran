using Apps.App.Api;
using Apps.App.Invocables;
using Apps.Systran.Models;
using Apps.Systran.Models.Request;
using Apps.Systran.Models.Response;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.App.Actions;

[ActionList]
public class TranslateTextActions(InvocationContext invocationContext) : SystranInvocable(invocationContext)
{

    [Action("Translate text", Description = "Translate text")]
    public async Task<TranslateTextResponse> TranslateText([ActionParameter] TranslateLanguagesOptions options,
        [ActionParameter] TranslateTextRequest input)
    {
        if (input.Input.Length > 50000 || GetInputSize(input.Input) > 50 * 1024 * 1024)
        {
            throw new PluginMisconfigurationException("Input exceeds the maximum allowed size of 50,000 paragraphs or 50 MB.");
        }

        var request = new SystranRequest($"/translation/text/translate", RestSharp.Method.Post);

        request.AddQueryParameter("input", input.Input);
        request.AddQueryParameter("source", options.Source ?? "auto");
        request.AddQueryParameter("target", options.Target);

        if (!string.IsNullOrEmpty(input.Profile))
            request.AddQueryParameter("profile", input.Profile);

        if (input.WithInfo.HasValue)
            request.AddQueryParameter("withInfo", input.WithInfo.Value.ToString().ToLower());

        if (input.WithSource.HasValue)
            request.AddQueryParameter("withSource", input.WithSource.Value.ToString().ToLower());

        if (input.WithAnnotations.HasValue)
            request.AddQueryParameter("withAnnotations", input.WithAnnotations.Value.ToString().ToLower());

        if (input.BackTranslation.HasValue)
            request.AddQueryParameter("backTranslation", input.BackTranslation.Value.ToString().ToLower());

        var response = await Client.ExecuteWithErrorHandling<TranslateTextResponse>(request);
        return response;
    }

    private long GetInputSize(string input)
    {
        return System.Text.Encoding.UTF8.GetByteCount(input);
    }
}