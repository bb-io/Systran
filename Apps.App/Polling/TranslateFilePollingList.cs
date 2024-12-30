using Apps.App.Api;
using Apps.Systran.Actions.Base;
using Apps.Systran.Polling.Models;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;

namespace Apps.Systran.Polling
{
    [PollingEventList]
    public class TranslationPollingList(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : BaseActions(invocationContext, fileManagementClient)
    {

        [PollingEvent("On translation finished", "Triggered when the translation status is finished")]
        public async Task<PollingEventResponse<TranslateFileMemory, TranslationResultResponse>> OnTranslationFinished(
            PollingEventRequest<TranslateFileMemory> request,
            [PollingEventParameter][Display("Request ID")] string requestId)
        {
            if (request.Memory is null)
            {
                return new()
                {
                    FlyBird = false,
                    Memory = new()
                    {
                        LastPollingTime = DateTime.UtcNow,
                        Triggered = false
                    }
                };
            }

            var statusRequest = new SystranRequest($"/translation/file/status", Method.Get);
            statusRequest.AddQueryParameter("requestId", requestId);
            var statusResponse = await Client.ExecuteAsync<TranslationStatusResponse>(statusRequest);

            if (statusResponse.Data == null ||
                !statusResponse.Data.Status.Equals("finished", StringComparison.OrdinalIgnoreCase))
            {
                return new()
                {
                    FlyBird = false,
                    Memory = new()
                    {
                        LastPollingTime = DateTime.UtcNow,
                        Triggered = false
                    }
                };
            }


            var resultRequest = new SystranRequest($"/translation/file/result", Method.Get); ;
            resultRequest.AddQueryParameter("requestId", requestId);
            var resultResponse = await Client.ExecuteAsync<TranslationResultResponse>(resultRequest);

            var rawResponse = await Client.ExecuteAsync(resultRequest);

            if (!rawResponse.IsSuccessful || rawResponse.RawBytes == null)
            {
                throw new PluginApplicationException($"Failed to retrieve the translation result. Status: {rawResponse.StatusCode}, Error: {rawResponse.ErrorMessage}");
            }

            var translatedFile = await FileManagementClient.UploadAsync(
               new MemoryStream(rawResponse.RawBytes),
               rawResponse.ContentType,
               $"{requestId}");


            return new()
            {
                FlyBird = true,
                Result = new TranslationResultResponse
                {
                    RequestId = requestId,
                    Status = "finished",
                    FinishedAt = DateTime.UtcNow,
                    File = translatedFile
                },
                Memory = new()
                {
                    LastPollingTime = DateTime.UtcNow,
                    Triggered = true
                }
            };
        }
    }

}
