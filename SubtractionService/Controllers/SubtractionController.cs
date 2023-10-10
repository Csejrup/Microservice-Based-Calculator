using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Trace;
using Serilog;

namespace SubtractionService.Controllers;

[ApiController]
[Route("sub")]
public class SubtractionController : ControllerBase
{
    private readonly Tracer tracer;

    public SubtractionController(TracerProvider tracerProvider)
    {
        tracer = tracerProvider.GetTracer("SubtractionService");
    }

    [HttpGet("{a:double}/{b:double}")]
    public IActionResult Subtract(double a, double b)
    {
        var span = tracer.StartSpan("Subtract");
            
        try
        {
            span.SetAttribute("operand1", a);
            span.SetAttribute("operand2", b);

            var result = a - b;

            span.SetAttribute("result", result);
            Log.Information($"Subtracted {a} and {b}: {result}");

            span.End();

            return Ok(result);
        }
        catch (Exception ex)
        {
            span.RecordException(ex);
            span.End();

            Log.Error(ex, "Error occurred while subtracting numbers");
            return BadRequest(new { message = "Error occurred while subtracting numbers", details = ex.Message });
        }
    }
}

