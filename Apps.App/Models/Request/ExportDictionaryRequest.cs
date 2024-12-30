using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.Systran.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Systran.Models.Request
{
    public class ExportDictionaryRequest
    {
        [Display("Dictionary ID")]
        [DataSource(typeof(DictionaryDataHandler))]
        public string DictionaryId { get; set; }
    }
}
