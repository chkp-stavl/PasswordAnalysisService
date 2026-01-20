using Domain.Constants;
using Domain.Enums;
using Infrastructure.Breach;


namespace PasswordAnalysisService.Tests
{
    public class BreachPrevalenceMapperTests
    {
        private readonly BreachPrevalenceMapper mapper = new();
      

        [Theory]
        [InlineData(null, BreachPrevalence.Unknown)]
        [InlineData(0, BreachPrevalence.Unknown)]
        [InlineData(1, BreachPrevalence.Low)]
        [InlineData(BreachThresholds.MEDIUM - 1, BreachPrevalence.Low)]
        [InlineData(BreachThresholds.MEDIUM, BreachPrevalence.Medium)]
        [InlineData(BreachThresholds.HIGH - 1, BreachPrevalence.Medium)]
        [InlineData(BreachThresholds.HIGH, BreachPrevalence.High)]
        public void Map_ReturnsExpectedPrevalence(
            int? count,
            BreachPrevalence expected)
        {
            var result = mapper.Map(count);

            Assert.Equal(expected, result);
        }
    }

}
