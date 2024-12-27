using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.App.Api;
using Apps.App.Invocables;
using Apps.Systran.Models.Request;
using Apps.Systran.Models.Response;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Glossaries.Utils.Converters;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;

namespace Apps.Systran.Actions
{
    [ActionList]
    public class DictionaryActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : SystranInvocable(invocationContext)
    {
        [Action("Create dictionary", Description = "Create a dictionary and populate it using a TBX file")]
        public async Task CreateDictionary(
            [ActionParameter] CreateDictionaryParameters parameters)
        {

            var createDictionaryRequest = new SystranRequest("/resources/dictionary/add", Method.Post);
            createDictionaryRequest.AddJsonBody(new
            {
                dictionary = new
                {
                    name = parameters.Name,
                    sourceLang = parameters.SourceLang,
                    sourcePos = parameters.SourcePos,
                    targetLangs = parameters.TargetLangs,
                    type = parameters.Type,
                    comments = parameters.Comment ?? "Created via API and populated using a TBX file"
                }
            });

            var createResponse = await Client.ExecuteWithErrorHandling<CreateDictionaryResponse>(createDictionaryRequest);

            if (createResponse.Error != null)
                throw new Exception($"Failed to create dictionary: {createResponse.Error.Message}");

            var dictionaryId = createResponse.Added.Id;
            Console.WriteLine($"Dictionary created successfully with ID: {dictionaryId}");

            var systranFormattedContent = await ConvertTbxToSystranFormat(parameters.TbxFile, parameters.SourceLang, parameters.TargetLangs);

            var importEntriesRequest = new SystranRequest("/resources/dictionary/entry/import", Method.Post);
            importEntriesRequest.AddQueryParameter("dictionaryId", dictionaryId);
            importEntriesRequest.AddQueryParameter("sourceLang", parameters.SourceLang);

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
                throw new Exception($"Failed to import entries: {importResponse.Error.Message}");

        }


        private async Task<string> ConvertTbxToSystranFormat(FileReference tbxFile, string sourceLang, string targetLang)
        {
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

            Console.WriteLine("Generated TSV Content:");
            Console.WriteLine(tsvContent);

            return tsvContent;
        }
    }
}

