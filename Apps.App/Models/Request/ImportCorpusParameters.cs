using Apps.Systran.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Systran.Models.Request
{
    public class ImportCorpusParameters
    {
        [Display("Corpus name")]
        public string Name { get; set; }

        [Display("Format")]
        [StaticDataSource(typeof(CorpusFormatDataHandler))]
        public string Format { get; set; }

        [Display("Input",Description = "Content of the existing corpus")]
        public string? Input { get; set; }

        [Display("Input file")]
        public FileReference? InputFile { get; set; }

        [Display("Tag")]
        public string[]? Tag { get; set; }

    }
}
