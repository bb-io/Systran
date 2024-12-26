using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.App.Dto;

namespace Apps.Systran.Models.Response
{
    public class CreateDictionaryResponse
    {
        public SystranError? Error { get; set; }
        public AddedDictionary Added { get; set; }
    }

    public class AddedDictionary
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string SourceLang { get; set; }
        public string SourcePos { get; set; }
        public string Type { get; set; }
        public string Comments { get; set; }
    }
}
