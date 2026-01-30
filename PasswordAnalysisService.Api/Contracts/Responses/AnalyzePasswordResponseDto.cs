using Domain.Models;
using PasswordAnalysisService.Api.Contracts;

namespace PasswordAnalysisService.Models.Responses
{
    public record AnalyzePasswordResponseDto
    {
        public RiskDto Risk { get; init; } = default!;
    }

}
