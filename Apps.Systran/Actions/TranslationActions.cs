using Apps.App.Api;
using Apps.App.Invocables;
using Apps.Systran.Models.Request;
using Apps.Systran.Models.Response;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Blueprints;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Blackbird.Filters.Transformations;
using Blackbird.Filters.Constants;
using Blackbird.Filters.Enums;
using Blackbird.Filters.Extensions;

namespace Apps.Systran.Actions
{
    [ActionList("Translation")]
    public class TranslationActions : SystranInvocable
    {
        private readonly IFileManagementClient _fileManagementClient;

        public TranslationActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
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
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "application/vnd.oasis.opendocument",
            "application/x-subrip",
            "application/pdf",
            "application/msword",
            "image/bmp",
            "image/jpeg",
            "image/png",
            "image/tiff"
        };


        [BlueprintActionDefinition(BlueprintAction.TranslateText)]
        [Action("Translate text", Description = "Translate text")]
        public async Task<TranslateTextResponse> TranslateText([ActionParameter] TranslateTextRequest input)
        {
            if (input.Text.Length > 50000 || GetInputSize(input.Text) > 50 * 1024 * 1024)
            {
                throw new PluginMisconfigurationException("Input exceeds the maximum allowed size of 50,000 paragraphs or 50 MB.");
            }

            var request = new SystranRequest($"/translation/text/translate", RestSharp.Method.Post);

            request.AddQueryParameter("input", input.Text);
            request.AddQueryParameter("source", input.Source ?? "auto");
            request.AddQueryParameter("target", input.TargetLanguage);

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

            var systranResponse = await Client.ExecuteWithErrorHandling<TranslateText>(request);

            var response = new TranslateTextResponse
            {
                TranslatedText = systranResponse.Outputs.FirstOrDefault()?.Output ?? string.Empty
            };
            return response;
        }



        [BlueprintActionDefinition(BlueprintAction.TranslateFile)]
        [Action("Translate", Description = "Translate a file using Blackbird or Systran native strate")]
        public async Task<FileReferenceResponse> Translate([ActionParameter] TranslateFileRequest input)
        {
            var strategy = input.FileTranslationStrategy?.ToLowerInvariant() ?? "blackbird";

            if (strategy == "systran")
            {
                return await TranslateWithSystranNative(input);
            }
            else // "blackbird"
            {
                try
                {
                    using var stream = await _fileManagementClient.DownloadAsync(input.File);
                    var content = await Transformation.Parse(stream, input.File.Name);

                    return await HandleInteroperableTransformation(content, input);
                }
                catch (Exception e) when (e.Message.Contains("not supported", StringComparison.OrdinalIgnoreCase))
                {
                    throw new PluginMisconfigurationException(
                        "The file format is not supported by the Blackbird interoperable setting. " +
                        "Try setting the file translation strategy to Systran native.");
                }
            }
        }

        private async Task<FileReferenceResponse> HandleInteroperableTransformation(Transformation content, TranslateFileRequest input)
        {
            content.SourceLanguage ??= input.Source;
            content.TargetLanguage ??= input.TargetLanguage;

            if (content.SourceLanguage == null || content.TargetLanguage == null)
                throw new PluginMisconfigurationException("Source or target language not defined.");

            var segments = content.GetSegments()
                .Where(s => s.Ignorable != true && s.IsInitial)
                .ToList();

            if (segments.Count == 0)
            {
                if (string.Equals(input.OutputFileHandling, "original", StringComparison.OrdinalIgnoreCase))
                {
                    var targetContent = content.Target();
                    var outEmpty = await _fileManagementClient.UploadAsync(
                        targetContent.Serialize().ToStream(),
                        targetContent.OriginalMediaType,
                        targetContent.OriginalName);
                    return new FileReferenceResponse { File = outEmpty };
                }

                var xliffEmpty = content.Serialize();
                var xliffFileEmpty = await _fileManagementClient.UploadAsync(
                    xliffEmpty.ToStream(),
                    MediaTypes.Xliff,
                    content.XliffFileName);
                return new FileReferenceResponse { File = xliffFileEmpty };
            }

            const int batchSize = 100;
            for (int offset = 0; offset < segments.Count; offset += batchSize)
            {
                var batch = segments.Skip(offset).Take(batchSize).ToList();
                var translations = new List<string>(batch.Count);

                foreach (var seg in batch)
                {
                    var sourceText = seg.GetSource() ?? string.Empty;

                    if (string.IsNullOrWhiteSpace(sourceText))
                    {
                        translations.Add(string.Empty);
                        continue;
                    }

                    var request = new SystranRequest("/translation/text/translate", Method.Post);
                    request.AddQueryParameter("input", sourceText);
                    request.AddQueryParameter("source", string.IsNullOrWhiteSpace(input.Source) ? "auto" : input.Source);
                    request.AddQueryParameter("target", input.TargetLanguage);

                    if (!string.IsNullOrEmpty(input.Profile))
                        request.AddQueryParameter("profile", input.Profile);

                    var response = await Client.ExecuteWithErrorHandling<TranslateText>(request);
                    translations.Add(response.Outputs.FirstOrDefault()?.Output ?? string.Empty);
                }

                for (int i = 0; i < batch.Count; i++)
                {
                    var translatedText = translations[i];
                    if (!string.IsNullOrEmpty(translatedText))
                    {
                        var segment = batch[i];
                        segment.SetTarget(translatedText);
                        segment.State = SegmentState.Translated;
                    }
                }
            }

            if (string.Equals(input.OutputFileHandling, "original", StringComparison.OrdinalIgnoreCase))
            {
                var targetContent = content.Target();
                var outFile = await _fileManagementClient.UploadAsync(
                    targetContent.Serialize().ToStream(),
                    targetContent.OriginalMediaType,
                    targetContent.OriginalName);

                return new FileReferenceResponse { File = outFile };
            }

            var xliff = content.Serialize();
            var xliffFile = await _fileManagementClient.UploadAsync(
                xliff.ToStream(),
                MediaTypes.Xliff,
                content.XliffFileName);

            return new FileReferenceResponse { File = xliffFile };
        }

        private async Task<FileReferenceResponse> TranslateWithSystranNative(TranslateFileRequest input)
        {
            var inputType = input.File.ContentType;
            if (!string.IsNullOrWhiteSpace(inputType) && !InputFormats.Contains(inputType))
                throw new PluginMisconfigurationException($"Unsupported file format: {inputType}. Please provide a supported format.");

            var request = new SystranRequest("/translation/file/translate", Method.Post)
            {
                AlwaysMultipartFormData = true
            };

            if (!string.IsNullOrEmpty(input.Source))
                request.AddQueryParameter("source", input.Source);

            if (!string.IsNullOrEmpty(input.TargetLanguage))
                request.AddQueryParameter("target", input.TargetLanguage);

            if (!string.IsNullOrEmpty(input.Profile))
                request.AddQueryParameter("profile", input.Profile);

            using var fileStream = await _fileManagementClient.DownloadAsync(input.File);
            request.AddFile("input", () => fileStream, input.File.Name);

            var rawResponse = await Client.ExecuteAsync(request);

            if (!rawResponse.IsSuccessful || rawResponse.RawBytes == null)
                throw new PluginApplicationException($"Failed to translate file. Status: {rawResponse.StatusCode}, Error: {rawResponse.ErrorMessage}");

            var contentType = string.IsNullOrWhiteSpace(rawResponse.ContentType) ? "application/octet-stream" : rawResponse.ContentType;

            var translatedFile = await _fileManagementClient.UploadAsync(
                new MemoryStream(rawResponse.RawBytes),
                contentType,
                $"{Path.GetFileNameWithoutExtension(input.File.Name)}_translated{Path.GetExtension(input.File.Name)}");

            return new FileReferenceResponse { File = translatedFile };
        }

        private long GetInputSize(string input)
        {
            return System.Text.Encoding.UTF8.GetByteCount(input);
        }
    }
}
