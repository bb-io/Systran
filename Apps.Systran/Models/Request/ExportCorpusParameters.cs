using Apps.Systran.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Systran.Models.Request
{
    public class ExportCorpusParameters
    {
        [Display("Corpus ID")]
        [DataSource(typeof(CorporaDataHandler))]
        public string CorpusId { get; set; }
    }
}
