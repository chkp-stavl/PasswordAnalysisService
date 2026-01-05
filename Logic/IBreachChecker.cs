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
   
);

    public record BreachSourceResult(
        string Source,
        bool IsBreached,
        int? BreachCount,
        BreachPrevalence Prevalence
    );


}
