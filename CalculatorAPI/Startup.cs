using CalculatorAPI.Services;
using OpenTelemetry.Trace;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Resources;

namespace CalculatorAPI;

public class Startup
{
    private IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddOpenTelemetry().ConfigureResource(otelBuilder => otelBuilder
            .AddService(serviceName: "Calculator Microservices")).WithTracing((builder) => builder
            .AddAspNetCoreInstrumentation()
            .AddConsoleExporter()
            .AddHttpClientInstrumentation()
            .AddZipkinExporter(options =>
            {
                options.Endpoint = new Uri("http://127.0.0.1:9411/api/v2/spans");
            }));

        services.AddHttpClient<CalculatorService>();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "CalculatorAPI API", Version = "v1" });
        });
        services.AddHealthChecks();
        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();

        app.UseAuthorization();

        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CalculatorAPI API v1"));

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks("/health");
            endpoints.MapControllers();
        });
    }
}