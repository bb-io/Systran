
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.SDK.Blueprints.Interfaces.Translate;
using Newtonsoft.Json;

namespace Apps.Systran.Models.Response
{
    public class TranslateTextResponse : ITranslateTextOutput
    {
        [Display("Translated text")]
        [JsonProperty("output")]
        public string TranslatedText {  get; set; }
    }
}
