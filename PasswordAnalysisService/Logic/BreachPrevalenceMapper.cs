using static PasswordAnalysisService.Consts;

namespace PasswordAnalysisService.Logic
{
    public sealed class BreachPrevalenceMapper : IBreachPrevalenceMapper
    {
        public BreachPrevalence Map(int? count)
        {
            if (count is null or 0)
                return BreachPrevalence.Unknown;

            return count switch
            {
                >= HIGH_THRESHOLD_BREACH => BreachPrevalence.High,
                >= MEDIUM_THRESHOLD_BREACH => BreachPrevalence.Medium,
                _ => BreachPrevalence.Low
            };
        }
    }

}
