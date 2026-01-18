namespace PasswordAnalysisService.Tests;

using Moq;
using PasswordAnalysisService.Logic;
using PasswordAnalysisService.Services;
using PasswordAnalysisService.Models.Responses;
using System.Collections.Immutable;
using static PasswordAnalysisService.Consts;

public class PasswordAnalysisOrchestratorTests
{
    private readonly Mock<IStrengthChecker> strengthCheckerMock;
    private readonly Mock<IBreachChecker> breachCheckerMock;
    private readonly Mock<IRiskAssessmentService> riskAssessmentServiceMock;
    private readonly PasswordAnalysisOrchestrator orchestrator;

    public PasswordAnalysisOrchestratorTests()
    {
        strengthCheckerMock = new Mock<IStrengthChecker>();
        breachCheckerMock = new Mock<IBreachChecker>();
        riskAssessmentServiceMock = new Mock<IRiskAssessmentService>();

        orchestrator = new PasswordAnalysisOrchestrator(
            strengthCheckerMock.Object,
            breachCheckerMock.Object,
            riskAssessmentServiceMock.Object);
    }


    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public async Task Analyze_WhenPasswordIsNullOrWhiteSpace_ReturnsInvalidResponse(string password)
    {
        // Act
        var result = await orchestrator.Analyze(password);

        // Assert
        Assert.False(result.IsValid);

        strengthCheckerMock.VerifyNoOtherCalls();
        breachCheckerMock.VerifyNoOtherCalls();
        riskAssessmentServiceMock.VerifyNoOtherCalls();
    }


    [Fact]
    public async Task Analyze_WhenValidPassword_ReturnsValidResponse()
    {
        
        var password = "StrongPassword123!";

        var strengthResult = CreateStrengthResult();
        var breachResult = CreateBreachResult();
        var riskResult = CreateRiskResult();

        SetupValidPasswordFlow(password, strengthResult, breachResult, riskResult);

        
        var result = await orchestrator.Analyze(password);

       
        Assert.NotNull(result);
        Assert.True(result.IsValid);
    }

  

    [Fact]
    public async Task Analyze_WhenCalled_InvokesStrengthAndBreachCheckers()
    {
        var password = "Test123";

        SetupValidPasswordFlow(
            password,
            CreateStrengthResult(),
            CreateBreachResult(),
            CreateRiskResult());

        await orchestrator.Analyze(password);

        strengthCheckerMock.Verify(
            s => s.CheckAsync(password, It.IsAny<CancellationToken>()),
            Times.Once);

        breachCheckerMock.Verify(
            b => b.CheckAsync(password, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Analyze_WhenCalled_AssessesRiskUsingStrengthAndBreachResults()
    {
        // Arrange
        var password = "Test123";

        var strengthResult = CreateStrengthResult();
        var breachResult = CreateBreachResult();
        var riskResult = CreateRiskResult();

        SetupValidPasswordFlow(password, strengthResult, breachResult, riskResult);

        // Act
        await orchestrator.Analyze(password);

        // Assert
        riskAssessmentServiceMock.Verify(
            r => r.Assess(strengthResult, breachResult),
            Times.Once);
    }

    private void SetupValidPasswordFlow(
      string password,
      StrengthResult strengthResult,
      BreachResult breachResult,
      RiskResult riskResult)
    {
        strengthCheckerMock
            .Setup(s => s.CheckAsync(password, It.IsAny<CancellationToken>()))
            .ReturnsAsync(strengthResult);

        breachCheckerMock
            .Setup(b => b.CheckAsync(password, It.IsAny<CancellationToken>()))
            .ReturnsAsync(breachResult);

        riskAssessmentServiceMock
            .Setup(r => r.Assess(strengthResult, breachResult))
            .Returns(riskResult);
    }

    private static StrengthResult CreateStrengthResult() =>
        new(
            Score: 70,
            Level: PasswordStrengthLevel.Medium,
            Issues: ImmutableArray<string>.Empty);

    private static BreachResult CreateBreachResult() =>
        new(
            IsBreached: false,
            Sources: Array.Empty<BreachSourceResult>());

    private static RiskResult CreateRiskResult() =>
        new(
            Level: RiskLevel.Low,
            Score: 10,
            Reasons: Array.Empty<string>());
}
