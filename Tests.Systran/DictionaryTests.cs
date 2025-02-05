using Apps.Systran.Actions;
using Apps.Systran.Models;
using Apps.Systran.Models.Request;
using SystranTests.Base;

namespace Tests.Systran
{
    [TestClass]
    public class DictionaryTests : TestBase
    {
        [TestMethod]
        public async Task CreateDictionary_ValidFile_ReturnsResponse()
        {
            // Arrange
            var fileReference = await FileManager.UploadTestFileAsync("english_to_french.tbx");

            var parameters = new CreateDictionaryParameters
            {
                Name = "YOUR_DICTIONARY_NAME",
                SourcePos = "noun",
                Comment = "Test comment",
                Type = "UD",
                TbxFile = fileReference
            };

            var options = new TranslateLanguagesOptions
            {
                Source = "en",
                Target = "es"
            };

            var actions = new DictionaryActions(InvocationContext, FileManager);

            // Act
            var result = await actions.CreateDictionary(parameters, options);

            // Assert
            Assert.IsNotNull(result, "Response is null.");
            Assert.IsNotNull(result.Added, "Added dictionary data is null.");
            Assert.IsFalse(string.IsNullOrEmpty(result.Added.Id), "Dictionary ID is missing.");
        }


        [TestMethod]
        public async Task ExportDictionaryToTbx_ValidDictionaryId_ReturnsTbxFile()
        {
            // Arrange
            var parameters = new ExportDictionaryRequest
            {
                DictionaryId = "YOUR_DICTIONARY_ID",
            };

            var actions = new DictionaryActions(InvocationContext, FileManager);

            // Act
            var result = await actions.ExportDictionary(parameters);

            // Assert
            Assert.IsNotNull(result, "Response is null.");
            Assert.IsNotNull(result.FileResponse, "TBX file is null.");
            Assert.IsFalse(string.IsNullOrEmpty(result.FileResponse.Name), "TBX file name is empty.");
        }

        [TestMethod]
        public async Task UpdateDictionary_ValidTbxFile_UpdatesDictionary()
        {
            // Arrange
            var parameters = new UpdateDictionaryRequest
            {
                DictionaryId = "YOUR_DICTIONARY_ID",
                File = await FileManager.UploadTestFileAsync("test.tbx", "application/x-tbx+xml")
            };

            var options = new TranslateLanguagesOptions
            {
                Source = "en",
                Target = "es"
            };

            var actions = new DictionaryActions(InvocationContext, FileManager);

            // Act
            var result = await actions.UpdateDictionary(parameters, options);

            // Assert
            Assert.IsNotNull(result, "Response is null.");
            Assert.IsTrue(result.Success, "Update operation failed.");
            Assert.IsFalse(string.IsNullOrEmpty(result.Message), "Response message is empty.");
        }
    }
}
