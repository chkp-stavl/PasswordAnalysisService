using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using Moq;
using Core.Services;


namespace PasswordAnalysisService.Core.Tests
{
    public class MainTests
    {
        private readonly Mock<IStrengthChecker> _fakeStrengthChecker = new();
        private readonly Mock<IBreachChecker> _fakeBreachChecker = new();
        private readonly Mock<IRiskAssessmentService> _riskService = new();

        private readonly Main _sut;

        public MainTests()
        {
            _sut = new Main(
                _fakeStrengthChecker.Object,
                _fakeBreachChecker.Object,
                _riskService.Object);
        }

        [Fact]
        public async Task Analyze_WhenCalled_ReturnsRiskResultFromRiskService()
        {
            // Arrange
            var password = "StrongPassword123!";
            var ct = CancellationToken.None;

            var strength = new StrengthResult(
                Score: 5,
                Level: PasswordStrengthLevel.Strong,
                Issues: []);

            var breach = new BreachResult(
                IsBreached: false,
                Sources: []);

            var expectedRisk = new RiskResult(
                Score: 1,
                Level: RiskLevel.Low,
                Reasons: []);

            _fakeStrengthChecker
                .Setup(x => x.Check(password, ct))
                .Returns(strength);

            _fakeBreachChecker
                .Setup(x => x.CheckAsync(password, ct))
                .ReturnsAsync(breach);

            _riskService
                .Setup(x => x.Assess(strength, breach))
                .Returns(expectedRisk);

            var result = await _sut.Analyze(password, ct);

            Assert.Equal(expectedRisk, result);
        }

        [Fact]
        public async Task Analyze_CallsDependenciesWithCorrectArguments()
        {
            var password = "AnyPassword";
            var ct = CancellationToken.None;

            var strength = new StrengthResult(0, PasswordStrengthLevel.Weak, []);
            var breach = new BreachResult(false, []);
            var risk = new RiskResult(0, 0, []);

            _fakeStrengthChecker.Setup(x => x.Check(password, ct)).Returns(strength);
            _fakeBreachChecker.Setup(x => x.CheckAsync(password, ct)).ReturnsAsync(breach);
            _riskService.Setup(x => x.Assess(strength, breach)).Returns(risk);

            await _sut.Analyze(password, ct);

            _fakeStrengthChecker.Verify(x => x.Check(password, ct), Times.Once);
            _fakeBreachChecker.Verify(x => x.CheckAsync(password, ct), Times.Once);
            _riskService.Verify(x => x.Assess(strength, breach), Times.Once);
        }
    }

}
