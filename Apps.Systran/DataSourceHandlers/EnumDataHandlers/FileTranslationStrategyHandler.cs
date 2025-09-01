using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Systran.DataSourceHandlers.EnumDataHandlers
{
    public class FileTranslationStrategyHandler : IStaticDataSourceItemHandler
    {
        public IEnumerable<DataSourceItem> GetData()
        {
            return new List<DataSourceItem>()
        {
            new DataSourceItem("blackbird", "Blackbird interoperable (default)"),
            new DataSourceItem("systran", "Systran native"),
        };
        }
    }

}
