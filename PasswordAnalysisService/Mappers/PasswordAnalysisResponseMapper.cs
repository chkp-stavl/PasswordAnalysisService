using Application.Responses;
using PasswordAnalysisService.Models.Responses;

namespace PasswordAnalysisService.Mappers
{
    public static class PasswordAnalysisResponseMapper
    {
        public static AnalyzePasswordResponseDto Map(
           AnalyzePasswordResult result)
        {
            return new AnalyzePasswordResponseDto
            {
                Strength = new PasswordStrengthDto
                {
                    Score = result.Strength.Score,
                    Level = result.Strength.Level.ToString(),
                    Issues = result.Strength.Issues
                },
                Breach = new BreachDto
                {
                    IsCompromised = result.Breach.IsBreached,
                    Sources = result.Breach.Sources.Select(s => new BreachSourceDto
                    {
                        Source = s.Source,
                        BreachCount = s.BreachCount,
                        Prevalence = s.Prevalence.ToString()
                    }).ToList(),
                    Warning = result.Breach.AllSourcesUnavailable
                        ? "Breach checks are temporarily unavailable."
                        : null
                },
                Risk = new RiskDto
                {
                    Score = result.Risk.Score,
                    Level = result.Risk.Level.ToString(),
                    Reasons = result.Risk.Reasons
                }
            };
        }
    }
}
