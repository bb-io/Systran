using Apps.Systran.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Systran.Models.Request
{
    public class TranslateTextRequest
    {
        [Display("Input text")]
        public string Input { get; set; }

        [Display("Profile ID")]
        public string? Profile { get; set; }

        [Display("With info")]
        public bool? WithInfo { get; set; }

        [Display("With source")]
        public bool? WithSource { get; set; }

        [Display("With annotations")]
        public bool? WithAnnotations { get; set; }

        [Display("Back translation")]
        public bool? BackTranslation { get; set; }
    }
}
