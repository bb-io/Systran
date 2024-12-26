using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Systran.Models.Response
{
    public class TranslateFileResponse
    {
        public ErrorResponse? Error { get; set; }
        public string RequestId { get; set; } = string.Empty;

        public FileReference File {  get; set; }
    }


}
