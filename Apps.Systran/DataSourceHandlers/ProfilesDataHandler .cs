using Apps.App.Api;
using Apps.App.Invocables;
using Apps.Systran.Models.Response;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Systran.DataSourceHandlers
{
    public class ProfilesDataHandler : SystranInvocable, IAsyncDataSourceItemHandler
    {
        public ProfilesDataHandler(InvocationContext invocationContext) : base(invocationContext) { }

        public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
        {
            var request = new SystranRequest("/profiles", RestSharp.Method.Get);
            var response = await Client.ExecuteWithErrorHandling<ProfilesListResponse>(request);

            if (response?.Profiles == null || !response.Profiles.Any())
            {
                throw new Exception("No profiles found.");
            }

            return response.Profiles
                .Where(profile => string.IsNullOrWhiteSpace(context.SearchString) ||
                                  profile.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
                .Select(profile => new DataSourceItem(
                    profile.Id,
                    $"{profile.Name} ({profile.Source} -> {profile.Target})"
                ));
        }
    }
}
