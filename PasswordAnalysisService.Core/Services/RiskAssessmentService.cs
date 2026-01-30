using Domain.Constants;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using PasswordAnalysisService.Core.Risk;

namespace Core.Services
{
    public class RiskAssessmentService : IRiskAssessmentService
    {
        private readonly BreachRiskEvaluator _breachEvaluator;
        private readonly StrengthRiskEvaluator _strengthEvaluator;
        private readonly RiskLevelCalculator _levelCalculator;

        public RiskAssessmentService(
            BreachRiskEvaluator breachEvaluator,
            StrengthRiskEvaluator strengthEvaluator,
            RiskLevelCalculator levelCalculator)
        {
            _breachEvaluator = breachEvaluator;
            _strengthEvaluator = strengthEvaluator;
            _levelCalculator = levelCalculator;
        }

        public RiskResult Assess(StrengthResult strength, BreachResult breach)
        {
            var reasons = new List<string>();
            int score = 0;

            score += _breachEvaluator.Evaluate(breach, reasons);
            score += _strengthEvaluator.Evaluate(strength, reasons);

            score = Math.Clamp(score, 0, RiskConstants.MAX_SCORE);
            var level = _levelCalculator.Calculate(score);

            if (reasons.Count == 0)
            {
                reasons.Add("No significant risk factors detected");
            }

            return new RiskResult(level, score, reasons);
        }
    }

}
