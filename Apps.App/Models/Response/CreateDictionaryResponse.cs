using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.App.Dto;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Systran.Models.Response
{
    public class CreateDictionaryResponse
    {
        public SystranError? Error { get; set; }

        [Display("Added")]
        public AddedDictionary Added { get; set; }
    }

    public class AddedDictionary
    {
        [Display("Dictionary ID")]
        public string Id { get; set; }

        [Display("Dictionary name")]
        public string Name { get; set; }

        [Display("Source language code")]
        public string SourceLang { get; set; }

        public string SourcePos { get; set; }

        [Display("Dictionary type")]
        public string Type { get; set; }

        [Display("Dictionary description")]
        public string Comments { get; set; }
    }
}
