namespace PasswordAnalysisService.Api.Contracts
{

    public record RiskDto
    {
        public int Score { get; init; }
        public string Level { get; init; } = default!;
        public IReadOnlyList<string> Reasons { get; init; } = [];
    }
}
