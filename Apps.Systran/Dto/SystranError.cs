namespace Apps.App.Dto
{
    public class SystranError
    {
        public bool HasError => !string.IsNullOrEmpty(Message);
        public string? Message { get; set; }
    }
}
