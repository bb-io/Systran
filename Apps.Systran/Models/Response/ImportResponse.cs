using Apps.App.Dto;

namespace Apps.Systran.Models.Response
{
    public class ImportResponse
    {
        public SystranError? Error { get; set; }
        public int Inserted { get; set; }
        public int Duplicates { get; set; }
        public int Total { get; set; }
    }
}
