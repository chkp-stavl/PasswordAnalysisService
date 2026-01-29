using Core.Services;
using Domain.Constants;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordAnalysisService.Core.Tests
{
    public class StrengthCheckerTests
    {
        private readonly StrengthChecker _sut = new();

        [Fact]
        public void CheckAsync_WhenPasswordIsStrong_ReturnsStrongResult()
        {
            var password = "StrongPass123!";

            var result = _sut.Check(password, CancellationToken.None);

            Assert.Equal(100, result.Score);
            Assert.Equal(PasswordStrengthLevel.Strong, result.Level);
            Assert.Empty(result.Issues);
        }

        [Fact]
        public void CheckAsync_WhenPasswordIsVeryWeak_ReturnsWeakResultWithIssues()
        {
            var password = "abc";

            var result = _sut.Check(password, CancellationToken.None);

            Assert.True(result.Score < StrengthConstants.WEAK_THRESHOLD);
            Assert.Equal(PasswordStrengthLevel.Weak, result.Level);

            Assert.Contains("Password is too short", result.Issues);
            Assert.Contains("Missing uppercase letter", result.Issues);
            Assert.Contains("Missing digit", result.Issues);
            Assert.Contains("Missing special character", result.Issues);
        }

        [Fact]
        public void CheckAsync_WhenScoreIs80OrAbove_ReturnsStrong()
        {
            var password = "Abcdef1!"; // score = 100

            var result = _sut.Check(password, CancellationToken.None);

            Assert.Equal(PasswordStrengthLevel.Strong, result.Level);
        }

        [Fact]
        public void CheckAsync_WhenScoreIsBetween50And79_ReturnsMedium()
        {
            var password = "Abcdef12"; // score = 70

            var result = _sut.Check(password, CancellationToken.None);

            Assert.Equal(PasswordStrengthLevel.Medium, result.Level);
        }

        [Fact]
        public void CheckAsync_WhenScoreIsBelow50_ReturnsWeak()
        {
            var password = "abc"; // very weak

            var result = _sut.Check(password, CancellationToken.None);

            Assert.Equal(PasswordStrengthLevel.Weak, result.Level);
        }

        [Fact]
        public void CheckAsync_WhenMissingUppercase_AddsUppercaseIssue()
        {
            var password = "lowercase123!";

            var result = _sut.Check(password, CancellationToken.None);

            Assert.Contains("Missing uppercase letter", result.Issues);
            Assert.DoesNotContain("Missing lowercase letter", result.Issues);
        }
    }
}
