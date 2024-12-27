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
        public async Task ImportCorpus_ValidFile_ReturnsResponse()
        {
            // Arrange
            var projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
            var testFilePath = Path.Combine(projectDirectory, "TestFiles", "test.tmx");

            Assert.IsTrue(File.Exists(testFilePath), $"Test file not found at: {testFilePath}");

            using var fileStream = new FileStream(testFilePath, FileMode.Open, FileAccess.Read);
            var fileReference = await FileManager.UploadAsync(
                fileStream,
                "application/x-tmx+xml",
                "test.tmx"
            );

            var parameters = new ImportCorpusParameters
            {
                Name = "Test Corpus",
                InputFile = fileReference,
                Tag = new[] { "tag1", "tag2" }
            };

            var actions = new CorpusActions(InvocationContext, FileManager);


            // Act
            var result = await actions.ImportCorpus(parameters);

            // Assert
            Assert.IsNotNull(result, "Response is null.");
            Assert.IsNotNull(result.Corpus, "Corpus data is null.");
            Assert.IsFalse(string.IsNullOrEmpty(result.Corpus.Id), "Corpus ID is missing.");

        }

        [TestMethod]
        public async Task ExportCorpus_ValidCorpusId_ReturnsFileReference()
        {
            var testFileName = "exported-corpus.tmx";
            var corpusId = "676d81b85f6f31e36a019213";
            var expectedContentType = "application/x-tmx+xml";

            var parameters = new ExportCorpusParameters
            {
                CorpusId = corpusId
            };

            var actions = new CorpusActions(InvocationContext, FileManager);

            var result = await actions.ExportCorpus(parameters);

            Assert.IsNotNull(result, "Response is null.");
            Assert.IsNotNull(result.FileResponse, "FileResponse is null.");
        }


        [TestMethod]
        public async Task TranslateFile_ValidFile_ReturnsResponse()
        {
            // Arrange
            var projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
            var testFilePath = Path.Combine(projectDirectory, "TestFiles", "Translate.txt");

            Assert.IsTrue(File.Exists(testFilePath), $"Test file not found at: {testFilePath}");

            using var fileStream = new FileStream(testFilePath, FileMode.Open, FileAccess.Read);
            var fileReference = await FileManager.UploadAsync(
                fileStream,
                "text/plain",
                "Translate_output.txt"
            );

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

    }
}
