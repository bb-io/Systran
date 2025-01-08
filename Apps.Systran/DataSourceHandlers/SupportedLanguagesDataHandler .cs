using Apps.App.Api;
using Apps.App.Invocables;
using Apps.Systran.Dto;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Systran.DataSourceHandlers
{
    public class SupportedLanguagesDataHandler : SystranInvocable, IAsyncDataSourceItemHandler
    {
        protected Dictionary<string, string> LanguageCodes => new()
        {
            {"aa", "Afar"},{"ab", "Abkhazian"},{"af", "Afrikaans"},
            {"ak", "Akan"},{"sq", "Albanian"},{"am", "Amharic"},
            {"ar", "Arabic"},{"an", "Aragonese"},{"hy", "Armenian"},
            {"as", "Assamese"},{"av", "Avaric"},{"ae", "Avestan"},
            {"ay", "Aymara"},{"az", "Azerbaijani"},{"ba", "Bashkir"},
            {"bm", "Bambara"},{"eu", "Basque"},{"be", "Belarusian"},
            {"bn", "Bengali"},{"bh", "Bihari"},{"bi", "Bislama"},
            {"bs", "Bosnian"},{"br", "Breton"},{"bg", "Bulgarian"},
            {"my", "Burmese"},{"ca", "Catalan"},{"ch", "Chamorro"},
            {"ce", "Chechen"},{"zh", "Chinese"},{"cu", "Church Slavic"},
            {"cv", "Chuvash"},{"kw", "Cornish"},{"co", "Corsican"},
            {"cr", "Cree"},{"hr", "Croatian"},{"cs", "Czech"},
            {"da", "Danish"},{"dv", "Divehi"},{"nl", "Dutch"},
            {"en", "English"},{"eo", "Esperanto"},{"et", "Estonian"},
            {"ee", "Ewe"},{"fo", "Faroese"},{"fj", "Fijian"},
            {"fi", "Finnish"},{"fr", "French"},{"fy", "Western Frisian"},
            {"ff", "Fulah"},{"ka", "Georgian"},{"de", "German"},
            {"el", "Greek"},{"gn", "Guarani"},{"gu", "Gujarati"},
            {"ht", "Haitian"},{"ha", "Hausa"},{"he", "Hebrew"},
            {"hz", "Herero"},{"hi", "Hindi"},{"ho", "Hiri Motu"},
            {"hu", "Hungarian"},{"ia", "Interlingua"},{"id", "Indonesian"},
            {"ie", "Interlingue"},{"ga", "Irish"},{"ig", "Igbo"},
            {"ik", "Inupiaq"},{"io", "Ido"},{"is", "Icelandic"},
            {"it", "Italian"},{"iu", "Inuktitut"},{"ja", "Japanese"},
            {"jv", "Javanese"},{"kl", "Kalaallisut"},{"kn", "Kannada"},
            {"kr", "Kanuri"},{"ks", "Kashmiri"},{"kk", "Kazakh"},
            {"km", "Central Khmer"},{"ki", "Kikuyu"},{"rw", "Kinyarwanda"},
            {"ky", "Kirghiz"},{"kv", "Komi"},{"kg", "Kongo"},
            {"ko", "Korean"},{"kj", "Kuanyama"},{"ku", "Kurdish"},
            {"lo", "Lao"},{"la", "Latin"},{"lv", "Latvian"},
            {"li", "Limburgan"},{"ln", "Lingala"},{"lt", "Lithuanian"},
            {"lu", "Luba-Katanga"},{"lb", "Luxembourgish"},{"mk", "Macedonian"},
            {"mg", "Malagasy"},{"ms", "Malay"},{"ml", "Malayalam"},
            {"mt", "Maltese"},{"mi", "Maori"},{"mr", "Marathi"},
            {"mh", "Marshallese"},{"mn", "Mongolian"},{"na", "Nauru"},
            {"nv", "Navajo"},{"nd", "North Ndebele"},{"nr", "South Ndebele"},
            {"ng", "Ndonga"},{"ne", "Nepali"},{"no", "Norwegian"},
            {"nb", "Norwegian Bokmal"},{"nn", "Norwegian Nynorsk"},
            {"ii", "Sichuan Yi"},{"oc", "Occitan"},{"oj", "Ojibwa"},
            {"or", "Oriya"},{"om", "Oromo"},{"os", "Ossetian"},
            {"pi", "Pali"},{"pa", "Panjabi"},{"ps", "Pashto"},
            {"fa", "Persian"},{"pl", "Polish"},{"pt", "Portuguese"},
            {"qu", "Quechua"},{"ro", "Romanian"},{"rm", "Romansh"},
            {"rn", "Rundi"},{"ru", "Russian"},{"sm", "Samoan"},
            {"sg", "Sango"},{"sa", "Sanskrit"},{"sc", "Sardinian"},
            {"sr", "Serbian"},{"sn", "Shona"},{"sd", "Sindhi"},
            {"si", "Sinhala"},{"sk", "Slovak"},{"sl", "Slovenian"},
            {"so", "Somali"},{"st", "Southern Sotho"},{"es", "Spanish"},
            {"su", "Sundanese"},{"sw", "Swahili"},{"ss", "Swati"},
            {"sv", "Swedish"},{"ta", "Tamil"},{"te", "Telugu"},
            {"tg", "Tajik"},{"th", "Thai"},{"ti", "Tigrinya"},
            {"to", "Tonga"},{"tr", "Turkish"},{"tk", "Turkmen"},
            {"tw", "Twi"},{"uk", "Ukrainian"},{"ur", "Urdu"},
            {"uz", "Uzbek"},{"vi", "Vietnamese"},{"vo", "Volapük"},
            {"wa", "Walloon"},{"cy", "Welsh"},{"wo", "Wolof"},
            {"xh", "Xhosa"},{"yi", "Yiddish"},{"yo", "Yoruba"},
            {"za", "Zhuang"},{"zu", "Zulu"}
        };

        public SupportedLanguagesDataHandler(InvocationContext invocationContext) : base(invocationContext)
        {
        }

        public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
        {
            var request = new SystranRequest("/translation/supportedLanguages", RestSharp.Method.Get);
            var response = await Client.ExecuteWithErrorHandling<SupportedLanguagesResponse>(request);

            var pairs = response.LanguagePairs;

            var allLanguages = new HashSet<string>();
            foreach (var pair in pairs)
            {
                allLanguages.Add(pair.Source);
                allLanguages.Add(pair.Target);
            }

            var filtered = allLanguages
                .Where(code =>
                    string.IsNullOrWhiteSpace(context.SearchString)
                    || code.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase)
                    || (LanguageCodes.ContainsKey(code)
                        && LanguageCodes[code]
                            .Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
                )
                .Take(50)
                .Select(code =>
                {
                    var displayName = LanguageCodes.TryGetValue(code, out var languageName)
                        ? $"{languageName}"
                        : code;

                    return new DataSourceItem(value: code, displayName: displayName);
                });

            return filtered;
        }
    }
}
