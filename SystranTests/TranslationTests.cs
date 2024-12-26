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

namespace SystranTests
{
    [TestClass]
    public class TranslationTests : TestBase
    {
        [TestMethod]
        public async Task TranslateTextReturnsValidResponse()
        {
            // Arrange
            var input = new TranslateTextRequest
            {
                Input = new List<string> { "Hello, world!" },
                Source = "en",
                Target = "de",
                WithInfo = true,

            };
            var actions = new TranslateTextActions(InvocationContext);

            // Act
            var result =await actions.TranslateText(input);

            // Assert
            Assert.IsNotNull(result, "Response is null.");
            Assert.AreEqual(1, result.Outputs.Count, "Unexpected number of outputs.");
            Assert.AreEqual("¡Hola, mundo!", result.Outputs.First().Output, "Translation does not match expected value.");
        }
    }
}
