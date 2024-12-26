using Apps.Systran.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Systran.Models.Request
{
    public class TranslateFileRequest
    {
        [Display("Input file")]
        public FileReference Input { get; set; }

        [Display("Source language code")]
        public string? Source { get; set; }

        [Display("Target language code")]
        public string Target { get; set; }

        [Display("Format")]
        public string? Format { get; set; }

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

        [Display("Async")]
        [StaticDataSource(typeof(YesNoDataHandler))]
        public bool? Async { get; set; }

        [Display("Owner")]
        public string? Owner { get; set; }

        [Display("Domain")]
        public string? Domain { get; set; }

        [Display("Size")]
        public string? Size { get; set; }
    }
}
