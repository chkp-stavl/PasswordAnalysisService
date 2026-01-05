
namespace PasswordAnalysisService.Logic
{
    public class BreachChecker : IBreachChecker
    {
        public Task<BreachResult> CheckAsync(string password, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            return Task.FromResult(
                new BreachResult(
                    IsBreached: false,
                    BreachCount: 0,
                    Source: "Stub"
                )
            );
        }
    }
}
