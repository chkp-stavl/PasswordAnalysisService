namespace PasswordAnalysisService.Tests;

using API.ControllerLogics;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PasswordAnalysisService.Controllers;
using PasswordAnalysisService.Models.Requests;
using PasswordAnalysisService.Models.Responses;

public class PasswordAnalysisControllerTests
{
    private readonly Mock<IPasswordAnalysisControllerLogic> fakePasswordAnalysisControllerLogic;
    private readonly PasswordAnalysisController controller;

    public PasswordAnalysisControllerTests()
    {
        fakePasswordAnalysisControllerLogic = new Mock<IPasswordAnalysisControllerLogic>();
        controller = new PasswordAnalysisController(fakePasswordAnalysisControllerLogic.Object);
    }

    [Fact]
    public async Task AnalyzeAsync_WhenCalled_ReturnsOkResultWithServiceResponse()
    {
        var dto = new AnalyzePasswordResponseDto { IsValid = true };

        fakePasswordAnalysisControllerLogic.Setup(l => l.Analyze(It.IsAny<AnalyzePasswordRequestDto>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(dto);

        var result = await controller.Analyze(new AnalyzePasswordRequestDto() { Password = "xxx"}, CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(dto, okResult.Value);
    }

    [Fact]
    public async Task AnalyzeAsync_WhenLogicReturnsInvalidResponse_ReturnsBadRequest()
    {
        var request = new AnalyzePasswordRequestDto
        {
            Password = "bad"
        };

        var response = AnalyzePasswordResponseDto.Invalid("error");

        fakePasswordAnalysisControllerLogic
            .Setup(l => l.Analyze(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await controller.Analyze(request, CancellationToken.None);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(response, badRequest.Value);
    }

    [Fact]
    public async Task AnalyzeAsync_WhenServiceThrowsException_ReturnsInternalServerError()
    {
        var request = new AnalyzePasswordRequestDto
        {
            Password = "AnyPassword"
        };

        fakePasswordAnalysisControllerLogic
            .Setup(l => l.Analyze(request, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("boom"));

        var result = await controller.Analyze(request, CancellationToken.None);

        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("Internal server error", objectResult.Value);
    }
}
