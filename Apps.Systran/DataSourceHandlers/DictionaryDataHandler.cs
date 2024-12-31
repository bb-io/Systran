using Apps.App.Api;
using Apps.App.Invocables;
using Apps.Systran.Models.Response;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Systran.DataSourceHandlers
{
    public class DictionaryDataHandler : SystranInvocable, IAsyncDataSourceItemHandler
    {
        public DictionaryDataHandler(InvocationContext invocationContext) : base(invocationContext) { }
        public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
        {
            var request = new SystranRequest("/resources/dictionary/list", RestSharp.Method.Post);
            var response = await Client.ExecuteWithErrorHandling<DictionaryDataHandlerResponse>(request);

            return response.Dictionaries
                .Where(dictionary => string.IsNullOrWhiteSpace(context.SearchString) ||
                                     dictionary.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
                .Take(50)
                .Select(dictionary => new DataSourceItem(
                    dictionary.Id,
                    $"{dictionary.Name} ({dictionary.SourceLang} -> {dictionary.TargetLangs})"
                ));
        }
    }
}
