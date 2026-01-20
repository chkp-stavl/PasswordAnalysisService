namespace PasswordAnalysisService.Models.Responses
{
    public record AnalyzePasswordResponseDto : BaseResponse
    {
        public PasswordStrengthDto Strength { get; init; } = default!;
        public BreachDto Breach { get; init; } = default!;
        public RiskDto Risk { get; init; } = default!;

        public static AnalyzePasswordResponseDto Invalid(string error) =>
        new()
        {
            IsValid = false,
            Error = error
        };
    }

    public record PasswordStrengthDto
    {
        public int Score { get; init; }                 
        public string Level { get; init; } = default!;  
        public IReadOnlyList<string> Issues { get; init; } = [];
    }

    public record BreachDto
    {
        public bool IsCompromised { get; init; }
        public int? BreachCount { get; init; }
        public IReadOnlyList<BreachSourceDto> Sources { get; init; } = [];
        public string? Warning { get; init; } = default!;
    }

    public record BreachSourceDto
    {
        public string Source { get; init; } = default!;
        public int? BreachCount { get; init; }
        public string Prevalence { get; init; } = default!;
    }

    public record RiskDto
    {
        public int Score { get; init; }                  
        public string Level { get; init; } = default!;   
        public IReadOnlyList<string> Reasons { get; init; } = [];
    }



}
