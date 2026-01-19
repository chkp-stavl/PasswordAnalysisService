
using System.Collections.Immutable;
using System.Reflection.Emit;
using static PasswordAnalysisService.Consts;

namespace PasswordAnalysisService.Logic
{
    public class StrengthChecker : IStrengthChecker
    {
        public Task<StrengthResult> CheckAsync(string password, CancellationToken ct)
        {
            var issuesBuilder = ImmutableArray.CreateBuilder<string>();
            int score = 0;

            if (password.Length >= MIN_PASS_LEN)
                score += LENGTH_SCORE;
            else
                issuesBuilder.Add("Password is too short");

            if (password.Any(char.IsUpper))
                score += UPPERCASE_SCORE;
            else
                issuesBuilder.Add("Missing uppercase letter");

            if (password.Any(char.IsLower))
                score += LOWERCASE_SCORE;
            else
                issuesBuilder.Add("Missing lowercase letter");

            if (password.Any(char.IsDigit))
                score += DIGIT_SCORE;
            else
                issuesBuilder.Add("Missing digit");

            if (password.Any(ch => !char.IsLetterOrDigit(ch)))
                score += SPECIAL_CHAR_SCORE;
            else
                issuesBuilder.Add("Missing special character");

            score = Math.Min(score, MAX_SCORE);
            var level = score switch
            {
                < WEAK_THRESHOLD => PasswordStrengthLevel.Weak,
                < STRENGTH_MEDIUM_THRESHOLD => PasswordStrengthLevel.Medium,
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
