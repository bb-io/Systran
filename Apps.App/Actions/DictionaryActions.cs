using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.App.Api;
using Apps.App.Invocables;
using Apps.Systran.Models.Request;
using Apps.Systran.Models.Response;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Glossaries.Utils.Converters;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;

namespace Apps.Systran.Actions
{
    [ActionList]
    public class DictionaryActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : SystranInvocable(invocationContext)
    {
        
    }
}

