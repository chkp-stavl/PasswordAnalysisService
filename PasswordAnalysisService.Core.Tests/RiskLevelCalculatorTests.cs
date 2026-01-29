using Domain.Constants;
using Domain.Enums;
using PasswordAnalysisService.Core.Risk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordAnalysisService.Core.Tests
{
    public class RiskLevelCalculatorTests
    {
        private readonly RiskLevelCalculator _sut = new();

        [Fact]
        public void Calculate_WhenScoreIsCritical_ReturnsCritical()
        {
            var level = _sut.Calculate(RiskConstants.CRITICAL_THRESHOLD);

            Assert.Equal(RiskLevel.Critical, level);
        }

        [Fact]
        public void Calculate_WhenScoreIsLow_ReturnsLow()
        {
            var level = _sut.Calculate(0);

            Assert.Equal(RiskLevel.Low, level);
        }
    }
}
