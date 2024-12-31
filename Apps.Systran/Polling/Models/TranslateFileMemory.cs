namespace Apps.Systran.Polling.Models
{
    public class TranslateFileMemory
    {
        public DateTime? LastPollingTime { get; set; }

        public bool Triggered { get; set; }
    }
}
