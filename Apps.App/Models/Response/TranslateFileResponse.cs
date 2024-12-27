using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Systran.Models.Response
{
    public class TranslateFileResponse
    {
        [Display("File")]
        public FileReference File { get; set; }
    }


}
