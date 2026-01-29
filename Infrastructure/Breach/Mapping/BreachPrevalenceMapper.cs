using Domain.Constants;
using Domain.Enums;
using PasswordAnalysisService.Infrastructure.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordAnalysisService.Infrastructure.Breach.Mapping
{
    public sealed class BreachPrevalenceMapper : IBreachPrevalenceMapper
    {
        public BreachPrevalence Map(int? count)
        {
            if (count is null or 0)
                return BreachPrevalence.Unknown;

            return count switch
            {
                >= BreachThresholds.HIGH => BreachPrevalence.High,
                >= BreachThresholds.MEDIUM => BreachPrevalence.Medium,
                _ => BreachPrevalence.Low
            };

        }
    }
}
