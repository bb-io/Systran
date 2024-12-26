using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Systran.DataSourceHandlers.EnumDataHandlers
{
    public class CorpusFormatDataHandler : IStaticDataSourceItemHandler
    {
        protected Dictionary<string, string> EnumValues => new()
        {
            { "application/x-tmx+xml", "TMX XML" },
            { "text/bitext", "Bitext" }
        };

        public IEnumerable<DataSourceItem> GetData()
        {
            return EnumValues.Select(item => new DataSourceItem
            {
                Value = item.Key,
                DisplayName = item.Value
            });
        }
    }
}
