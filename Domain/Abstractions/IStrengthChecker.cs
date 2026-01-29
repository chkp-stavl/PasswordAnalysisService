using Domain.Models;

namespace Domain.Interfaces
{
    public interface IStrengthChecker
    {
        StrengthResult Check(string password, CancellationToken ct = default);
    }
}
