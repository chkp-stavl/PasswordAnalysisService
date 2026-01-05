
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace PasswordAnalysisService.Logic
{
    public class HibpApiClient : IBreachChecker
    {
        private readonly HttpClient httpClient;

        public HibpApiClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
                "PasswordAnalysisService/1.0"
    );
        }

        public async Task<BreachResult> CheckAsync(string password, CancellationToken ct = default)
        {
            var hash = Sha1Hash(password);

            var prefix = hash.Substring(0, 5);   
            var suffix = hash.Substring(5);
            var response = await httpClient.GetAsync(
            $"https://api.pwnedpasswords.com/range/{prefix}",
            ct
            );

            if (!response.IsSuccessStatusCode)
            {
                return new BreachResult(
                    IsBreached: false,
                    BreachCount: null,
                    Source: "HIBP unavailable"
                );
            }
            var content = await response.Content.ReadAsStringAsync(ct);

            foreach (var line in content.Split('\n'))
            {
                var parts = line.Split(':');
                if (parts.Length != 2)
                    continue;

                var responseSuffix = parts[0].Trim();
                var count = parts[1].Trim();

                if (responseSuffix.Equals(suffix, StringComparison.OrdinalIgnoreCase))
                {
                    return new BreachResult(
                        IsBreached: true,
                        BreachCount: int.Parse(count),
                        Source: "HaveIBeenPwned"
                    );
                }
            }

            return new BreachResult(
                IsBreached: false,
                BreachCount: 0,
                Source: "HaveIBeenPwned"
            );
        }

        private static string Sha1Hash(string input)
        {
            using var sha1 = SHA1.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha1.ComputeHash(bytes);
            return Convert.ToHexString(hash);
        }
    }
}
