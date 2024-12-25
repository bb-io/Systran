using Apps.Systran.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Systran.Models.Request
{
    public class TranslateTextRequest
    {
        [Display("Input text")]
        public List<string> Input { get; set; }

        [Display("Source language code")]
        [StaticDataSource(typeof(LanguageCodeDataHandler))]
        public string? Source { get; set; }

        [Display("Target language code")]
        [StaticDataSource(typeof(LanguageCodeDataHandler))]
        public string Target { get; set; }

        [Display("Profile UUID")]
        public string? Profile { get; set; }

        [Display("With info")]
        [StaticDataSource(typeof(YesNoDataHandler))]
        public bool? WithInfo { get; set; }

        [Display("With source")]
        [StaticDataSource(typeof(YesNoDataHandler))]
        public bool? WithSource { get; set; }

        [Display("With annotations")]
        [StaticDataSource(typeof(YesNoDataHandler))]
        public bool? WithAnnotations { get; set; }

        [Display("Back translation")]
        [StaticDataSource(typeof(YesNoDataHandler))]
        public bool? BackTranslation { get; set; }
    }
}
