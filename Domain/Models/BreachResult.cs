namespace Domain.Models
{
    public record BreachResult(
    bool IsBreached,
    IReadOnlyList<BreachSourceResult> Sources

    )
    {
        public bool AllSourcesUnavailable =>
        Sources.Count > 0 && Sources.All(s => !s.IsAvailable);
    };
}
