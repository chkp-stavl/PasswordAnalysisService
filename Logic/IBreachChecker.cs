using static PasswordAnalysisService.Consts;

namespace PasswordAnalysisService.Logic
{
    public interface IBreachChecker
    {
        Task<BreachResult> CheckAsync(string password, CancellationToken ct = default);
    }

    public record BreachResult(
    bool IsBreached,
    IReadOnlyList<BreachSourceResult> Sources

    )
    {
        public bool AllSourcesUnavailable =>
        Sources.Count > 0 && Sources.All(s => !s.IsAvailable);
    };

    public record BreachSourceResult(
        string Source,
        bool IsBreached,
        int? BreachCount,
        BreachPrevalence Prevalence,
        bool IsAvailable = true

    )
    {
        public static BreachSourceResult Unavailable(string source) =>
        new(
            IsBreached: false,
            BreachCount: null,
            Source: source,
            Prevalence: BreachPrevalence.Unknown,
            IsAvailable: false
        );
    };


}
