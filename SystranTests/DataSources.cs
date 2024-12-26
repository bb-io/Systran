using Apps.Systran.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common.Dynamic;
using SystranTests.Base;

namespace SystranTests
{
    [TestClass]
    public class DataSources : TestBase
    {
        [TestMethod]
        public async Task DictionaryDataHandlerReturnsValues()
        {
            //Arrange
            var handler = new DictionaryDataHandler(InvocationContext);

            //Act
            var data = await handler.GetDataAsync(new DataSourceContext { SearchString = "" }, CancellationToken.None);

            //Assert
            foreach (var item in data)
            {
                Console.WriteLine($"{item.Value}: {item.DisplayName}");
            }

            Assert.IsNotNull(data);
            Assert.AreNotEqual(0, data.Count(), "No corpora were returned.");
        }

        [TestMethod]
        public async Task CorporaDataHandlerReturnsValues()
        {
            // Arrange
            var handler = new CorporaDataHandler(InvocationContext);

            // Act
            var data = await handler.GetDataAsync(new DataSourceContext { SearchString = "" }, CancellationToken.None);

            // Assert
            foreach (var item in data)
            {
                Console.WriteLine($"{item.Value}: {item.DisplayName}");
            }

            Assert.IsNotNull(data, "Handler returned null.");
            Assert.AreNotEqual(0, data.Count(), "No corpora were returned.");
        }

    }
}
