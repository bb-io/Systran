using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Systran.Models.Response
{
    public class TranslateTextResponse
    {
        [Display("Request ID")]
        public string RequestId { get; set; } = string.Empty;

        [Display("Outputs")]
        public List<TranslationOutput> Outputs { get; set; } = new();
    }

    public class TranslationOutput
    {
        [Display("Info")]
        public TranslationInfo? Info { get; set; }

        [Display("Translated text")]
        public string Output { get; set; } = string.Empty;

        [Display("Back translation")]
        public string? BackTranslation { get; set; }

        [Display("Source text")]
        public string? Source { get; set; }
    }

    public class TranslationInfo
    {
        [Display("Language identification")]
        public LanguageIdentification? Lid { get; set; }

    }

    public class LanguageIdentification
    {
        [Display("Language")]
        public string Language { get; set; } = string.Empty;

        [Display("Confidence level")]
        public float Confidence { get; set; }
    }
}
