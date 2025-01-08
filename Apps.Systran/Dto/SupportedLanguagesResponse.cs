using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Apps.Systran.Dto
{
    public class SupportedLanguagesResponse
    {
        [JsonProperty("languagePairs")]
        public List<LanguagePair> LanguagePairs { get; set; } = new();
    }

    public class LanguagePair
    {
        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("target")]
        public string Target { get; set; } 
    }
}
