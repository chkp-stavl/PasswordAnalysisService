namespace PasswordAnalysisService.Utilities
{
    public interface IHibpResponseParser
    {
        int? FindBreachCount(string response, string suffix);
    }
}
