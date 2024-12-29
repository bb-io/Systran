using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Systran.Polling.Models
{
    public class TranslationResultResponse
    {
        public string RequestId { get; set; }
        public string Status { get; set; }
        public DateTime FinishedAt { get; set; }
        public FileReference File { get; set; }
    }
}
