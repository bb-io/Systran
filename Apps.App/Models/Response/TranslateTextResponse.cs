using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Systran.Models.Response
{
    public class TranslateTextResponse
    {
        [Display("Error")]
        public ErrorResponse? Error { get; set; }

        [Display("Request ID")]
        public string RequestId { get; set; } = string.Empty;

        [Display("Outputs")]
        public List<TranslationOutput> Outputs { get; set; } = new();
    }

    public class TranslationOutput
    {
        [Display("Error")]
        public string? Error { get; set; }

        [Display("Info")]
        public TranslationInfo? Info { get; set; }

        [Display("Translated text")]
        public string Output { get; set; } = string.Empty;

        [Display("Back translation")]
        public string? BackTranslation { get; set; }

        [Display("Source text")]
        public string? Source { get; set; }
    }

    public class TranslationInfo
    {
        [Display("Language identification")]
        public LanguageIdentification? Lid { get; set; }

        [Display("Selected routes")]
        public List<SelectedRoute>? SelectedRoutes { get; set; }

        [Display("Statistics")]
        public object? Stats { get; set; }
    }

    public class LanguageIdentification
    {
        [Display("Language")]
        public string Language { get; set; } = string.Empty;

        [Display("Confidence level")]
        public float Confidence { get; set; }
    }

    public class SelectedRoute
    {
        [Display("Routes")]
        public List<RouteDetail> Routes { get; set; } = new();

        [Display("Step name")]
        public string StepName { get; set; } = string.Empty;
    }
    public class RouteDetail
    {
        [Display("Profile ID")]
        public string ProfileId { get; set; } = string.Empty;

        [Display("Queue")]
        public string Queue { get; set; } = string.Empty;

        [Display("Service")]
        public string Service { get; set; } = string.Empty;

        [Display("Version")]
        public int Version { get; set; }

        [Display("Selectors")]
        public object? Selectors { get; set; }
    }
}
