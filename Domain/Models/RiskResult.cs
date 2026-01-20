using Domain.Enums;

namespace Domain.Models
{
    public record RiskResult(
        RiskLevel Level,
        int Score,
        IReadOnlyList<string> Reasons
    );
}
