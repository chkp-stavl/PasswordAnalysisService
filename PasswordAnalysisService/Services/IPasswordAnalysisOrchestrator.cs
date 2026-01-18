using PasswordAnalysisService.Models.Responses;

namespace PasswordAnalysisService.Services
{
    public interface IPasswordAnalysisOrchestrator
    {
        Task<PasswordAnalysisResponse> Analyze(string password, CancellationToken ct = default);
    }
}

