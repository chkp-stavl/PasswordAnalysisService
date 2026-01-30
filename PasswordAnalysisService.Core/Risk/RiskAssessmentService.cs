using Domain.Constants;
using Domain.Enums;
using Domain.Models;

namespace PasswordAnalysisService.Core.Risk
{
    public class BreachRiskEvaluator
    {
        public int Evaluate(BreachResult breach, List<string> reasons)
        {
            int score = 0;

            if (breach.IsBreached)
            {
                score += RiskConstants.BREACH_FOUND_SCORE;
                reasons.Add("Password was found in known data breaches");
            }

            if (breach.Sources.Any(s => s.Prevalence == BreachPrevalence.High))
            {
                score += RiskConstants.HIGH_PREVALENCE_BONUS;
            }

            return score;
        }
    }

}
