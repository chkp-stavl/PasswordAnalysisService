
namespace PasswordAnalysisService.Logic
{
    public class StrengthChecker : IStrengthChecker
    {
        public Task<StrengthResult> CheckAsync(string password, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
