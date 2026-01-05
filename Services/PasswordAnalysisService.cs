using PasswordAnalysisService.Logic;
using PasswordAnalysisService.Models.Responses;

namespace PasswordAnalysisService.Services
{
    public class PasswordAnalysisService : IPasswordAnalysisService
    {
        private readonly IStrengthChecker strengthChecker;
        private readonly IBreachChecker breachChecker;
        private readonly IRiskAssessmentService riskAssessmentService;

        public PasswordAnalysisService(IStrengthChecker strengthChecker, IBreachChecker breachChecker, IRiskAssessmentService riskAssessmentService)
        {
            this.strengthChecker = strengthChecker;
            this.breachChecker = breachChecker;
            this.riskAssessmentService = riskAssessmentService;
        }

        public async Task<PasswordAnalysisResponse> Analyze(string password, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidPasswordException("Password must not be empty");
            }
            var strengthTask = strengthChecker.CheckAsync(password, ct);
            var breachTask = breachChecker.CheckAsync(password, ct);

            await Task.WhenAll(strengthTask, breachTask);
            var strength = await strengthTask;
            var breach = await breachTask;
            var risk = riskAssessmentService.Assess(strength, breach);
            var response = new PasswordAnalysisResponse
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
                    Sources = breach.Sources
                },
                Risk = new RiskDto
                {
                    Score = risk.Score,
                    Level = risk.Level.ToString(),
                    Reasons = risk.Reasons
                }
            };

            return response;
        }
    }
}
