using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using Moq;
using System.Collections.Immutable;

namespace PasswordAnalysisService.Tests
{
    public class AnalyzePasswordUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_InvokesAllDependencies()
        {
            var strength = new Mock<IStrengthChecker>();
            var breach = new Mock<IBreachChecker>();
            var risk = new Mock<IRiskAssessmentService>();

            var strengthResult = new StrengthResult(80, PasswordStrengthLevel.Strong, ImmutableArray<string>.Empty);
            var breachResult = new BreachResult(false, Array.Empty<BreachSourceResult>());
            var riskResult = new RiskResult(RiskLevel.Low, 0, Array.Empty<string>());

            strength.Setup(s => s.Check(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(strengthResult);

            breach.Setup(b => b.CheckAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(breachResult);

            risk.Setup(r => r.Assess(strengthResult, breachResult))
                .Returns(riskResult);

            var useCase = new AnalyzePasswordUseCase(
                strength.Object,
                breach.Object,
                risk.Object);

            var result = await useCase.ExecuteAsync(
                new AnalyzePasswordRequest { Password = "Test123!" },
                CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(RiskLevel.Low, result.Risk.Level);
        }
    }
}
