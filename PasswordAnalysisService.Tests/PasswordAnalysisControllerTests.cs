namespace PasswordAnalysisService.Tests;

using Microsoft.AspNetCore.Mvc;
using Moq;
using PasswordAnalysisService.Controllers;
using PasswordAnalysisService.Models.Requests;
using PasswordAnalysisService.Models.Responses;
using PasswordAnalysisService.Services;

public class PasswordAnalysisControllerTests
{
    private readonly Mock<IPasswordAnalysisOrchestrator> orchestratorMock;
    private readonly PasswordAnalysisController controller;

    public PasswordAnalysisControllerTests()
    {
        orchestratorMock = new Mock<IPasswordAnalysisOrchestrator>();
        controller = new PasswordAnalysisController(orchestratorMock.Object);
    }

    [Fact]
    public async Task AnalyzeAsync_WhenCalled_ReturnsOkResultWithServiceResponse()
    {
        var request = new PasswordAnalysisRequest
        {
            Password = "TestPassword123!"
        };

        var expectedResponse = new PasswordAnalysisResponse();

        orchestratorMock
            .Setup(o => o.Analyze(request.Password, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        var result = await controller.AnalyzeAsync(request);

      
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResponse, okResult.Value);
    }

    [Fact]
    public async Task AnalyzeAsync_WhenServiceThrowsException_ReturnsInternalServerError()
    {
        var request = new PasswordAnalysisRequest
        {
            Password = "AnyPassword"
        };

        orchestratorMock
            .Setup(o => o.Analyze(request.Password, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Service failure"));

        var result = await controller.AnalyzeAsync(request);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("Internal server error", objectResult.Value);
    }

    [Fact]
    public async Task AnalyzeAsync_WhenCalled_CallsAnalyzeWithCorrectPassword()
    {
        var request = new PasswordAnalysisRequest
        {
            Password = "VerifyCall123"
        };

        orchestratorMock
            .Setup(o => o.Analyze(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PasswordAnalysisResponse());

        await controller.AnalyzeAsync(request);

        orchestratorMock.Verify(
            o => o.Analyze(request.Password, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
