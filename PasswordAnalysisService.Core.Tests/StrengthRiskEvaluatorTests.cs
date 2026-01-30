using Domain.Constants;
using Domain.Enums;
using Domain.Models;
using PasswordAnalysisService.Core.Risk;

namespace PasswordAnalysisService.Core.Tests
{
    public class StrengthRiskEvaluatorTests
    {
        private readonly StrengthRiskEvaluator _sut = new();

        [Fact]
        public void Evaluate_WhenStrengthIsWeak_AddsPenaltyAndReason()
        {
            var reasons = new List<string>();
            var strength = new StrengthResult(
                Score: 1,
                Level: PasswordStrengthLevel.Weak,
                Issues: []);

            var penalty = _sut.Evaluate(strength, reasons);

            Assert.Equal(RiskConstants.WEAK_PENALTY, penalty);
            Assert.Contains("Password is weak", reasons);
        }

        [Fact]
        public void Evaluate_WhenStrengthIsStrong_AddsNoPenalty()
        {
            var reasons = new List<string>();

            var strength = new StrengthResult(
                Score: 10,
                Level: PasswordStrengthLevel.Strong,
                Issues: []);

            var penalty = _sut.Evaluate(strength, reasons);

            Assert.Equal(0, penalty);
            Assert.Empty(reasons);
        }
    }

}
