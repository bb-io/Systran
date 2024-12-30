namespace Apps.Systran.Models.Response
{
    public class CorporaListResponse
    {
        public IEnumerable<CorpusFile> Files { get; set; }
        public IEnumerable<string> Directories { get; set; }
    }
    public class CorpusFile
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string SourceLang { get; set; }
        public IEnumerable<string> TargetLangs { get; set; }
        public string Status { get; set; }
    }
}
