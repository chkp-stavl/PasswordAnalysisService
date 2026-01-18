namespace PasswordAnalysisService.Tests;

using PasswordAnalysisService.Logic;
using System.Net;
using System.Net.Http;
using System.Text;
using static PasswordAnalysisService.Consts;

public class HibpBreachSourceTests
{
    private static HttpClient CreateHttpClient(
        HttpStatusCode statusCode,
        string responseContent)
    {
        var handler = new FakeHttpMessageHandler(statusCode, responseContent);
        return new HttpClient(handler);
    }

    [Fact]
    public async Task CheckAsync_WhenPasswordIsFound_ReturnsBreachedResult()
    {
        var password = "password123";
        var hash = "CBFDAC6008F9CAB4083784CBD1874F76618D2A97"; // SHA1(password123)
        var suffix = hash[5..];

        var response =
            $"{suffix}:1000\nOTHERHASH:5";

        var client = CreateHttpClient(HttpStatusCode.OK, response);
        var source = new HibpBreachSource(client);


        var result = await source.CheckAsync(password);

       

        Assert.True(result.IsBreached);
        Assert.Equal(1000, result.BreachCount);
        Assert.Equal(BreachPrevalence.High, result.Prevalence);
    }

    [Fact]
    public async Task CheckAsync_WhenPasswordIsNotFound_ReturnsNotBreached()
    {
        var password = "safePassword!";
        var response = "ABCDEF1234567890:5";

        var client = CreateHttpClient(HttpStatusCode.OK, response);
        var source = new HibpBreachSource(client);

      
        var result = await source.CheckAsync(password);

      
        Assert.False(result.IsBreached);
        Assert.Equal(0, result.BreachCount);
        Assert.Equal(BreachPrevalence.Unknown, result.Prevalence);
    }

    [Fact]
    public async Task CheckAsync_WhenApiReturnsFailure_ReturnsUnavailable()
    {
        var client = CreateHttpClient(HttpStatusCode.InternalServerError, "");
        var source = new HibpBreachSource(client);


        var result = await source.CheckAsync("password");

        Assert.False(result.IsAvailable);
    }

    [Fact]
    public async Task CheckAsync_WhenExceptionThrown_ReturnsUnavailable()
    {
        var client = new HttpClient(new ThrowingHttpMessageHandler());
        var source = new HibpBreachSource(client);

        var result = await source.CheckAsync("password");

        Assert.False(result.IsAvailable);
    }

    [Fact]
    public async Task CheckAsync_WhenMediumBreachCount_ReturnsMediumPrevalence()
    {
        var password = "password123";
        var hash = "CBFDAC6008F9CAB4083784CBD1874F76618D2A97";
        var suffix = hash[5..];

        var response = $"{suffix}:20";
        var client = CreateHttpClient(HttpStatusCode.OK, response);
        var source = new HibpBreachSource(client);

        var result = await source.CheckAsync(password);

        Assert.Equal(BreachPrevalence.Medium, result.Prevalence);
    }


    private sealed class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpStatusCode statusCode;
        private readonly string content;

        public FakeHttpMessageHandler(HttpStatusCode statusCode, string content)
        {
            this.statusCode = statusCode;
            this.content = content;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(content, Encoding.UTF8)
            });
        }
    }

    private sealed class ThrowingHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            throw new HttpRequestException("Network error");
        }
    }
}
