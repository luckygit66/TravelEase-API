using TravelEaseApi.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/travelease-log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// ✅ Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register GPT Parser Service
builder.Services.AddSingleton<GPTParserService>();
builder.Services.AddSingleton<PricePredictionService>();
builder.Services.AddHttpClient<FlightSearchService>();
builder.Services.AddScoped<FlightAggregatorService>();



var app = builder.Build();

// Use CORS **before** Authorization
app.UseCors("AllowReactApp");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// 👇 This enables your API controllers like FlightSearchController
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
