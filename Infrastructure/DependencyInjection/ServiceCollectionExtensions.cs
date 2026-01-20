using Domain.Interfaces;
using Infrastructure.Breach;
using Infrastructure.Security;
using Infrastructure.Strength;
using Infrastructure.Risk;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IStrengthChecker, StrengthChecker>();
        services.AddScoped<IBreachChecker, BreachChecker>();
        services.AddScoped<IRiskAssessmentService, RiskAssessmentService>();

        services.AddScoped<IPasswordHasher, Sha1PasswordHasher>();
        services.AddScoped<IHibpResponseParser, HibpResponseParser>();
        services.AddScoped<IBreachPrevalenceMapper, BreachPrevalenceMapper>();
        services.AddScoped<IBreachSource, HibpBreachSource>();

        services.AddHttpClient<IHibpClient, HibpClient>(client =>
        {
            client.Timeout = TimeSpan.FromSeconds(5);
            client.DefaultRequestHeaders.UserAgent.ParseAdd(
                "PasswordAnalysisService/1.0");
        });

        return services;
    }
}
