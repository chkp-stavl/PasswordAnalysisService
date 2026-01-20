using PasswordAnalysisService.Logic;
using PasswordAnalysisService.Services;
using PasswordAnalysisService.Utilities;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPasswordAnalysisOrchestrator, PasswordAnalysisOrchestrator>();
builder.Services.AddScoped<IStrengthChecker, StrengthChecker>();
builder.Services.AddScoped<IRiskAssessmentService, RiskAssessmentService>();

builder.Services.AddHttpClient<IHibpClient, HibpClient>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(5);
    client.DefaultRequestHeaders.UserAgent.ParseAdd(
        "PasswordAnalysisService/1.0"
    );
});
builder.Services.AddScoped<IPasswordHasher, Sha1PasswordHasher>();
builder.Services.AddScoped<IHibpResponseParser, HibpResponseParser>();
builder.Services.AddScoped<IBreachPrevalenceMapper, BreachPrevalenceMapper>();

builder.Services.AddScoped<IBreachSource, HibpBreachSource>();

builder.Services.AddScoped<IBreachChecker, BreachChecker>();

builder.Services.AddControllers();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();
app.Run();