using PasswordAnalysisService.Logic;

namespace PasswordAnalysisService.Models.Responses
{
    public record PasswordAnalysisResponse
    {
        public PasswordStrengthDto Strength { get; init; } = default!;
        public BreachDto Breach { get; init; } = default!;
        public RiskDto Risk { get; init; } = default!;
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
        public IReadOnlyList<BreachSourceResult> Sources { get; init; } = [];
    }

    public record RiskDto
    {
        public int Score { get; init; }                  
        public string Level { get; init; } = default!;   
        public IReadOnlyList<string> Reasons { get; init; } = [];
    }



}
