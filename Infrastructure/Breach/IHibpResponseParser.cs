namespace Infrastructure.Breach
{
    public interface IHibpResponseParser
    {
        int? FindBreachCount(string response, string suffix);
    }
}
