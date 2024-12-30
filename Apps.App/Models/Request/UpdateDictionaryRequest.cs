using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Systran.Models.Request
{
    public class UpdateDictionaryRequest
    {
        [Display("Dictionary ID")]
        public string DictionaryId { get; set; }

        [Display("TBX file")]
        public FileReference File { get; set; }
    }
}
