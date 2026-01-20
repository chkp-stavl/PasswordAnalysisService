namespace PasswordAnalysisService.Tests;

using Moq;
using PasswordAnalysisService.Services;
using System.Collections.Immutable;
using Application.Interfaces;
using PasswordAnalysisService.Models.Requests;
using Application.Requests;
using Application.Responses;
using Domain.Enums;
using Domain.Models;

public class PasswordAnalysisControllerLogicTests
{
    private readonly Mock<IAnalyzePasswordUseCase> useCaseMock;
    private readonly PasswordAnalysisControllerLogic logic;

    public PasswordAnalysisControllerLogicTests()
    {
        useCaseMock = new Mock<IAnalyzePasswordUseCase>();
        logic = new PasswordAnalysisControllerLogic(useCaseMock.Object);
    }


    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public async Task AnalyzeAsync_WhenPasswordIsNullOrWhiteSpace_ReturnsInvalidResponse(string password)
    {
        
        var request = new AnalyzePasswordRequestDto
        {
            Password = password
        };

        var result = await logic.Analyze(request, CancellationToken.None);

 
        Assert.False(result.IsValid);

        useCaseMock.VerifyNoOtherCalls();
    }



    [Fact]
    public async Task AnalyzeAsync_WhenUseCaseSucceeds_ReturnsValidMappedResponse()
    {

        var request = new AnalyzePasswordRequestDto
        {
            Password = "StrongPassword123!"
        };

        useCaseMock
            .Setup(u => u.ExecuteAsync(
                It.IsAny<AnalyzePasswordRequest>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateValidResult());

        var result = await logic.Analyze(request, CancellationToken.None);

        Assert.True(result.IsValid);

        Assert.NotNull(result.Strength);
        Assert.Equal(80, result.Strength.Score);
        Assert.Equal(PasswordStrengthLevel.Strong.ToString(), result.Strength.Level);

        Assert.NotNull(result.Breach);
        Assert.False(result.Breach.IsCompromised);

        Assert.NotNull(result.Risk);
        Assert.Equal(10, result.Risk.Score);
        Assert.Equal(RiskLevel.Low.ToString(), result.Risk.Level);

        useCaseMock.Verify(
            u => u.ExecuteAsync(
                It.IsAny<AnalyzePasswordRequest>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }


    [Fact]
    public async Task AnalyzeAsync_WhenUseCaseThrowsException_ReturnsInvalidResponse()
    {
        var request = new AnalyzePasswordRequestDto
        {
            Password = "AnyPassword"
        };

        useCaseMock
            .Setup(u => u.ExecuteAsync(
                It.IsAny<AnalyzePasswordRequest>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("boom"));

        var result = await logic.Analyze(request, CancellationToken.None);

        Assert.False(result.IsValid);
        Assert.Equal("Unexpected error occurred", result.Error);
    }


    private static AnalyzePasswordResult CreateValidResult()
    {
        return new AnalyzePasswordResult(
            Strength: new StrengthResult(
                Score: 80,
                Level: PasswordStrengthLevel.Strong,
                Issues: new ImmutableArray<string>()
            ),
            Breach: new BreachResult(
                IsBreached: false,
                Sources: Array.Empty<BreachSourceResult>()
            ),
            Risk: new RiskResult(
                Level: RiskLevel.Low,
                Score: 10,
                Reasons: new[] { "No significant risk factors detected" }
            )
        );
    }
}


