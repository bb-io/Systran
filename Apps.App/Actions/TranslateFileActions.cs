using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.App.Api;
using Apps.App.Invocables;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;
using Apps.Systran.Models.Request;
using Apps.Systran.Models.Response;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;

namespace Apps.Systran.Actions
{
    [ActionList]
    public class TranslateFileActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : SystranInvocable(invocationContext)
    {

        [Action("Translate file", Description = "Translate a file from source language to target language")]
        public async Task<TranslateFileResponse> TranslateFile([ActionParameter] TranslateFileRequest input)
        {
            var baseUrl = InvocationContext.AuthenticationCredentialsProviders
                .First(p => p.KeyName == "url").Value;

            var request = new SystranRequest("/translation/file/translate", Method.Post)
            {
                AlwaysMultipartFormData = true
            };

            if (!string.IsNullOrEmpty(input.Source))
                request.AddQueryParameter("source", input.Source);
            if (!string.IsNullOrEmpty(input.Target))
                request.AddQueryParameter("target", input.Target);
            if (!string.IsNullOrEmpty(input.Format))
                request.AddQueryParameter("format", input.Format);
            if (!string.IsNullOrEmpty(input.Profile))
                request.AddQueryParameter("profile", input.Profile);
            if (input.WithInfo == true)
                request.AddQueryParameter("withInfo", "true");
            if (input.WithSource == true)
                request.AddQueryParameter("withSource", "true");
            if (input.WithAnnotations == true)
                request.AddQueryParameter("withAnnotations", "true");
            if (input.Async == true)
                request.AddQueryParameter("async", "true");
            if (!string.IsNullOrEmpty(input.Owner))
                request.AddQueryParameter("owner", input.Owner);
            if (!string.IsNullOrEmpty(input.Domain))
                request.AddQueryParameter("domain", input.Domain);
            if (!string.IsNullOrEmpty(input.Size))
                request.AddQueryParameter("size", input.Size);



            var response = await Client.ExecuteWithErrorHandling<TranslateFileResponse>(request);

            return response;
        }
    }
}

