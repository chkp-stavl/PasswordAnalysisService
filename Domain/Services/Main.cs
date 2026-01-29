using Core.Abstractions;
using Domain.Interfaces;
using Domain.Models;

namespace Core.Services
{
    public class Main : IMain
    {
        private readonly IStrengthChecker _strengthChecker;
        private readonly IBreachChecker _breachChecker;
        private readonly IRiskAssessmentService _riskService;

        public Main(IStrengthChecker strengthChecker, IBreachChecker breachChecker, IRiskAssessmentService riskService)
        {
            _strengthChecker = strengthChecker;
            _breachChecker = breachChecker;
            _riskService = riskService;
        }

        public async Task<RiskResult> Analyze(
            string password,
            CancellationToken ct)
        {
            var strengthResult = _strengthChecker.Check(password, ct);
            var breachResult = await _breachChecker.CheckAsync(password, ct);

            return _riskService.Assess(strengthResult, breachResult);
        }
    }
}
