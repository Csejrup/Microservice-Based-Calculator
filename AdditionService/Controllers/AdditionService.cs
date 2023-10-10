using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Trace;
using Serilog;
using System.Diagnostics;

namespace AdditionService.Controllers
{
    [ApiController]
    [Route("add")]
    public class AdditionController : ControllerBase
    {
        private readonly Tracer tracer;

        public AdditionController(TracerProvider tracerProvider)
        {
            tracer = tracerProvider.GetTracer("AdditionService");
        }

        [HttpGet("{a:double}/{b:double}")]
        public IActionResult Add(double a, double b)
        {
            var span = tracer.StartSpan("Add");
            
            try
            {
                span.SetAttribute("operand1", a);
                span.SetAttribute("operand2", b);

                var result = a + b;

                span.SetAttribute("result", result);
                Log.Information($"Added {a} and {b}: {result}");

                span.End();

                return Ok(result);
            }
            catch (Exception ex)
            {
                span.RecordException(ex);
                span.End();

                Log.Error(ex, "Error occurred while adding numbers");
                return BadRequest(new { message = "Error occurred while adding numbers", details = ex.Message });
            }
        }
    }
}