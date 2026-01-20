using Domain.Constants;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using System.Collections.Immutable;

namespace Infrastructure.Risk
{
    public class RiskAssessmentService : IRiskAssessmentService
    {
        public RiskResult Assess(StrengthResult strength, BreachResult breach)
        {
            var reasons = ImmutableArray.CreateBuilder<string>();
            int score = 0;

            var breachScore = CalculateBreachScore(breach);
            if (breachScore > 0)
            {
                score += breachScore;
                reasons.Add("Password was found in known data breaches");
            }

            // 2. Strength contribution
            var strengthPenalty = CalculateStrengthPenalty(strength);
            if (strengthPenalty > 0)
            {
                score += strengthPenalty;

                var strengthReason = GetStrengthReason(strength.Level);
                if (strengthReason != null)
                {
                    reasons.Add(strengthReason);
                }
            }
            score = Math.Clamp(score, 0, RiskConstants.MAX_SCORE);
            var level = DetermineRiskLevel(score);

            if (reasons.Count == 0)
            {
                reasons.Add("No significant risk factors detected");
            }

            return new RiskResult(
                level,
                score,
                reasons.ToImmutable()
            );
        }


        private static int CalculateBreachScore(BreachResult breach)
        {
            int score = 0;

            if (breach.IsBreached)
                score += RiskConstants.BREACH_FOUND_SCORE;

            if (breach.Sources.Any(s => s.Prevalence == BreachPrevalence.High))
                score += RiskConstants.HIGH_PREVALENCE_BONUS;

            return score;
        }

        private static int CalculateStrengthPenalty(StrengthResult strength)
        {
            return strength.Level switch
            {
                PasswordStrengthLevel.VeryWeak => RiskConstants.VERY_WEAK_PENALTY,
                PasswordStrengthLevel.Weak => RiskConstants.WEAK_PENALTY,
                PasswordStrengthLevel.Medium => RiskConstants.MEDIUM_PENALTY,
                _ => 0
            };
        }

        private static string? GetStrengthReason(PasswordStrengthLevel level)
        {
            return level switch
            {
                PasswordStrengthLevel.VeryWeak => "Password is extremely weak",
                PasswordStrengthLevel.Weak => "Password is weak",
                PasswordStrengthLevel.Medium => "Password strength is moderate",
                _ => null
            };
        }

        private static RiskLevel DetermineRiskLevel(int score)
        {
            return score switch
            {
                >= RiskConstants.CRITICAL_THRESHOLD => RiskLevel.Critical,
                >= RiskConstants.HIGH_THRESHOLD => RiskLevel.High,
                >= RiskConstants.RISK_MEDIUM_THRESHOLD => RiskLevel.Medium,
                _ => RiskLevel.Low
            };
        }
    }
}
