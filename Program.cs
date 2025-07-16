using TravelEaseApi.Services;
using TravelEaseApi.Models;
using Serilog;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// ✅ Load .env variables (OPENAI_API_KEY, TRAVELPAYOUTS_API_TOKEN, TRAVELPAYOUTS_MARKER)
DotNetEnv.Env.Load();
var openAIKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
var travelPayoutsToken = Environment.GetEnvironmentVariable("TRAVELPAYOUTS_API_TOKEN");
var travelPayoutsMarker = Environment.GetEnvironmentVariable("TRAVELPAYOUTS_MARKER");

// ✅ Configure Serilog
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/travelease-log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// ✅ Optional: Log loaded keys (dev-only)
Log.Information("OPENAI_API_KEY loaded: {OpenAIKey}", openAIKey);
Log.Information("TRAVELPAYOUTS_API_TOKEN loaded: {TravelPayoutsToken}", travelPayoutsToken);
Log.Information("TRAVELPAYOUTS_MARKER loaded: {TravelPayoutsMarker}", travelPayoutsMarker);

// ✅ Centralized API Settings DI
var apiSettings = new ApiSettings
{
    OpenAIApiKey = openAIKey,
    TravelPayoutsApiToken = travelPayoutsToken,
    TravelPayoutsMarker = travelPayoutsMarker
};
builder.Services.AddSingleton(apiSettings);

// ✅ CORS for React app
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ✅ Register services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<GPTParserService>();
builder.Services.AddSingleton<PricePredictionService>();
builder.Services.AddHttpClient<FlightSearchService>();
builder.Services.AddScoped<FlightAggregatorService>();

// ✅ Build app
var app = builder.Build();

// ✅ Middleware pipeline
app.UseCors("AllowReactApp");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

try
{
    Log.Information("Starting TravelEaseApi");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start correctly");
}
finally
{
    Log.CloseAndFlush();
}
