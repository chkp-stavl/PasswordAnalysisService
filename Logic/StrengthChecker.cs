
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

            if (password.Length >= 8)
                score += 25;
            else
                issuesBuilder.Add("Password is too short");

            if (password.Any(char.IsUpper))
                score += 15;
            else
                issuesBuilder.Add("Missing uppercase letter");

            if (password.Any(char.IsLower))
                score += 15;
            else
                issuesBuilder.Add("Missing lowercase letter");

            if (password.Any(char.IsDigit))
                score += 15;
            else
                issuesBuilder.Add("Missing digit");

            if (password.Any(ch => !char.IsLetterOrDigit(ch)))
                score += 30;
            else
                issuesBuilder.Add("Missing special character");

            score = Math.Min(score, 100);
            var level = score switch
            {
                < 50 => PasswordStrengthLevel.Weak,
                < 80 => PasswordStrengthLevel.Medium,
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
