
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using static PasswordAnalysisService.Consts;

namespace PasswordAnalysisService.Logic
{
    public class HibpBreachSource : IBreachSource
    {
        private const string SourceName = "HaveIBeenPwned";
        private readonly HttpClient httpClient;

        public HibpBreachSource(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
                "PasswordAnalysisService/1.0"
    );
        }

        public async Task<BreachSourceResult> CheckAsync(string password, CancellationToken ct = default)
        {
            try
            {
                var hash = Sha1Hash(password);
                var prefix = hash[..5];
                var suffix = hash[5..];
                var response = await httpClient.GetAsync(
               $"https://api.pwnedpasswords.com/range/{prefix}", ct);

                if (!response.IsSuccessStatusCode)
                {
                    return BreachSourceResult.Unavailable(SourceName);
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
                        return new BreachSourceResult(
                            IsBreached: true,
                            BreachCount: int.Parse(count),
                            Source: "HaveIBeenPwned",
                            Prevalence: MapPrevalence(int.Parse(count))
                        );
                    }
                }

                return new BreachSourceResult(
                    IsBreached: false,
                    BreachCount: 0,
                    Source: "HaveIBeenPwned",
                    Prevalence: BreachPrevalence.Unknown
                );
            }
            catch 
            {
                return BreachSourceResult.Unavailable("HaveIBeenPwned");
            }
        }

        private static string Sha1Hash(string input)
        {
            using var sha1 = SHA1.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha1.ComputeHash(bytes);
            return Convert.ToHexString(hash);
        }

        private static BreachPrevalence MapPrevalence(int? breachCount)
        {
            if (breachCount is null or 0)
                return BreachPrevalence.Unknown;

            return breachCount switch
            {
                >= 1000 => BreachPrevalence.High,
                >= 10 => BreachPrevalence.Medium,
                _ => BreachPrevalence.Low
            };
        }
    }
}
