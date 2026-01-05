namespace PasswordAnalysisService
{
    public class Consts
    {
        public enum PasswordStrengthLevel
        {
            VeryWeak,
            Weak,
            Medium,
            Strong,
            VeryStrong
        }
        public enum RiskLevel
        {
            Low,
            Medium,
            High,
            Critical
        }
        public enum BreachPrevalence
        {
            Unknown,
            Low,
            Medium,
            High
        }
    }
}
