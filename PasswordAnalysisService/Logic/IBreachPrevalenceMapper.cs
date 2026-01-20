using static PasswordAnalysisService.Consts;

namespace PasswordAnalysisService.Logic
{
    public interface IBreachPrevalenceMapper
    {
        BreachPrevalence Map(int? count);
    }
}
