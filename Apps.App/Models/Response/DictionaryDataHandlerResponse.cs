using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Systran.Models.Response
{
    public class DictionaryDataHandlerResponse
    {
        public ErrorResponse? Error { get; set; }
        public int TotalNoLimit { get; set; }
        public List<DictionaryItem> Dictionaries { get; set; } = new();
    }
    public class DictionaryItem
    {
        public string SourceLang { get; set; } = string.Empty;
        public string TargetLangs { get; set; } = string.Empty;
        public int NbEntries { get; set; }
        public string Comments { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
    }
}
