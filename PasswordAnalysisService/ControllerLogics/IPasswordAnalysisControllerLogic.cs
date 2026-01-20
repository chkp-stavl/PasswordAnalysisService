using PasswordAnalysisService.Models.Requests;
using PasswordAnalysisService.Models.Responses;

namespace API.ControllerLogics
{
    public interface IPasswordAnalysisControllerLogic
    {
        Task<AnalyzePasswordResponseDto> Analyze(AnalyzePasswordRequestDto request, CancellationToken ct = default);
    }
}
