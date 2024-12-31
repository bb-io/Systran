using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Systran.Models.Response
{
    public class ImportCorpusResponse
    {
        [JsonProperty("corpus")]
        public CorpusInfo Corpus { get; set; }
    }
    public class CorpusInfo
    {
        [Display("Coprus ID")]
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
