namespace Apps.Systran.Models.Response
{
    public class ErrorResponse
    {
        public string? Message { get; set; } = string.Empty;
        public int? StatusCode { get; set; }
        public object? Info { get; set; }
    }
}
