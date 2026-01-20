using PasswordAnalysisService.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PasswordAnalysisService.Consts;

namespace PasswordAnalysisService.Tests
{
    public class BreachPrevalenceMapperTests
    {
        private readonly BreachPrevalenceMapper mapper = new();
      

        [Theory]
        [InlineData(null, BreachPrevalence.Unknown)]
        [InlineData(0, BreachPrevalence.Unknown)]
        [InlineData(1, BreachPrevalence.Low)]
        [InlineData(MEDIUM_THRESHOLD_BREACH - 1, BreachPrevalence.Low)]
        [InlineData(MEDIUM_THRESHOLD_BREACH, BreachPrevalence.Medium)]
        [InlineData(HIGH_THRESHOLD_BREACH - 1, BreachPrevalence.Medium)]
        [InlineData(HIGH_THRESHOLD_BREACH, BreachPrevalence.High)]
        public void Map_ReturnsExpectedPrevalence(
            int? count,
            BreachPrevalence expected)
        {
            var result = mapper.Map(count);

            Assert.Equal(expected, result);
        }
    }

}
