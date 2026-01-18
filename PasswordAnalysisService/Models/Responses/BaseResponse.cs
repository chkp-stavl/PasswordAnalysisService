namespace PasswordAnalysisService.Models.Responses
{
    public abstract record BaseResponse
    {
        public bool IsValid { get; init; } = true;
        public string? Error { get; init; }
    }

}
