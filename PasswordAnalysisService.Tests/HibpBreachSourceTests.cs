namespace PasswordAnalysisService.Tests;

using PasswordAnalysisService.Logic;
using PasswordAnalysisService.Utilities;
using static PasswordAnalysisService.Consts;

public class HibpBreachSourceTests
{
    [Fact]
    public async Task CheckAsync_WhenPasswordIsFound_ReturnsBreachedResult()
    {
        var hasher = new FakeHasher("ABCDESUFFIX");
        var client = new FakeHibpClient($"SUFFIX:{HIGH_THRESHOLD_BREACH}");
        var parser = new HibpResponseParser();
        var mapper = new BreachPrevalenceMapper();

        var source = new HibpBreachSource(hasher, client, parser, mapper);

        var result = await source.CheckAsync("password123");

        Assert.True(result.IsBreached);
        Assert.Equal(HIGH_THRESHOLD_BREACH, result.BreachCount);
        Assert.Equal(BreachPrevalence.High, result.Prevalence);
        Assert.True(result.IsAvailable);
    }

    [Fact]
    public async Task CheckAsync_WhenPasswordIsNotFound_ReturnsNotBreached()
    {
        var hasher = new FakeHasher("ABCDE12345SUFFIX");
        var client = new FakeHibpClient("OTHER:5");
        var parser = new HibpResponseParser();
        var mapper = new BreachPrevalenceMapper();

        var source = new HibpBreachSource(hasher, client, parser, mapper);

        var result = await source.CheckAsync("safePassword");

        Assert.False(result.IsBreached);
        Assert.Equal(0, result.BreachCount);
        Assert.Equal(BreachPrevalence.Unknown, result.Prevalence);
    }

    [Fact]
    public async Task CheckAsync_WhenApiReturnsFailure_ReturnsUnavailable()
    {
        var hasher = new FakeHasher("ABCDE12345SUFFIX");
        var client = new FakeHibpClient(null); 
        var parser = new HibpResponseParser();
        var mapper = new BreachPrevalenceMapper();

        var source = new HibpBreachSource(hasher, client, parser, mapper);

        var result = await source.CheckAsync("password");

        Assert.False(result.IsAvailable);
    }

    [Fact]
    public async Task CheckAsync_WhenExceptionThrown_ReturnsUnavailable()
    {
        var hasher = new FakeHasher("ABCDE12345SUFFIX");
        var client = new ThrowingHibpClient();
        var parser = new HibpResponseParser();
        var mapper = new BreachPrevalenceMapper();

        var source = new HibpBreachSource(hasher, client, parser, mapper);

        var result = await source.CheckAsync("password");

        Assert.False(result.IsAvailable);
    }

    [Fact]
    public async Task CheckAsync_WhenMediumBreachCount_ReturnsMediumPrevalence()
    {
        var hasher = new FakeHasher("ABCDESUFFIX");
        var client = new FakeHibpClient($"SUFFIX:{MEDIUM_THRESHOLD_BREACH}");
        var parser = new HibpResponseParser();
        var mapper = new BreachPrevalenceMapper();

        var source = new HibpBreachSource(hasher, client, parser, mapper);

        var result = await source.CheckAsync("password123");

        Assert.Equal(BreachPrevalence.Medium, result.Prevalence);
    }

    private sealed class FakeHasher : IPasswordHasher
    {
        private readonly string hash;

        public FakeHasher(string hash)
        {
            this.hash = hash;
        }

        public string Hash(string password) => hash;
    }

    private sealed class FakeHibpClient : IHibpClient
    {
        private readonly string? response;

        public FakeHibpClient(string? response)
        {
            this.response = response;
        }

        public Task<string?> GetRangeAsync(string prefix, CancellationToken ct)
        {
            return Task.FromResult(response);
        }
    }

    private sealed class ThrowingHibpClient : IHibpClient
    {
        public Task<string?> GetRangeAsync(string prefix, CancellationToken ct)
        {
            throw new HttpRequestException("Network error");
        }
    }
}
