using Apps.Systran.Actions;
using Apps.Systran.Models;
using Apps.Systran.Models.Request;
using Blackbird.Applications.Sdk.Common.Files;
using SystranTests.Base;

namespace Tests.Systran
{
    [TestClass]
    public class TranslationTests : TestBase
    {
        [TestMethod]
        public async Task TranslateTextReturnsValidResponse()
        {
            // Arrange
            var inputRequest = new TranslateTextRequest
            {
                Text = "YOUR_INPUT_TEXT",
                WithInfo = true,
                Source = "en",
                TargetLanguage = "fr"
            };

            var actions = new TranslationActions(InvocationContext, FileManager);

            // Act
            var result = await actions.TranslateText(inputRequest);

            // Assert
            Assert.IsNotNull(result, "Response is null.");
        }


        [TestMethod]
        public async Task TranslateFile_ValidFile_ReturnsResponse()
        {
            // Arrange
            var fileReference = await FileManager.UploadTestFileAsync("Client.txt");

            var inputRequest = new TranslateFileRequest
            {
                File = fileReference,
                Profile = null
            };

            var actions = new TranslateFileActions(InvocationContext, FileManager);

            // Act
            var result = await actions.TranslateFile(inputRequest);

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
                File = fileReference,
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


        [TestMethod]
        public async Task Translate_ValidInput_ReturnsRequestId()
        {
            var action = new TranslationActions(InvocationContext, FileManager);

            var response = await action.Translate(new TranslateFileRequest
            {
                File = new FileReference { Name = "contentful.html 1.2.xliff" },
                Source = "en",
                TargetLanguage = "fr",
                //OutputFileHandling = "original",
                FileTranslationStrategy = "blackbird"
            });

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            Console.WriteLine(json);
            Assert.IsNotNull(response);
        }

    }
}
