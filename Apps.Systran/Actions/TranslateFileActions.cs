using Apps.App.Api;
using Apps.App.Invocables;
using Apps.Systran.Models;
using Apps.Systran.Models.Request;
using Apps.Systran.Models.Response;
using Apps.Systran.Polling.Models;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;

namespace Apps.Systran.Actions
{
    [ActionList]
    public class TranslateFileActions : SystranInvocable
    {
        private readonly IFileManagementClient _fileManagementClient;

        public TranslateFileActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
            : base(invocationContext)
        {
            _fileManagementClient = fileManagementClient;
        }

        public static readonly List<string> InputFormats = new()
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
        public async Task<FileReferenceResponse> TranslateFile(
            [ActionParameter] TranslateLanguagesOptions options,
            [ActionParameter] TranslateFileRequest input)
        {
            var inputType = input.Input.ContentType;
            if (!InputFormats.Contains(inputType))
            {
                throw new PluginMisconfigurationException($"Unsupported file format: {inputType}. Please provide a file with one of the supported formats.");
            }

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

            var fileStream = await _fileManagementClient.DownloadAsync(input.Input);

            request.AddFile("input", () => fileStream, input.Input.Name);

            var rawResponse = await Client.ExecuteAsync(request);

            if (!rawResponse.IsSuccessful || rawResponse.RawBytes == null)
            {
                throw new PluginApplicationException($"Failed to translate file. Status: {rawResponse.StatusCode}, Error: {rawResponse.ErrorMessage}");
            }

            var translatedFile = await _fileManagementClient.UploadAsync(
                new MemoryStream(rawResponse.RawBytes),
                rawResponse.ContentType,
                $"{Path.GetFileNameWithoutExtension(input.Input.Name)}_translated{Path.GetExtension(input.Input.Name)}");

            return new FileReferenceResponse { FileResponse = translatedFile };
        }

        [Action("Translate file (Async)", Description = "Translate a file from source language to target language")]
        public async Task<TranslateFileAsyncResponse> TranslateFileAsync(
            [ActionParameter] TranslateLanguagesOptions options,
            [ActionParameter] TranslateFileRequest input)
        {
            var inputType = input.Input.ContentType;
            if (!InputFormats.Contains(inputType))
            {
                throw new PluginMisconfigurationException($"Unsupported file format: {inputType}. Please provide a file with one of the supported formats.");
            }

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

            request.AddQueryParameter("async", true);

            using var fileStream = await _fileManagementClient.DownloadAsync(input.Input);

            request.AddFile("input", () => fileStream, input.Input.Name);

            var rawResponse = await Client.ExecuteWithErrorHandling<TranslateFileAsyncResponse>(request);

            return new TranslateFileAsyncResponse { RequestId = rawResponse.RequestId };
        }

        [Action("Download translated file", Description = "Download a translated file by request ID")]
        public async Task<FileReferenceResponse> DownloadTranslatedFile([ActionParameter] string requestId)
        {
            var statusRequest = new SystranRequest("/translation/file/status", Method.Get);
            statusRequest.AddQueryParameter("requestId", requestId);

            var statusResponse = await Client.ExecuteAsync<TranslationStatusResponse>(statusRequest);

            if (statusResponse.Data == null || statusResponse.Data.Status != "finished")
            {
                throw new PluginApplicationException("Translation is not finished yet or request ID is invalid.");
            }

            var resultRequest = new SystranRequest("/translation/file/result", Method.Get);
            resultRequest.AddQueryParameter("requestId", requestId);

            var rawResponse = await Client.ExecuteAsync(resultRequest);

            if (!rawResponse.IsSuccessful || rawResponse.RawBytes == null)
            {
                throw new PluginApplicationException($"Failed to retrieve the translation result. Status: {rawResponse.StatusCode}, Error: {rawResponse.ErrorMessage}");
            }

            var translatedFile = await _fileManagementClient.UploadAsync(
                new MemoryStream(rawResponse.RawBytes),
                rawResponse.ContentType,
                $"{requestId}");

            return new FileReferenceResponse { FileResponse = translatedFile };
        }
    }
}
