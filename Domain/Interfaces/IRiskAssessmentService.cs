using Domain.Models;

namespace Domain.Interfaces
{
    public interface IRiskAssessmentService
    {
        RiskResult Assess(
        StrengthResult strength,
        BreachResult breach
        );
    }
}
