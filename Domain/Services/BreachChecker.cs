using Domain.Interfaces;
using Domain.Models;

namespace Core.Services
{
    public class BreachChecker : IBreachChecker
    {
        private readonly IEnumerable<IBreachSource> _breachSources;

        public BreachChecker(IEnumerable<IBreachSource> sources)
        {
           _breachSources = sources;
        }

        public async Task<BreachResult> CheckAsync(string password, CancellationToken ct)
        {
            var tasks = _breachSources.Select(s => SafeCheck(s, password, ct));
            var results = await Task.WhenAll(tasks);

            return new BreachResult(
                results.Any(r => r.IsBreached),
                results != null ? [.. results] : new List<BreachSourceResult>()
            );
        }

        private static async Task<BreachSourceResult> SafeCheck(
            IBreachSource source,
            string password,
            CancellationToken ct)
        {
            try
            {
                return await source.CheckAsync(password, ct);
            }
            catch
            {
                return BreachSourceResult.Unavailable(source.GetType().Name);
            }
        }
    }
}
