using Application.Interfaces;
using Application.Requests;
using Application.Responses;
using Domain.Interfaces;

namespace Application.UseCases
{
    public class AnalyzePasswordUseCase : IAnalyzePasswordUseCase
    {
        private readonly IStrengthChecker _strengthChecker;
        private readonly IBreachChecker _breachChecker;
        private readonly IRiskAssessmentService _riskAssessmentService;

        public AnalyzePasswordUseCase(
            IStrengthChecker strengthChecker,
            IBreachChecker breachChecker,
            IRiskAssessmentService riskAssessmentService)
        {
            _strengthChecker = strengthChecker;
            _breachChecker = breachChecker;
            _riskAssessmentService = riskAssessmentService;
        }

        public async Task<AnalyzePasswordResult> ExecuteAsync(
            AnalyzePasswordRequest request,
            CancellationToken ct)
        {
            var strengthTask = _strengthChecker.CheckAsync(request.Password, ct);
            var breachTask = _breachChecker.CheckAsync(request.Password, ct);

            await Task.WhenAll(strengthTask, breachTask);

            var strength = await strengthTask;
            var breach = await breachTask;

            var risk = _riskAssessmentService.Assess(strength, breach);

            return new AnalyzePasswordResult(strength, breach, risk);
        }
    }
}
