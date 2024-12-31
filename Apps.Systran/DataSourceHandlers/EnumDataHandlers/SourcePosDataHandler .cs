using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Systran.DataSourceHandlers.EnumDataHandlers
{
    public class SourcePosDataHandler : IStaticDataSourceItemHandler
    {
        protected Dictionary<string, string> EnumValues => new()
        {
            {"acr", "Acronym"},
            {"adj", "Adjective"},
            {"adv", "Adverb"},
            {"conj", "Conjunction"},
            {"expr", "Expression"},
            {"noun", "Noun"},
            {"prep", "Preposition"},
            {"proper noun", "Proper Noun"},
            {"rule", "Rule"},
            {"verb", "Verb"}
        };

        public IEnumerable<DataSourceItem> GetData()
        {
            return EnumValues.Select(item => new DataSourceItem
            {
                Value = item.Key,
                DisplayName = item.Value
            });
        }
    }
}
