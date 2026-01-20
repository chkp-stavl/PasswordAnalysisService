using Domain.Models;

namespace Domain.Interfaces
{
    public interface IBreachSource
    {
        Task<BreachSourceResult> CheckAsync(string password, CancellationToken ct);
    }
}
