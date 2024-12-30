using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Vml.Office;

namespace Apps.Systran.Models.Response
{
    public class ListEntriesResponse
    {
        public IEnumerable<EntryInfo> Entries { get; set; }
        public int TotalNoLimit { get; set; }
    }
    public class EntryInfo
    {
        public string Source { get; set; }
        public string Target { get; set; }
        public string PartOfSpeech { get; set; }
        public string SourceLang { get; set; }
        public string TargetLang { get; set; }
        public string SourceId { get; set; }
        public string TargetId { get; set; }
    }

}
