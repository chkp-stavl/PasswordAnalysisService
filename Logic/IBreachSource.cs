namespace PasswordAnalysisService.Logic
{
    public interface IBreachSource
    {
        Task<BreachSourceResult> CheckAsync(string password, CancellationToken ct);
    }

}
