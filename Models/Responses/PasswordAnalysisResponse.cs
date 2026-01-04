namespace PasswordAnalysisService.Models.Responses
{
    public class PasswordAnalysisResponse
    {
        public bool IsCompromised { get; set; }
        public int StrengthScore { get; set; }
        public string? Message { get; set; }
    }
}
