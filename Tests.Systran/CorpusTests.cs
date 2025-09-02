using Apps.Systran.Actions;
using Apps.Systran.Models.Request;
using SystranTests.Base;

namespace Tests.Systran
{
    [TestClass]
    public class CorpusTests : TestBase
    {
        [TestMethod]
        public async Task ImportCorpus_ValidFile_ReturnsResponse()
        {
            // Arrange
            var fileReference = await FileManager.UploadTestFileAsync("test.tmx", "application/x-tmx+xml");


            var parameters = new ImportCorpusParameters
            {
                Name = "YOUR_CORPUS_NAME",
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
            var corpusId = "YOUR_CORPUS_ID";
            var expectedContentType = "application/x-tmx+xml";

            var parameters = new ExportCorpusParameters
            {
                CorpusId = corpusId
            };

            var actions = new CorpusActions(InvocationContext, FileManager);

            var result = await actions.ExportCorpus(parameters);

            Assert.IsNotNull(result, "Response is null.");
            Assert.IsNotNull(result.File, "FileResponse is null.");
        }
    }
}
