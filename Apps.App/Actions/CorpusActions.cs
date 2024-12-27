using System.Net.Mime;
using Apps.App.Api;
using Apps.App.Invocables;
using Apps.Systran.Models.Request;
using Apps.Systran.Models.Response;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;

namespace Apps.Systran.Actions
{
    [ActionList]
    public class CorpusActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : SystranInvocable(invocationContext)
    {

        [Action("Export corpus", Description = "Export a corpus file by its ID")]
        public async Task<FileReferenceResponse> ExportCorpus([ActionParameter] ExportCorpusParameters parameters)
        {
            var request = new SystranRequest($"/resources/corpus/export", RestSharp.Method.Get);
            request.AddQueryParameter("corpusId", parameters.CorpusId);

            var response = await Client.DownloadStreamAsync(request);
            if (response == null)
                throw new PluginApplicationException($"Failed to export corpus. Response stream is null.");


            var fileReference = await fileManagementClient.UploadAsync(
                    response,
                    "application/x-tmx+xml",
                    parameters.CorpusId
                );

            return new FileReferenceResponse{
                FileResponse = fileReference
            };
        }

        [Action("Import corpus from TMX file", Description = "Add a new corpus from an existing corpus.")]
        public async Task<ImportCorpusResponse> ImportCorpus([ActionParameter] ImportCorpusParameters parameters)
        {
            if (parameters.InputFile == null)
                throw new PluginMisconfigurationException("Input file must be provided.");

            var fileStream = await fileManagementClient.DownloadAsync(parameters.InputFile);

            var request = new SystranRequest($"/resources/corpus/import", RestSharp.Method.Post)
            {
                AlwaysMultipartFormData = true
            };

            request.AddQueryParameter("name", parameters.Name);
            request.AddQueryParameter("format", "application/x-tmx+xml");

            if (parameters.Tag != null && parameters.Tag.Any())
            {
                foreach (var tag in parameters.Tag)
                {
                    request.AddQueryParameter("tag", tag);
                }
            }

            request.AddFile("inputFile", () => fileStream, parameters.InputFile.Name, "application/x-tmx+xml");

            var response = await Client.ExecuteWithErrorHandling<ImportCorpusResponse>(request);

            return response;
        }
    }



}
