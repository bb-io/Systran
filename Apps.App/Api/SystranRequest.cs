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
