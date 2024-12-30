using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.Systran.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Systran.Models.Request
{
    public class CreateDictionaryParameters
    {
        [Display("Dictionary name")]
        public string Name { get; set; }

        [Display("Source pos")]
        [StaticDataSource(typeof(SourcePosDataHandler))]
        public string? SourcePos { get; set; }

        [Display("Dictionary type")]
        [StaticDataSource(typeof(DictionaryTypeDataHandler))]
        public string Type { get; set; }

        [Display("Dictionary description")]
        public string? Comment { get; set; }
        [Display("File")]
        public FileReference TbxFile { get; set; }
    }
}
