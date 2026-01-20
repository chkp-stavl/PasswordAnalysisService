using Domain.Interfaces;
using Domain.Models;

namespace Infrastructure.Breach
{
    public class HibpBreachSource : IBreachSource
    {
        private const string SourceName = "HaveIBeenPwned";
        private readonly IPasswordHasher hasher;
        private readonly IHibpClient client;
        private readonly IHibpResponseParser parser;
        private readonly IBreachPrevalenceMapper prevalenceMapper;

        public HibpBreachSource(IPasswordHasher hasher, IHibpClient client, IHibpResponseParser parser, IBreachPrevalenceMapper prevalenceMapper)
        {
            this.hasher = hasher;
            this.client = client;
            this.parser = parser;
            this.prevalenceMapper = prevalenceMapper;
        }

        public async Task<BreachSourceResult> CheckAsync(string password, CancellationToken ct = default)
        {
            try
            {
                var hash = hasher.Hash(password);
                var prefix = hash[..5];
                var suffix = hash[5..];
                var response = await client.GetRangeAsync(prefix, ct);
                if (response is null)
                    return BreachSourceResult.Unavailable(SourceName);

                var breachCount = parser.FindBreachCount(response, suffix);

                return new BreachSourceResult(
                    IsBreached: breachCount.HasValue,
                    BreachCount: breachCount ?? 0,
                    Source: SourceName,
                    Prevalence: prevalenceMapper.Map(breachCount)
                );
            }
            catch
            {
                return BreachSourceResult.Unavailable("HaveIBeenPwned");
            }
        }

    }
}
