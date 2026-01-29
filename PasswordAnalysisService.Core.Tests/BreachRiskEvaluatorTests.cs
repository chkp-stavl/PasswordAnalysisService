using Domain.Constants;
using Domain.Enums;
using Domain.Models;
using PasswordAnalysisService.Core.Risk;

namespace PasswordAnalysisService.Core.Tests
{
    public class BreachRiskEvaluatorTests
    {
        private readonly BreachRiskEvaluator _sut = new();

        [Fact]
        public void Evaluate_WhenPasswordBreached_AddsBreachScoreAndReason()
        {
            var reasons = new List<string>();
            var breach = new BreachResult(
                IsBreached: true,
                Sources:
                [
                new BreachSourceResult(
                    "HIBP",
                    true,
                    10,
                    BreachPrevalence.Low)
                ]);

            var score = _sut.Evaluate(breach, reasons);

            Assert.Equal(RiskConstants.BREACH_FOUND_SCORE, score);
            Assert.Contains(
                "Password was found in known data breaches",
                reasons);
        }

        [Fact]
        public void Evaluate_WhenHighPrevalenceSourceExists_AddsHighPrevalenceBonus()
        {
            var reasons = new List<string>();

            var breach = new BreachResult(
                IsBreached: false,
                Sources:
                [
                new BreachSourceResult(
                    "HIBP",
                    true,
                    1000,
                    BreachPrevalence.High)
                ]);

            var score = _sut.Evaluate(breach, reasons);

            Assert.Equal(RiskConstants.HIGH_PREVALENCE_BONUS, score);
        }
    }

}
