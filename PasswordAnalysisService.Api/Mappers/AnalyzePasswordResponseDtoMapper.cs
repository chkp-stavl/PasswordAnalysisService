using Domain.Models;
using PasswordAnalysisService.Api.Contracts;
using PasswordAnalysisService.Models.Responses;

namespace PasswordAnalysisService.Mappers
{
    public static class AnalyzePasswordResponseDtoMapper
    {
        public static AnalyzePasswordResponseDto From(RiskResult risk)
        {
            return new AnalyzePasswordResponseDto
            {
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
