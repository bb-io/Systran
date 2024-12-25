using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.App.Dto
{
    public class SystranError
    {
        public bool HasError => !string.IsNullOrEmpty(Message);
        public string? Message { get; set; }
    }
}
