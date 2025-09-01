﻿using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.SDK.Blueprints.Interfaces.Translate;

namespace Apps.Systran.Models.Response
{
    public class FileReferenceResponse : ITranslateFileOutput
    {
        [Display("File")]
        public FileReference File { get; set; }
    }
}
