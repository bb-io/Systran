using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace Apps.App.Api
{
    public class SystranRequest : RestRequest
    {
        public SystranRequest(string resource, Method method) : base(resource, method)
        {
        }
    }
}
