using API.ControllerLogics;
using Application.Interfaces;
using Application.Requests;
using PasswordAnalysisService.Mappers;
using PasswordAnalysisService.Models.Requests;
using PasswordAnalysisService.Models.Responses;

namespace PasswordAnalysisService.Services
{
    public class PasswordAnalysisControllerLogic : IPasswordAnalysisControllerLogic
    {
        private readonly IAnalyzePasswordUseCase _analyzePasswordUseCase;

        public PasswordAnalysisControllerLogic(
            IAnalyzePasswordUseCase useCase)
        {
            _analyzePasswordUseCase = useCase;
        }

        public async Task<AnalyzePasswordResponseDto> Analyze(
        AnalyzePasswordRequestDto request,
        CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return AnalyzePasswordResponseDto
                    .Invalid("Password must not be empty");
            }

            try
            {
                var result = await _analyzePasswordUseCase.ExecuteAsync(
                    new AnalyzePasswordRequest() { Password = request.Password},
                    ct);

                return PasswordAnalysisResponseMapper.Map(result);
            }
            catch (Exception)
            {
                return AnalyzePasswordResponseDto
                    .Invalid("Unexpected error occurred");
            }
        }
    }
}
