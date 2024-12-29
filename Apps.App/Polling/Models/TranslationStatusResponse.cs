using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Systran.Polling.Models
{
    public class TranslationStatusResponse
    {
        public string RequestId { get; set; }
        public string Status { get; set; }
        public DateTime? FinishedAt { get; set; }
    }
}
