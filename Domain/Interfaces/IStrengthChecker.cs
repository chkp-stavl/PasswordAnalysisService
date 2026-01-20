using Domain.Models;

namespace Domain.Interfaces
{
    public interface IStrengthChecker
    {
        Task<StrengthResult> CheckAsync(string password, CancellationToken ct = default);
    }
}
