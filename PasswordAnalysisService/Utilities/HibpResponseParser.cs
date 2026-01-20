namespace PasswordAnalysisService.Utilities
{
    public sealed class HibpResponseParser : IHibpResponseParser
    {
        public int? FindBreachCount(string response, string suffix)
        {
            foreach (var line in response.Split('\n'))
            {
                var parts = line.Split(':');
                if (parts.Length != 2)
                    continue;

                if (parts[0].Equals(suffix, StringComparison.OrdinalIgnoreCase))
                    return int.Parse(parts[1]);
            }

            return null;
        }
    }

}
