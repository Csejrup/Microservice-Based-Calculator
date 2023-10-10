using HistoryService.DbContext;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<HistoryContext>();

builder.Services.AddOpenTelemetry().WithTracing((b) => b
    .AddAspNetCoreInstrumentation()
    .AddZipkinExporter(options =>
    {
        options.Endpoint = new Uri("http://localhost:9411/api/v2/spans");
    }));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.Run();