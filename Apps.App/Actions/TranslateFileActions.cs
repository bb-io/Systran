using Apps.App.Api;
using Apps.App.Invocables;
using Apps.Systran.Models;
using Apps.Systran.Models.Request;
using Apps.Systran.Models.Response;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;

namespace Apps.Systran.Actions
{

    [ActionList]
    public class TranslateFileActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : SystranInvocable(invocationContext)
    {
        public static readonly List<string> InputFormats = new List<string>
    {
        "application/vnd.android.string-resource+xml",
        "text/bitext",
        "text/html",
        "text/htm",
        "application/xhtml+xml",
        "application/vnd.systran.i18n+json",
        "text/x-java-properties",
        "application/vnd.microsoft.net.resx+xml",
        "text/rtf",
        "text/plain",
        "xml/xliff",
        "xml/xlf",
        "application/x-tmx+xml",
        "application/vnd.openxmlformats",
        "application/vnd.oasis.opendocument",
        "application/x-subrip",
        "application/pdf",
        "application/msword",
        "image/bmp",
        "image/jpeg",
        "image/png",
        "image/tiff"
    };

        [Action("Translate file", Description = "Translate a file from source language to target language")]
        public async Task<TranslateFileResponse> TranslateFile([ActionParameter] TranslateLanguagesOptions options, [ActionParameter] TranslateFileRequest input)
        {

            var inputType = input.Input.ContentType;
            if (!InputFormats.Contains(inputType))
                throw new PluginMisconfigurationException($"Unsupported file format: {inputType}. Please provide a file with one of the supported formats.");

            var request = new SystranRequest("/translation/file/translate", Method.Post)
            {
                AlwaysMultipartFormData = true
            };

            if (!string.IsNullOrEmpty(options.Source))
                request.AddQueryParameter("source", options.Source);
            if (!string.IsNullOrEmpty(options.Target))
                request.AddQueryParameter("target", options.Target);
            if (!string.IsNullOrEmpty(input.Profile))
                request.AddQueryParameter("profile", input.Profile);
            if (input.WithInfo == true)
                request.AddQueryParameter("withInfo", "true");
            if (input.WithSource == true)
                request.AddQueryParameter("withSource", "true");
            if (input.WithAnnotations == true)
                request.AddQueryParameter("withAnnotations", "true");

            var fileStream = await fileManagementClient.DownloadAsync(input.Input);
            
            request.AddFile("input", () => fileStream, input.Input.Name);

            var response = await Client.ExecuteWithErrorHandling<TranslateFileResponse>(request);

            return response;
        }
    }
}

