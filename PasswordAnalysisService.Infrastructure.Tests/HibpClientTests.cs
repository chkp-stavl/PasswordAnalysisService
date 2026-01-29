using PasswordAnalysisService.Infrastructure.Breach.Hibp;
using System.Net;

namespace PasswordAnalysisService.Infrastructure.Tests
{
    public class HibpClientTests
    {
        [Fact]
        public async Task GetRangeAsync_WhenResponseOk_ReturnsContent()
        {
            var handler = new FakeHttpMessageHandler(
                HttpStatusCode.OK,
                "ABCDEF:10");

            var client = new HttpClient(handler);
            var hibpClient = new HibpClient(client);

            var result = await hibpClient.GetRangeAsync("ABCDE", CancellationToken.None);

            Assert.Equal("ABCDEF:10", result);
        }

        [Fact]
        public async Task GetRangeAsync_WhenResponseFails_ReturnsNull()
        {
            var handler = new FakeHttpMessageHandler(
                HttpStatusCode.InternalServerError,
                "");

            var client = new HttpClient(handler);
            var hibpClient = new HibpClient(client);

            var result = await hibpClient.GetRangeAsync("ABCDE", CancellationToken.None);

            Assert.Null(result);
        }
    }
}
