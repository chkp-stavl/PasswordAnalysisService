using Domain.Enums;

namespace Infrastructure.Breach
{
    public interface IBreachPrevalenceMapper
    {
        BreachPrevalence Map(int? count);
    }
}
