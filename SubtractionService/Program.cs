
using Oakton;
using Serilog;
using SubtractionService;

public abstract class Program
{
    public static Task<int> Main(string[] args)
    {
        return CreateHostBuilder(args)
            .RunOaktonCommands(args);
    }
    
    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog((hostingContext, loggerConfiguration) => 
            {
                loggerConfiguration
                    .ReadFrom.Configuration(hostingContext.Configuration)
                    .Enrich.FromLogContext()
                    .WriteTo.Seq("http://localhost:5341"); 
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });


}