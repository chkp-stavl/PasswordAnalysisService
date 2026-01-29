using Core.Services;
using Domain.Enums;
using Domain.Models;
using PasswordAnalysisService.Core.Risk;
using System.Collections.Immutable;

namespace PasswordAnalysisService.Core.Tests
{
    public class RiskAssessmentServiceTests
    {
        private readonly RiskAssessmentService _service =
            new(
                new BreachRiskEvaluator(),
                new StrengthRiskEvaluator(),
                new RiskLevelCalculator()
                );

        [Fact]
        public void Assess_WhenNoRiskFactors_ReturnsLowRiskWithDefaultReason()
        {
            var strength = new StrengthResult(
                Score: 10,
                Level: PasswordStrengthLevel.Strong,
                Issues: []);

            var breach = new BreachResult(
                IsBreached: false,
                Sources: []);

            var result = _service.Assess(strength, breach);

            Assert.Equal(RiskLevel.Low, result.Level);
            Assert.Contains(
                "No significant risk factors detected",
                result.Reasons);
        }

        [Fact]
        public void Assess_WhenMultipleRiskFactorsExist_ReturnsHigherRisk()
        {
            var strength = new StrengthResult(
                Score: 2,
                Level: PasswordStrengthLevel.Weak,
                Issues: []);

            var breach = new BreachResult(
                IsBreached: true,
                Sources: []);

            var result = _service.Assess(strength, breach);

            Assert.NotEqual(RiskLevel.Low, result.Level);
            Assert.NotEmpty(result.Reasons);
        }
    }
}
