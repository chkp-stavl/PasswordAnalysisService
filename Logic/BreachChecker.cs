
namespace PasswordAnalysisService.Logic
{
    public class BreachChecker : IBreachChecker
    {
        public Task<BreachResult> CheckAsync(string password, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
