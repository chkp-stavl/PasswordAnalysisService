using Domain.Constants;
using Domain.Enums;

namespace Infrastructure.Breach
{
    public sealed class BreachPrevalenceMapper : IBreachPrevalenceMapper
    {
        public BreachPrevalence Map(int? count)
        {
            if (count is null or 0)
                return BreachPrevalence.Unknown;

            return count switch
            {
                >= BreachThresholds.HIGH => BreachPrevalence.High,
                >= BreachThresholds.MEDIUM => BreachPrevalence.Medium,
                _ => BreachPrevalence.Low
            };
        }
    }
}
