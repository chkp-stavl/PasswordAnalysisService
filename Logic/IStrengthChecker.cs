using System.Collections.Immutable;
using static PasswordAnalysisService.Consts;

namespace PasswordAnalysisService.Logic
{
    public interface IStrengthChecker
    {
        Task<StrengthResult> CheckAsync(string password, CancellationToken ct = default);
    }

    public record StrengthResult
    (
        int Score,
        PasswordStrengthLevel Level,
        ImmutableArray<string> Issues
    );
}
