using PasswordAnalysisService.Models.Responses;

namespace PasswordAnalysisService.Services
{
    public interface IPasswordAnalysisService
    {
        PasswordAnalysisResponse Analyze(string password);
    }
}

