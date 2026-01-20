namespace Domain.Constants
{
    public static class RiskConstants
    {
        public const int CRITICAL_THRESHOLD = 80;
        public const int HIGH_THRESHOLD = 60;
        public const int RISK_MEDIUM_THRESHOLD = 30;

        // Breach contribution
        public const int BREACH_FOUND_SCORE = 40;
        public const int HIGH_PREVALENCE_BONUS = 10;

        // Strength penalties
        public const int VERY_WEAK_PENALTY = 40;
        public const int WEAK_PENALTY = 30;
        public const int MEDIUM_PENALTY = 15;

        public const int MAX_SCORE = 100;
    }
}
