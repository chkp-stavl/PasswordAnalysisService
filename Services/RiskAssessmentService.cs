using PasswordAnalysisService.Logic;
using static PasswordAnalysisService.Consts;
using System.Collections.Immutable;

namespace PasswordAnalysisService.Services
{
    public class RiskAssessmentService : IRiskAssessmentService
    {
        public RiskResult Assess(StrengthResult strength, BreachResult breach)
        {
            var reasons = ImmutableArray.CreateBuilder<string>();
            int riskScore = 0;

            
            if (breach.IsBreached)
            {
                int breachScore = breach.BreachCount switch
                {
                    >= 10 => 70,
                    >= 5 => 60,
                    >= 1 => 40,
                    _ => 30
                };

                riskScore += breachScore;
                reasons.Add("Password was found in known data breaches");
            }

            int strengthPenalty = strength.Level switch
            {
                PasswordStrengthLevel.VeryWeak => 40,
                PasswordStrengthLevel.Weak => 30,
                PasswordStrengthLevel.Medium => 15,
                PasswordStrengthLevel.Strong => 0,
                PasswordStrengthLevel.VeryStrong => 0,
                _ => 0
            };

            if (strengthPenalty > 0)
            {
                riskScore += strengthPenalty;
                reasons.Add($"Password strength is {strength.Level}");
            }

            riskScore = Math.Clamp(riskScore, 0, 100);

            var level = riskScore switch
            {
                >= 80 => RiskLevel.Critical,
                >= 60 => RiskLevel.High,
                >= 30 => RiskLevel.Medium,
                _ => RiskLevel.Low
            };

            if (reasons.Count == 0)
            {
                reasons.Add("No significant risk factors detected");
            }

            return new RiskResult(
                level,
                riskScore,
                reasons.ToImmutable()
            );
        }
    }
}
