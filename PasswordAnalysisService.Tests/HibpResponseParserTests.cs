using PasswordAnalysisService.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordAnalysisService.Tests
{
    public class HibpResponseParserTests
    {
        private readonly HibpResponseParser parser = new();

        [Fact]
        public void FindBreachCount_WhenSuffixExists_ReturnsCount()
        {
            var response = "ABCDEF:10\n123456:50";

            var result = parser.FindBreachCount(response, "ABCDEF");

            Assert.Equal(10, result);
        }

        [Fact]
        public void FindBreachCount_WhenSuffixDoesNotExist_ReturnsNull()
        {
            var response = "ABCDEF:10";

            var result = parser.FindBreachCount(response, "NOTFOUND");

            Assert.Null(result);
        }

        [Fact]
        public void FindBreachCount_IgnoresInvalidLines_ReturnsCount()
        {
            var response = "INVALID_LINE\nABCDEF:20";

            var result = parser.FindBreachCount(response, "ABCDEF");

            Assert.Equal(20, result);
        }

        [Fact]
        public void FindBreachCount_IsCaseInsensitive_ReturnsCount()
        {
            var response = "abcdef:15";

            var result = parser.FindBreachCount(response, "ABCDEF");

            Assert.Equal(15, result);
        }
    }

}
