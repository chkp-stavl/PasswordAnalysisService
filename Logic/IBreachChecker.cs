namespace PasswordAnalysisService.Logic
{
    public interface IBreachChecker
    {
        Task<BreachResult> CheckAsync(string password, CancellationToken ct = default);
    }

    public record BreachResult(
        bool IsBreached,
        int? BreachCount,        
        string? Source          
    );

}
