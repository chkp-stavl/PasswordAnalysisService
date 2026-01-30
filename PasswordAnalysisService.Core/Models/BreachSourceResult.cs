using Domain.Enums;

namespace Domain.Models
{
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
