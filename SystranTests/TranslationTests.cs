using Apps.Systran.Actions;
using Apps.Systran.Models.Request;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;
using System.Net.Mime;
using System.Net;
using System.Text;
using SystranTests.Base;
using Moq;
using Apps.Systran.Models.Response;
using Apps.App.Actions;
using Apps.Systran.Models;

namespace SystranTests
{
    [TestClass]
    public class TranslationTests : TestBase
    {
        [TestMethod]
        public async Task TranslateTextReturnsValidResponse()
        {
            // Arrange
            var inputOptions = new TranslateLanguagesOptions
            {
                Source = "en",
                Target = "fr"
            };

            var inputRequest = new TranslateTextRequest
            {
                Input = "daughter",
                WithInfo = true
            };

            var actions = new TranslateTextActions(InvocationContext);

            // Act
            var result = await actions.TranslateText(inputOptions, inputRequest);

            // Assert
            Assert.IsNotNull(result, "Response is null.");
        }


        [TestMethod]
        public async Task TranslateFile_ValidFile_ReturnsResponse()
        {
            // Arrange
            var fileReference = await FileManager.UploadTestFileAsync("Translate.txt");

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

            var actions = new TranslateFileActions(InvocationContext, FileManager);

            // Act
            var result = await actions.TranslateFile(inputOptions, inputRequest);

            // Assert
            Assert.IsNotNull(result, "Response is null.");
            Assert.IsNotNull(result.File, "Translated file is null.");
        }

        [TestMethod]
        public async Task TranslateFileAsync_ValidInput_ReturnsRequestId()
        {
            // Arrange
            var fileReference = await FileManager.UploadTestFileAsync("Translate.txt");

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

            var actions = new TranslateFileActions(InvocationContext, FileManager);
            // Act
            var result = await actions.TranslateFileAsync(inputOptions, inputRequest);

            // Assert
            Assert.IsNotNull(result, "Response is null.");
            Assert.IsFalse(string.IsNullOrEmpty(result.RequestId), "Request ID is empty or null.");
        }

    }
}
