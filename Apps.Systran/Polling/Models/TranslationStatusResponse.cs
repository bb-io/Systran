

using Blackbird.Applications.Sdk.Common;

namespace Apps.Systran.Polling.Models
{
    public class TranslationStatusResponse
    {
        [Display("Request ID")]
        public string RequestId { get; set; }

        [Display("Request status")]
        public string Status { get; set; }

        public long? FinishedAtEpoch { get; set; }
        public DateTime? FinishedAt
        {
            get
            {
                if (!FinishedAtEpoch.HasValue)
                    return null;

                return DateTimeOffset
                    .FromUnixTimeMilliseconds(FinishedAtEpoch.Value)
                    .UtcDateTime;
            }
        }
    }
}
