namespace PasswordAnalysisService.Models.Requests
{
    public class AnalyzePasswordRequestDto
    {
        public required string Password { get; set; } = string.Empty;
    }
}
