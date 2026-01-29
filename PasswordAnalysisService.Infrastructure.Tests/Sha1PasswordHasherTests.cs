using Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordAnalysisService.Infrastructure.Tests
{
    public class Sha1PasswordHasherTests
    {
        [Fact]
        public void Hash_KnownPassword_ReturnsExpectedSha1()
        {
            var hasher = new Sha1PasswordHasher();

            var result = hasher.Hash("password123");

            Assert.Equal(
                "CBFDAC6008F9CAB4083784CBD1874F76618D2A97",
                result);
        }

        [Fact]
        public void Hash_Always_Returns40CharHexString()
        {
            var hasher = new Sha1PasswordHasher();

            var result = hasher.Hash("any-password");

            Assert.Equal(40, result.Length);
        }
    }
}
