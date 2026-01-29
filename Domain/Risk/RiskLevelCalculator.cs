using Domain.Constants;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordAnalysisService.Core.Risk
{
    public class RiskLevelCalculator
    {
        public RiskLevel Calculate(int score)
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
