namespace PasswordAnalysisService.Tests;

using Domain.Enums;
using Domain.Models;
using Infrastructure.Risk;
using System.Collections.Immutable;


public class RiskAssessmentServiceTests
{
    private readonly RiskAssessmentService service = new();

    [Fact]
    public void Assess_WhenNoRiskFactors_ReturnsLowRiskWithDefaultReason()
    {
        var strength = new StrengthResult(
            Score: 90,
            Level: PasswordStrengthLevel.Strong,
            Issues: ImmutableArray<string>.Empty);

        var breach = new BreachResult(
            IsBreached: false,
            Sources: Array.Empty<BreachSourceResult>());


        var result = service.Assess(strength, breach);


        Assert.Equal(RiskLevel.Low, result.Level);
        Assert.Equal(0, result.Score);
        Assert.Single(result.Reasons);
        Assert.Contains("No significant risk factors detected", result.Reasons);
    }

    [Fact]
    public void Assess_WhenPasswordIsBreached_AddsBreachRisk()
    {
        var strength = new StrengthResult(
            80,
            PasswordStrengthLevel.Strong,
            ImmutableArray<string>.Empty);

        var breach = new BreachResult(
            true,
            new[]
            {
                new BreachSourceResult(
                    Source: "HIBP",
                    IsBreached: true,
                    IsAvailable: true,
                    BreachCount: 100,
                    Prevalence: BreachPrevalence.Low)
            });


        var result = service.Assess(strength, breach);


        Assert.True(result.Score >= 40);
        Assert.Contains(
            "Password was found in known data breaches",
            result.Reasons);
    }

    [Fact]
    public void Assess_WhenPasswordIsWeak_AddsStrengthPenalty()
    {
        var strength = new StrengthResult(
            40,
            PasswordStrengthLevel.Weak,
            ImmutableArray<string>.Empty);

        var breach = new BreachResult(
            false,
            Array.Empty<BreachSourceResult>());


        var result = service.Assess(strength, breach);

        Assert.Equal(30, result.Score);
        Assert.Contains("Password is weak", result.Reasons);
        Assert.Equal(RiskLevel.Medium, result.Level);
    }

    [Fact]
    public void Assess_WhenBreachAndWeakStrength_CombinesRisks()
    {
        var strength = new StrengthResult(
            30,
            PasswordStrengthLevel.Weak,
            ImmutableArray<string>.Empty);

        var breach = new BreachResult(
            true,
            new[]
            {
                new BreachSourceResult(
                    Source: "HIBP",
                    IsBreached: true,
                    IsAvailable: true,
                    BreachCount: 500,
                    Prevalence: BreachPrevalence.Medium)
            });



        var result = service.Assess(strength, breach);


        Assert.Equal(70, result.Score); // 40 (breach) + 30 (weak)
        Assert.Equal(RiskLevel.High, result.Level);
        Assert.Equal(2, result.Reasons.Count);
    }

    [Fact]
    public void Assess_WhenHighPrevalenceBreach_AddsExtraRisk()
    {
        // Arrange
        var strength = new StrengthResult(
            80,
            PasswordStrengthLevel.Strong,
            ImmutableArray<string>.Empty);

        var breach = new BreachResult(
            true,
            new[]
            {
                new BreachSourceResult(
                    Source: "HIBP",
                    IsBreached: true,
                    IsAvailable: true,
                    BreachCount: 5000,
                    Prevalence: BreachPrevalence.High)
            });

        var result = service.Assess(strength, breach);

        // Assert
        Assert.Equal(50, result.Score); // 40 + 10
        Assert.Equal(RiskLevel.Medium, result.Level);
    }
}
