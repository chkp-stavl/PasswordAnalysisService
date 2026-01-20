using Domain.Interfaces;
using Domain.Models;

namespace Infrastructure.Breach
{
    public class BreachChecker : IBreachChecker
    {
        private readonly IEnumerable<IBreachSource> sources;

        public BreachChecker(IEnumerable<IBreachSource> sources)
        {
            this.sources = sources;
        }

        public async Task<BreachResult> CheckAsync(string password, CancellationToken ct)
        {
            var tasks = sources.Select(s => SafeCheck(s, password, ct));
            var results = await Task.WhenAll(tasks);

            return new BreachResult(
                results.Any(r => r.IsBreached),
                results
            );
        }

        private async Task<BreachSourceResult> SafeCheck(
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
