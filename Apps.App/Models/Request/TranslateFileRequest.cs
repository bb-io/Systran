using Apps.Systran.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Systran.Models.Request
{
    public class TranslateFileRequest
    {
        [Display("Input file")]
        public FileReference Input { get; set; }

        [Display("Profile ID")]
        [DataSource(typeof(ProfilesDataHandler))]
        public string? Profile { get; set; }
    }
}
