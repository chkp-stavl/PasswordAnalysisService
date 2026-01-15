using PasswordAnalysisService.Logic;
using PasswordAnalysisService.Models.Responses;
using PasswordAnalysisService.Services;

namespace PasswordAnalysisService.Mappers
{
    public static class PasswordAnalysisResponseMapper
    {
        public static PasswordAnalysisResponse Map(
            StrengthResult strength,
            BreachResult breach,
            RiskResult risk)
        {
            return new PasswordAnalysisResponse
            {
                Strength = new PasswordStrengthDto
                {
                    Score = strength.Score,
                    Level = strength.Level.ToString(),
                    Issues = strength.Issues
                },
                Breach = new BreachDto
                {
                    IsCompromised = breach.IsBreached,
                    Sources = breach.Sources,
                    Warning = breach.AllSourcesUnavailable
                        ? "Breach checks are temporarily unavailable."
                        : null
                },
                Risk = new RiskDto
                {
                    Score = risk.Score,
                    Level = risk.Level.ToString(),
                    Reasons = risk.Reasons
                }
            };
        }
    }
}
