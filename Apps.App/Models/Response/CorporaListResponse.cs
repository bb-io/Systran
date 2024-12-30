namespace Apps.Systran.Models.Response
{
    public class CorporaListResponse
    {
        public List<CorpusFile> Files { get; set; }
        public List<string> Directories { get; set; }
    }
    public class CorpusFile
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string SourceLang { get; set; }
        public List<string> TargetLangs { get; set; }
        public string Status { get; set; }
    }
}
