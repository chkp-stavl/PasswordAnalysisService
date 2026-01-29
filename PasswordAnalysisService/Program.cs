using FluentValidation;
using PasswordAnalysisService.Api.Validation;
using Core.Abstractions;
using Core.Services;
using PasswordAnalysisService.Core.Risk;
using Domain.Interfaces;
using Infrastructure.Breach.Hibp;
using Infrastructure.Security;
using PasswordAnalysisService.Infrastructure.Abstractions;
using PasswordAnalysisService.Infrastructure.Breach.Hibp;
using PasswordAnalysisService.Infrastructure.Breach.Mapping;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Controllers & Swagger
builder.Services
    .AddControllers()
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssemblyContaining<AnalyzePasswordRequestValidator>();
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Clean Architecture layers


builder.Services.AddScoped<IMain, Main>();
builder.Services.AddScoped<IStrengthChecker, StrengthChecker>();
builder.Services.AddScoped<IBreachChecker, BreachChecker>();
builder.Services.AddScoped<IRiskAssessmentService, RiskAssessmentService>();

builder.Services.AddScoped<IPasswordHasher, Sha1PasswordHasher>();
builder.Services.AddScoped<IHibpResponseParser, HibpResponseParser>();
builder.Services.AddScoped<IBreachPrevalenceMapper, BreachPrevalenceMapper>();
builder.Services.AddScoped<IBreachSource, HibpBreachSource>();

builder.Services.AddHttpClient<IHibpClient, HibpClient>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(5);
    client.DefaultRequestHeaders.UserAgent.ParseAdd(
        "PasswordAnalysisService/1.0");
});

builder.Services.AddScoped<BreachRiskEvaluator>();

builder.Services.AddScoped<StrengthRiskEvaluator>();

builder.Services.AddScoped<RiskLevelCalculator>();




builder.Services.AddValidatorsFromAssemblyContaining<AnalyzePasswordRequestValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
