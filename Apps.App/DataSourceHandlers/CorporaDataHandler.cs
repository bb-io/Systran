using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.App.Api;
using Apps.App.Invocables;
using Apps.Systran.Models.Response;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Systran.DataSourceHandlers
{
    public class CorporaDataHandler : SystranInvocable, IAsyncDataSourceItemHandler
    {
        public CorporaDataHandler(InvocationContext invocationContext) : base(invocationContext) { }

        public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
        {
            var request = new SystranRequest("/resources/corpus/list", RestSharp.Method.Get);
            var response = await Client.ExecuteWithErrorHandling<CorporaListResponse>(request);

            return response.Files
                .Where(corpus => string.IsNullOrWhiteSpace(context.SearchString) ||
                                 corpus.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
                .Take(50)
                .Select(corpus => new DataSourceItem(
                    corpus.Id,
                    $"{corpus.Name} ({corpus.SourceLang} -> {string.Join(", ", corpus.TargetLangs ?? Enumerable.Empty<string>())})"
                ));
        }
    }
}
