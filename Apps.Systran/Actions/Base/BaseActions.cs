using Apps.App.Api;
using Apps.App.Invocables;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;

namespace Apps.Systran.Actions.Base
{
    public abstract class BaseActions : SystranInvocable
    {
        protected readonly SystranClient Client;
        protected readonly IFileManagementClient FileManagementClient;

        protected BaseActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
        : base(invocationContext)
        {
            Client = new SystranClient(invocationContext.AuthenticationCredentialsProviders);
            FileManagementClient = fileManagementClient;
        }
    }
}
