

using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Systran.Models.Response
{
    public class TranslateTextResponse
    {
        [Display("Translated text")]
        [JsonProperty("output")]
        public string Output {  get; set; }
    }
}
