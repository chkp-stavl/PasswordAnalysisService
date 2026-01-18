namespace PasswordAnalysisService.Tests;

using Moq;
using PasswordAnalysisService.Logic;

public class BreachCheckerTests
{
    [Fact]
    public async Task CheckAsync_WhenNoSources_ReturnsNotBreached()
    {
        var checker = new BreachChecker(Enumerable.Empty<IBreachSource>());

        var result = await checker.CheckAsync("password", CancellationToken.None);

        Assert.False(result.IsBreached);
        Assert.Empty(result.Sources);
    }

    [Fact]
    public async Task CheckAsync_WhenNoSourceReportsBreach_ReturnsNotBreached()
    {
        var source1 = CreateSource(isBreached: false);
        var source2 = CreateSource(isBreached: false);

        var checker = new BreachChecker(new[] { source1.Object, source2.Object });

        var result = await checker.CheckAsync("password", CancellationToken.None);

        Assert.False(result.IsBreached);
        Assert.All(result.Sources, r => Assert.False(r.IsBreached));
    }

    [Fact]
    public async Task CheckAsync_WhenAnySourceReportsBreach_ReturnsBreached()
    {
        var safeSource = CreateSource(isBreached: false);
        var breachedSource = CreateSource(isBreached: true);

        var checker = new BreachChecker(new[] { safeSource.Object, breachedSource.Object });

        var result = await checker.CheckAsync("password", CancellationToken.None);

        Assert.True(result.IsBreached);
        Assert.Contains(result.Sources, r => r.IsBreached);
    }

    [Fact]
    public async Task CheckAsync_WhenSourceThrowsException_MarksSourceAsUnavailable()
    {
        var throwingSource = new Mock<IBreachSource>();
        throwingSource
            .Setup(s => s.CheckAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("boom"));

        var checker = new BreachChecker(new[] { throwingSource.Object });

        var result = await checker.CheckAsync("password", CancellationToken.None);

        Assert.Single(result.Sources);
        Assert.False(result.Sources[0].IsAvailable);
    }

    [Fact]
    public async Task CheckAsync_WhenOneSourceFails_OtherSourcesStillChecked()
    {
        var throwingSource = new Mock<IBreachSource>();
        throwingSource
            .Setup(s => s.CheckAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        var healthySource = CreateSource(isBreached: false);

        var checker = new BreachChecker(new[] { throwingSource.Object, healthySource.Object });

        var result = await checker.CheckAsync("password", CancellationToken.None);

        Assert.Equal(2, result.Sources.Count);
        Assert.Contains(result.Sources, r => !r.IsAvailable);
        Assert.Contains(result.Sources, r => r.IsAvailable);
    }

    private static Mock<IBreachSource> CreateSource(bool isBreached)
    {
        var mock = new Mock<IBreachSource>();

        mock.Setup(s => s.CheckAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new BreachSourceResult(
                    Source: "TestSource",
                    IsBreached: isBreached,
                    IsAvailable: true,
                    BreachCount: null,
                    Prevalence: 0.0));

        return mock;
    }
}
