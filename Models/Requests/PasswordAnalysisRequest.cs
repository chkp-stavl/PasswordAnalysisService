namespace PasswordAnalysisService.Models.Requests
{
    public class PasswordAnalysisRequest
    {
        public required string Password { get; set; } = string.Empty;
    }
}
