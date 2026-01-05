using PasswordAnalysisService.Models.Responses;

namespace PasswordAnalysisService.Services
{
    public interface IPasswordAnalysisService
    {
        Task<PasswordAnalysisResponse> Analyze(string password, CancellationToken ct = default);
    }
}

