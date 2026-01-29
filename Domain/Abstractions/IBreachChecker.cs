using Domain.Models;

namespace Domain.Interfaces
{
    public interface IBreachChecker
    {
        Task<BreachResult> CheckAsync(string password, CancellationToken ct);
    }
}
