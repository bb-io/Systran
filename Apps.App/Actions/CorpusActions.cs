using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
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
        public async Task<FileReference> ExportCorpus([ActionParameter] ExportCorpusParameters parameters)
        {
            var request = new SystranRequest($"/resources/corpus/export", RestSharp.Method.Get);
            request.AddQueryParameter("corpusId", parameters.CorpusId);

            var response = await Client.ExecuteAsync(request, cancellationToken: default);
            if (!response.IsSuccessful || response.RawBytes == null)
                throw new Exception($"Failed to export corpus. Status: {response.StatusCode}, Error: {response.ErrorMessage}");


            await using var memoryStream = new MemoryStream(response.RawBytes);

            var fileReference = await fileManagementClient.UploadAsync(
                memoryStream,
                response.ContentType,
                parameters.CorpusId 
             );

            return fileReference;
        }

        [Action("Import corpus from TMX file", Description = "Add a new corpus from an existing corpus.")]
        public async Task<ImportCorpusResponse> ImportCorpus([ActionParameter] ImportCorpusParameters parameters)
        {
            var request = new SystranRequest($"/resources/corpus/import", RestSharp.Method.Post);

            request.AddQueryParameter("name", parameters.Name);
            request.AddQueryParameter("format", parameters.Format);

            if (parameters.Tag != null && parameters.Tag.Any())
            {
                foreach (var tag in parameters.Tag)
                {
                    request.AddQueryParameter("tag", tag);
                }
            }

            if (!string.IsNullOrEmpty(parameters.Input))
            {
                request.AddParameter("input", parameters.Input);
            }
            else if (parameters.InputFile != null)
            {
                var fileBytes = await fileManagementClient.DownloadAsync(parameters.InputFile);
                using (var memoryStream = new MemoryStream())
                {
                    await fileBytes.CopyToAsync(memoryStream); 
                    request.AddFile("inputFile", memoryStream.ToArray(), parameters.InputFile.Name, MediaTypeNames.Application.Octet);
                }
            }
            else
            {
                throw new PluginMisconfigurationException("Either input or inputFile must be provided");
            }

            var response = await Client.ExecuteAsync<ImportCorpusResponse>(request);

            return response.Data;
        }
    }


    
}
