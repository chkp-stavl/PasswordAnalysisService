
using static PasswordAnalysisService.Consts;

namespace PasswordAnalysisService.Logic
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
            var sourceResults = new List<BreachSourceResult>();

            foreach (var source in sources)
            {
                var result = await source.CheckAsync(password, ct);
                if (result != null)
                {
                    sourceResults.Add(new BreachSourceResult(
                        result.Source,
                        result.IsBreached,
                        result.BreachCount,
                        result.Prevalence
                        ));
                }
            }

            return new BreachResult(
                IsBreached: sourceResults.Any(r => r.IsBreached),
                Sources: sourceResults
            );

        }

        

    }
}
