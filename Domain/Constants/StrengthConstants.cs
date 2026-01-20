namespace Domain.Constants
{
    public static class StrengthConstants
    {
        public const int MAX_SCORE = 100;
        public const int MIN_PASS_LEN = 8;
        public const int LENGTH_SCORE = 25;
        public const int UPPERCASE_SCORE = 15;
        public const int LOWERCASE_SCORE = 15;
        public const int DIGIT_SCORE = 15;
        public const int SPECIAL_CHAR_SCORE = 30;
        public const int WEAK_THRESHOLD = 50;
        public const int STRENGTH_MEDIUM_THRESHOLD = 80;
    }
}
