using Domain.Constants;
using Domain.Enums;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordAnalysisService.Core.Risk
{
    public class StrengthRiskEvaluator
    {
        public int Evaluate(StrengthResult strength, List<string> reasons)
        {
            var penalty = strength.Level switch
            {
                PasswordStrengthLevel.VeryWeak => RiskConstants.VERY_WEAK_PENALTY,
                PasswordStrengthLevel.Weak => RiskConstants.WEAK_PENALTY,
                PasswordStrengthLevel.Medium => RiskConstants.MEDIUM_PENALTY,
                _ => 0
            };

            if (penalty > 0)
            {
                reasons.Add(GetReason(strength.Level));
            }

            return penalty;
        }

        private static string GetReason(PasswordStrengthLevel level)
        {
            return level switch
            {
                PasswordStrengthLevel.VeryWeak => "Password is extremely weak",
                PasswordStrengthLevel.Weak => "Password is weak",
                PasswordStrengthLevel.Medium => "Password strength is moderate",
                _ => string.Empty
            };
        }
    }

}
