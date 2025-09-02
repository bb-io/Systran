using Apps.Systran.DataSourceHandlers;
using Apps.Systran.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.SDK.Blueprints.Handlers;
using Blackbird.Applications.SDK.Blueprints.Interfaces.Translate;

namespace Apps.Systran.Models.Request
{
    public class TranslateFileRequest : ITranslateFileInput
    {
        [Display("File")]
        public FileReference File { get; set; }

        [Display("Profile ID")]
        [DataSource(typeof(ProfilesDataHandler))]
        public string? Profile { get; set; }

        [Display("Source language")]
        [DataSource(typeof(SupportedLanguagesDataHandler))]
        public string Source { get; set; }

        [Display("Target language")]
        [DataSource(typeof(SupportedLanguagesDataHandler))]
        public string TargetLanguage { get; set; }

        [Display("Output file handling", Description = "Determine the format of the output file. The default Blackbird behavior is to convert to XLIFF for future steps."), StaticDataSource(typeof(ProcessFileFormatHandler))]
        public string? OutputFileHandling { get; set; }

        [Display("File translation strategy"), StaticDataSource(typeof(FileTranslationStrategyHandler))]
        public string? FileTranslationStrategy { get; set; } = "blackbird";
    }
}
