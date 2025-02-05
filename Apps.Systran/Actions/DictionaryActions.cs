using System.Text;
using System.Xml.Linq;
using Apps.App.Api;
using Apps.App.Invocables;
using Apps.Systran.Models;
using Apps.Systran.Models.Request;
using Apps.Systran.Models.Response;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Glossaries.Utils.Converters;
using Blackbird.Applications.Sdk.Glossaries.Utils.Dtos;
using Blackbird.Applications.Sdk.Glossaries.Utils.Dtos.Enums;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;

namespace Apps.Systran.Actions
{
    [ActionList]
    public class DictionaryActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : SystranInvocable(invocationContext)
    {
        [Action("Create dictionary", Description = "Create a dictionary and populate it using a TBX file")]
        public async Task<CreateDictionaryResponse> CreateDictionary(
            [ActionParameter] CreateDictionaryParameters parameters,
            [ActionParameter] TranslateLanguagesOptions options)
        {
            var createDictionaryRequest = new SystranRequest("/resources/dictionary/add", Method.Post);
            createDictionaryRequest.AddJsonBody(new
            {
                dictionary = new
                {
                    name = parameters.Name,
                    sourceLang = options.Source,
                    sourcePos = parameters.SourcePos,
                    targetLangs = options.Target,
                    type = parameters.Type,
                    comments = parameters.Comment ?? "Created via API and populated using a TBX file"
                }
            });

            var createResponse = await Client.ExecuteWithErrorHandling<CreateDictionaryResponse>(createDictionaryRequest);

            var dictionaryId = createResponse.Added.Id;

            var systranFormattedContent = await ConvertTbxToSystranFormat(parameters.TbxFile, options.Source, options.Target);

            var importEntriesRequest = new SystranRequest("/resources/dictionary/entry/import", Method.Post);
            importEntriesRequest.AddQueryParameter("dictionaryId", dictionaryId);
            importEntriesRequest.AddQueryParameter("sourceLang", options.Source);

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(systranFormattedContent)))
            {
                stream.Position = 0;
                importEntriesRequest.AddFile("inputFile",
                    stream.ToArray(),
                    "ConvertedDictionary.tsv",
                    "text/plain");
            }

            var importResponse = await Client.ExecuteWithErrorHandling<ImportResponse>(importEntriesRequest);

            if (importResponse.Error != null)
                throw new PluginApplicationException($"Failed to import entries: {importResponse.Error.Message}");

            return createResponse;
        }

        private async Task<string> ConvertTbxToSystranFormat(FileReference tbxFile, string sourceLang, string targetLang)
        {
            await using var originalStream = await fileManagementClient.DownloadAsync(tbxFile);

            var memoryStream = new MemoryStream();
            await originalStream.CopyToAsync(memoryStream);

            memoryStream.Position = 0;

            XDocument xDoc;
            xDoc = XDocument.Load(memoryStream);

            var root = xDoc.Root;
            if (root == null)
                throw new PluginMisconfigurationException("We couldn't find the main section in your file. Please make sure the file is complete and try again.");

            if (!string.Equals(root.Name.LocalName, "tbx", StringComparison.OrdinalIgnoreCase))
                throw new PluginMisconfigurationException($"The file should start with a <tbx> section. Please double-check the file format");

            var typeAttr = root.Attribute("type")?.Value;
            if (!string.Equals(typeAttr, "TBX-Core", StringComparison.OrdinalIgnoreCase))
            {
                throw new PluginMisconfigurationException(
                    $"The file is missing or has an incorrect type='TBX-Core' setting in its main section. Please verify the file structure and try again.");
            }

            memoryStream.Position = 0;

            await using var tbxStream = await fileManagementClient.DownloadAsync(tbxFile);
            var blackbirdGlossary = await tbxStream.ConvertFromTbx();

            var entries = new StringBuilder();
            entries.AppendLine("#ENCODING=UTF-8");
            entries.AppendLine($"#SUMMARY={tbxFile.Name}");
            entries.AppendLine($"#DESCRIPTION={blackbirdGlossary.ConceptEntries}");
            entries.AppendLine("#MULTI");
            entries.AppendLine($"#{sourceLang.ToUpper()}\tUPOS\t{targetLang.ToUpper()}\tPRIORITY_{targetLang.ToUpper()}\tCOMMENTS_{targetLang.ToUpper()}");

            foreach (var entry in blackbirdGlossary.ConceptEntries)
            {
                var langSectionSource = entry.LanguageSections.FirstOrDefault(x => x.LanguageCode.ToLower() == sourceLang.ToLower());
                var langSectionTarget = entry.LanguageSections.FirstOrDefault(x => x.LanguageCode.ToLower() == targetLang.ToLower());

                if (langSectionSource != null && langSectionTarget != null)
                {
                    var sourceTerm = langSectionSource.Terms.FirstOrDefault()?.Term;
                    var targetTerm = langSectionTarget.Terms.FirstOrDefault()?.Term;

                    if (!string.IsNullOrWhiteSpace(sourceTerm) && !string.IsNullOrWhiteSpace(targetTerm))
                    {
                        entries.AppendLine($"{sourceTerm}\tnoun\t{targetTerm}\t9\t");
                    }
                }
            }

            string tsvContent = entries.ToString();

            return tsvContent;
        }

        [Action("Export dictionary", Description = "Export dictionary as TBX file")]
        public async Task<FileReferenceResponse> ExportDictionary([ActionParameter] ExportDictionaryRequest parameters)
        {
            var listEntriesRequest = new SystranRequest("/resources/dictionary/entry/list", Method.Post);
            listEntriesRequest.AddQueryParameter("dictionaryId", parameters.DictionaryId);

            var listEntriesResponse = await Client.ExecuteWithErrorHandling<ListEntriesResponse>(listEntriesRequest);

            if (listEntriesResponse.Entries == null || !listEntriesResponse.Entries.Any())
            {
                throw new PluginApplicationException($"No entries found for dictionary ID: {parameters.DictionaryId}");
            }

            var conceptEntries = listEntriesResponse.Entries.Select(entry =>
                new GlossaryConceptEntry(entry.SourceId, new List<GlossaryLanguageSection>
                {
                    new GlossaryLanguageSection(entry.SourceLang, new List<GlossaryTermSection>
                    {
                        new GlossaryTermSection(entry.Source)
                        {
                            PartOfSpeech = Enum.TryParse<PartOfSpeech>(entry.PartOfSpeech, true, out var sourcePos) ? sourcePos : null
                        }
                    }),
                    new GlossaryLanguageSection(entry.TargetLang, new List<GlossaryTermSection>
                    {
                        new GlossaryTermSection(entry.Target)
                        {
                            PartOfSpeech = Enum.TryParse<PartOfSpeech>(entry.PartOfSpeech, true, out var targetPos) ? targetPos : null
                        }
                    })
                })).ToList();

            var glossary = new Glossary(conceptEntries)
            {
                Title = $"{parameters.DictionaryId}",
                SourceDescription = $"Exported from dictionary ID: {parameters.DictionaryId}"
            };

            using var tbxStream = glossary.ConvertToTbx();

            var fileReference = await fileManagementClient.UploadAsync(
                tbxStream,
                "application/x-tbx+xml",
                $"{parameters.DictionaryId}.tbx");

            return new FileReferenceResponse
            {
                FileResponse = fileReference
            };
        }

        [Action("Update dictionary from TBX file", Description = "Update an existing dictionary by importing entries from a TBX file")]
        public async Task<UpdateDictionaryResponse> UpdateDictionary(
            [ActionParameter] UpdateDictionaryRequest parameters,
            [ActionParameter] TranslateLanguagesOptions options)
        {
            var systranFormattedContent = await ConvertTbxToSystranFormat(parameters.File, options.Source, options.Target);

            var importEntriesRequest = new SystranRequest("/resources/dictionary/entry/import", Method.Post);
            importEntriesRequest.AddQueryParameter("dictionaryId", parameters.DictionaryId);
            importEntriesRequest.AddQueryParameter("sourceLang", options.Source);

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(systranFormattedContent)))
            {
                stream.Position = 0;
                importEntriesRequest.AddFile(
                    "inputFile",
                    stream.ToArray(),
                    "ConvertedDictionary.tsv",
                    "text/plain"
                );
            }

            var importResponse = await Client.ExecuteWithErrorHandling<ImportResponse>(importEntriesRequest);

            if (importResponse.Error != null)
            {
                throw new PluginApplicationException($"Failed to import entries: {importResponse.Error.Message}");
            }

            return new UpdateDictionaryResponse
            {
                Success = true,
                Message = $"Successfully updated dictionary ID: {parameters.DictionaryId}"
            };
        }
    }
}
