using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Systran.Models.Response
{
    public class ErrorResponse
    {
        public string? Message { get; set; } = string.Empty;
        public int? StatusCode { get; set; }
        public object? Info { get; set; }
    }
}
