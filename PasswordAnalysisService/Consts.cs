namespace PasswordAnalysisService
{
    public class Consts
    {
        public const int MAX_SCORE = 100;

        // Breach contribution
        public const int BREACH_FOUND_SCORE = 40;
        public const int HIGH_PREVALENCE_BONUS = 10;

        // Strength penalties
        public const int VERY_WEAK_PENALTY = 40;
        public const int WEAK_PENALTY = 30;
        public const int MEDIUM_PENALTY = 15;

        // Risk thresholds
        public const int CRITICAL_THRESHOLD = 80;
        public const int HIGH_THRESHOLD = 60;
        public const int RISK_MEDIUM_THRESHOLD = 30;


        public const int MIN_PASSWORD_LENGTH = 8;

        public const int  MIN_PASS_LEN = 8;
        public const int LENGTH_SCORE = 25;
        public const int UPPERCASE_SCORE = 15;
        public const int LOWERCASE_SCORE = 15;
        public const int DIGIT_SCORE = 15;
        public const int SPECIAL_CHAR_SCORE = 30;

        public const int WEAK_THRESHOLD = 50;
        public const int STRENGTH_MEDIUM_THRESHOLD = 80;
        public const int HIGH_THRESHOLD_BREACH = 1000;
        public const int MEDIUM_THRESHOLD_BREACH = 10;

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
