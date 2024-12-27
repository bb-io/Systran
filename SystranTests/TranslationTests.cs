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
    }
}
