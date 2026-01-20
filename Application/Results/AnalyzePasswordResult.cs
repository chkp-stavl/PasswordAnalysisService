using Domain.Models;

namespace Application.Responses
{
    public record AnalyzePasswordResult(
        StrengthResult Strength,
        BreachResult Breach,
        RiskResult Risk
    );
}
