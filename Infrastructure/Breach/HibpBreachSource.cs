using Domain.Interfaces;
using Domain.Models;

namespace Infrastructure.Breach
{
    public class HibpBreachSource : IBreachSource
    {
        private const string SourceName = "HaveIBeenPwned";
        private readonly IPasswordHasher _passwordHasher;
        private readonly IHibpClient _hibpClientient;
        private readonly IHibpResponseParser _hibpResponseParser;
        private readonly IBreachPrevalenceMapper _breachPrevalenceMapper;

        public HibpBreachSource(IPasswordHasher hasher, IHibpClient client, IHibpResponseParser parser, IBreachPrevalenceMapper prevalenceMapper)
        {
            this._passwordHasher = hasher;
            this._hibpClientient = client;
            this._hibpResponseParser = parser;
            this._breachPrevalenceMapper = prevalenceMapper;
        }

        public async Task<BreachSourceResult> CheckAsync(string password, CancellationToken ct = default)
        {
            try
            {
                var hash = _passwordHasher.Hash(password);
                var prefix = hash[..5];
                var suffix = hash[5..];
                var response = await _hibpClientient.GetRangeAsync(prefix, ct);
                if (response is null)
                    return BreachSourceResult.Unavailable(SourceName);

                var breachCount = _hibpResponseParser.FindBreachCount(response, suffix);

                return new BreachSourceResult(
                    IsBreached: breachCount.HasValue,
                    BreachCount: breachCount ?? 0,
                    Source: SourceName,
                    Prevalence: _breachPrevalenceMapper.Map(breachCount)
                );
            }
            catch
            {
                return BreachSourceResult.Unavailable("HaveIBeenPwned");
            }
        }

    }
}
