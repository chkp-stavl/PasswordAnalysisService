using Domain.Constants;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using System.Collections.Immutable;

namespace Infrastructure.Strength
{
    public class StrengthChecker : IStrengthChecker
    {
        public Task<StrengthResult> CheckAsync(string password, CancellationToken ct)
        {
            var issuesBuilder = ImmutableArray.CreateBuilder<string>();
            int score = 0;

            if (password.Length >= StrengthConstants.MIN_PASS_LEN)
                score += StrengthConstants.LENGTH_SCORE;
            else
                issuesBuilder.Add("Password is too short");

            if (password.Any(char.IsUpper))
                score += StrengthConstants.UPPERCASE_SCORE;
            else
                issuesBuilder.Add("Missing uppercase letter");

            if (password.Any(char.IsLower))
                score += StrengthConstants.LOWERCASE_SCORE;
            else
                issuesBuilder.Add("Missing lowercase letter");

            if (password.Any(char.IsDigit))
                score += StrengthConstants.DIGIT_SCORE;
            else
                issuesBuilder.Add("Missing digit");

            if (password.Any(ch => !char.IsLetterOrDigit(ch)))
                score += StrengthConstants.SPECIAL_CHAR_SCORE;
            else
                issuesBuilder.Add("Missing special character");

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
                issuesBuilder.ToImmutable()
            );

            return Task.FromResult(result);
        }
    }
}
