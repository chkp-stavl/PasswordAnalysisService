namespace PasswordAnalysisService.Utilities
{
    public interface IHibpClient
    {
        Task<string?> GetRangeAsync(string prefix, CancellationToken ct);
    }
}
