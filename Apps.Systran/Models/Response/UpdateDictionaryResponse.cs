using Blackbird.Applications.Sdk.Common;

namespace Apps.Systran.Models.Response
{
    public class UpdateDictionaryResponse
    {
        [Display("Success")]
        public bool Success { get; set; }

        [Display("Message")]
        public string Message { get; set; }
    }
}
