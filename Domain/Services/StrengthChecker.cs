using Domain.Constants;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using System.Collections.Immutable;

namespace Core.Services
{
    public class StrengthChecker : IStrengthChecker
    {
        public StrengthResult Check(string password, CancellationToken ct)
        {
            var issuesBuilder = new List<string>();
            int score = 0;

            score += CheckLength(password, issuesBuilder);
            score += CheckUppercase(password, issuesBuilder);
            score += CheckLowercase(password, issuesBuilder);
            score += CheckDigit(password, issuesBuilder);
            score += CheckSpecialCharacter(password, issuesBuilder); ;

            score = Math.Min(score, StrengthConstants.MAX_SCORE);
            var level = score switch
            {
                < StrengthConstants.WEAK_THRESHOLD => PasswordStrengthLevel.Weak,
                < StrengthConstants.STRENGTH_MEDIUM_THRESHOLD => PasswordStrengthLevel.Medium,
                _ => PasswordStrengthLevel.Strong
            };
            var result = new StrengthResult(
                score,
                level,
                issuesBuilder
            );

            return result;
        }

        private static int CheckLength(string password, List<string> issues)
        {
            if (password.Length >= StrengthConstants.MIN_PASS_LEN)
                return StrengthConstants.LENGTH_SCORE;

            issues.Add("Password is too short");
            return 0;
        }

        private static int CheckUppercase(
            string password,
            List<string> issues)
        {
            if (password.Any(char.IsUpper))
                return StrengthConstants.UPPERCASE_SCORE;

            issues.Add("Missing uppercase letter");
            return 0;
        }

        private static int CheckLowercase(
            string password,
            List<string> issues)
        {
            if (password.Any(char.IsLower))
                return StrengthConstants.LOWERCASE_SCORE;

            issues.Add("Missing lowercase letter");
            return 0;
        }

        private static int CheckDigit(
            string password,
            List<string> issues)
        {
            if (password.Any(char.IsDigit))
                return StrengthConstants.DIGIT_SCORE;

            issues.Add("Missing digit");
            return 0;
        }

        private static int CheckSpecialCharacter(
            string password,
            List<string> issues)
        {
            if (password.Any(ch => !char.IsLetterOrDigit(ch)))
                return StrengthConstants.SPECIAL_CHAR_SCORE;

            issues.Add("Missing special character");
            return 0;
        }

    }
}
