

using PasswordAnalysisService.Logic;
using PasswordAnalysisService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPasswordAnalysisService, PasswordAnalysisService.Services.PasswordAnalysisService>();
builder.Services.AddScoped<IStrengthChecker, StrengthChecker>();
builder.Services.AddScoped<IRiskAssessmentService, RiskAssessmentService>();
/*builder.Services.AddHttpClient<HibpBreachSource>();*/
builder.Services.AddHttpClient<HibpBreachSource>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(5);
    client.DefaultRequestHeaders.UserAgent.ParseAdd(
        "PasswordAnalysisService/1.0"
    );
});

builder.Services.AddScoped<IBreachSource, HibpBreachSource>();
builder.Services.AddScoped<IBreachChecker, BreachChecker>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();
app.Run();
