namespace Infrastructure.Breach
{
    public interface IHibpClient
    {
        Task<string?> GetRangeAsync(string prefix, CancellationToken ct);
    }
}
