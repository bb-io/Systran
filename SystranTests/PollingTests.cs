using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.Systran.Models.Request;
using Apps.Systran.Models;
using Apps.Systran.Polling.Models;
using Apps.Systran.Polling;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;
using SystranTests.Base;
using Apps.Systran.Actions;

namespace Tests.Systran
{
    [TestClass]
    public class PollingTests : TestBase
    {
        [TestMethod]
        public async Task TranslateFileAsync_WithPolling_ReturnsTranslatedFile()
        {
            // Arrange
            var fileReference = await FileManager.UploadTestFileAsync("Translate.txt", "text/plain");

            var inputRequest = new TranslateFileRequest
            {
                Input = fileReference,
                Profile = null
            };

            var inputOptions = new TranslateLanguagesOptions
            {
                Source = "en",
                Target = "fr"
            };

            var actions = new TranslationPollingList(InvocationContext, FileManager);

            // Act
            var translateAction = new TranslateFileActions(InvocationContext, FileManager);
            var translateResponse = await translateAction.TranslateFileAsync(inputOptions, inputRequest);

            Assert.IsNotNull(translateResponse, "Translate response is null.");
            Assert.IsFalse(string.IsNullOrEmpty(translateResponse.RequestId), "Request ID is null or empty.");

            var requestId = translateResponse.RequestId;
            PollingEventResponse<TranslateFileMemory, TranslationResultResponse> pollingResponse = null;

            for (int i = 0; i < 1; i++)
            {
                var pollingRequest = new PollingEventRequest<TranslateFileMemory>
                {
                    Memory = new TranslateFileMemory
                    {
                        LastPollingTime = DateTime.UtcNow,
                        Triggered = false
                    }
                };

                pollingResponse = await actions.OnTranslationFinished(pollingRequest, requestId);

                if (pollingResponse.FlyBird) 
                {
                    break;
                }

                await Task.Delay(2000);
            }

            // Assert
            Assert.IsNotNull(pollingResponse, "Polling response is null.");
            Assert.IsTrue(pollingResponse.FlyBird, "Polling did not finish successfully.");
            Assert.IsNotNull(pollingResponse.Result, "Polling result is null.");
            Assert.AreEqual("finished", pollingResponse.Result.Status, "Translation status is not 'finished'.");
            Assert.IsNotNull(pollingResponse.Result.File, "Translated file is null.");
            Assert.IsFalse(string.IsNullOrEmpty(pollingResponse.Result.File.Name), "Translated file name is null or empty.");
        }
    }
}
