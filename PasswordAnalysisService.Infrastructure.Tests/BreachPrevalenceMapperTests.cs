using Domain.Constants;
using Domain.Enums;
using PasswordAnalysisService.Infrastructure.Breach.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordAnalysisService.Infrastructure.Tests
{
    public class BreachPrevalenceMapperTests
    {
        private readonly BreachPrevalenceMapper _sut = new();


        [Fact]
        public void Map_WhenCountIsNull_ReturnsUnknown()
        {
            var result = _sut.Map(null);
            Assert.Equal(BreachPrevalence.Unknown, result);
        }

        [Fact]
        public void Map_WhenCountIsZero_ReturnsUnknown()
        {
            var result = _sut.Map(0);
            Assert.Equal(BreachPrevalence.Unknown, result);
        }

        [Fact]
        public void Map_WhenCountIsBelowMediumThreshold_ReturnsLow()
        {
            var result = _sut.Map(BreachThresholds.MEDIUM - 1);
            Assert.Equal(BreachPrevalence.Low, result);
        }

        [Fact]
        public void Map_WhenCountIsMediumThreshold_ReturnsMedium()
        {
            var result = _sut.Map(BreachThresholds.MEDIUM);
            Assert.Equal(BreachPrevalence.Medium, result);
        }

        [Fact]
        public void Map_WhenCountIsHighThreshold_ReturnsHigh()
        {
            var result = _sut.Map(BreachThresholds.HIGH);
            Assert.Equal(BreachPrevalence.High, result);
        }
    }
}
