using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Systran.Polling.Models
{
    public class TranslateFileMemory
    {
        public DateTime? LastPollingTime { get; set; }

        public bool Triggered { get; set; }
    }
}
