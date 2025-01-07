using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Systran.Models.Response
{
    public class FileReferenceResponse
    {
        [Display("File")]
        public FileReference FileResponse { get; set; }
    }
}
