namespace Apps.Systran.Polling.Models
{
    public class TranslationStatusResponse
    {
        public string RequestId { get; set; }
        public string Status { get; set; }
        public DateTime? FinishedAt { get; set; }
    }
}
