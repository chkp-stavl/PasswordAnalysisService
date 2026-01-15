using PasswordAnalysisService.Logic;
using PasswordAnalysisService.Mappers;
using PasswordAnalysisService.Models.Responses;

namespace PasswordAnalysisService.Services
{
    public class PasswordAnalysisOrchestrator : IPasswordAnalysisOrchestrator
    {
        private readonly IStrengthChecker strengthChecker;
        private readonly IBreachChecker breachChecker;
        private readonly IRiskAssessmentService riskAssessmentService;

        public PasswordAnalysisOrchestrator(IStrengthChecker strengthChecker, IBreachChecker breachChecker, IRiskAssessmentService riskAssessmentService)
        {
            this.strengthChecker = strengthChecker;
            this.breachChecker = breachChecker;
            this.riskAssessmentService = riskAssessmentService;
        }

        public async Task<PasswordAnalysisResponse> Analyze(
           string password,
           CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(password))
                return PasswordAnalysisResponse.Invalid("Password must not be empty");

            var strengthTask = strengthChecker.CheckAsync(password, ct);
            var breachTask = breachChecker.CheckAsync(password, ct);

            await Task.WhenAll(strengthTask, breachTask);

            var risk = riskAssessmentService.Assess(
                strengthTask.Result,
                breachTask.Result
            );

            return PasswordAnalysisResponseMapper.Map(
                strengthTask.Result,
                breachTask.Result,
                risk
            );
        }
    }
}
