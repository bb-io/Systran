using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.Systran.Actions;
using Apps.Systran.Models.Request;
using Apps.Systran.Models;
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
            var fileReference = await FileManager.UploadTestFileAsync("test.tbx", "application/x-tbx+xml");

            var parameters = new CreateDictionaryParameters
            {
                Name = "Test Dictionary1230",
                SourcePos = "noun",
                Comment = "Test comment",
                TbxFile = fileReference
            };

            var options = new TranslateLanguagesOptions
            {
                Source = "en",
                Target = "fr"
            };

            var actions = new DictionaryActions(InvocationContext, FileManager);

            // Act
            var result = await actions.CreateDictionary(parameters, options);

            // Assert
            Assert.IsNotNull(result, "Response is null.");
            Assert.IsNotNull(result.Added, "Added dictionary data is null.");
            Assert.IsFalse(string.IsNullOrEmpty(result.Added.Id), "Dictionary ID is missing.");
        }
    }
}
