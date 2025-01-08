﻿using Apps.Systran.DataSourceHandlers;
using Apps.Systran.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Systran.Models
{
    public class TranslateLanguagesOptions
    {
        [Display("Source language code")]
        [StaticDataSource(typeof(SupportedLanguagesDataHandler))]
        public string Source { get; set; }

        [Display("Target language code")]
        [StaticDataSource(typeof(SupportedLanguagesDataHandler))]
        public string Target { get; set; }
    }
}
