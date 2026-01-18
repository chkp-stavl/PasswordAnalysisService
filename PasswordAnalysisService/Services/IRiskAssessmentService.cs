using PasswordAnalysisService.Logic;
using static PasswordAnalysisService.Consts;

namespace PasswordAnalysisService.Services
{
    public interface IRiskAssessmentService
    {
        RiskResult Assess(
        StrengthResult strength,
        BreachResult breach
        );
    }
    public record RiskResult(
        RiskLevel Level,
        int Score,
        IReadOnlyList<string> Reasons
    );
}
