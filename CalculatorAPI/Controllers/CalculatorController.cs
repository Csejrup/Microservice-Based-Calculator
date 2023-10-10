using CalculatorAPI.Aggregates;
using CalculatorAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CalculatorAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class CalculatorController : ControllerBase
    {
        private readonly CalculatorService _calculatorService;

        public CalculatorController(CalculatorService calculatorService)
        {
            _calculatorService = calculatorService ?? throw new ArgumentNullException(nameof(calculatorService));
        }

        // Full route: /api/calculator/add/{a}/{b}
        [HttpGet("add/{a:double}/{b:double}")]
        public async Task<ActionResult<double>> Add(double a, double b)
        {
            try
            {
                Log.Information($"Adding {a} and {b}");

                if (double.IsInfinity(a) || double.IsInfinity(b))
                {
                    Log.Warning("Invalid input: Values cannot be infinity.");
                    return BadRequest("Values cannot be infinity.");
                }

                var result = await _calculatorService.Add(a, b);
                await _calculatorService.AddToHistory($"{a} + {b} = {result}");
                Log.Information($"Result: {result}");
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while adding numbers");
                return BadRequest(new { message = "An error occurred while adding numbers", details = ex.Message });
            }
        }

        // Full route: /api/calculator/subtract/{a}/{b}
        [HttpGet("subtract/{a:double}/{b:double}")]
        public async Task<ActionResult<double>> Subtract(double a, double b)
        {
            var url = $"http://subtraction-service:<port>/subtract/{a}/{b}";
            Log.Information($"Calling URL: {url}");
            try
            {
                Log.Information($"Subtracting {b} from {a}");

                if (double.IsInfinity(a) || double.IsInfinity(b))
                {
                    Log.Warning("Invalid input: Values cannot be infinity.");
                    return BadRequest("Values cannot be infinity.");
                }

                var result = await _calculatorService.Subtract(a, b);
                await _calculatorService.AddToHistory($"{a} - {b} = {result}");
                Log.Information($"Result: {result}");
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while subtracting numbers");
                return BadRequest(new { message = "An error occurred while subtracting numbers", details = ex.Message });
            }
        }

        // Full route: /api/calculator/history
        [HttpGet("history")]
        public async Task<ActionResult<IEnumerable<CalculationHistory>>> GetHistory()
        {
            try
            {
                Log.Information("Fetching calculation history");

                var history = await _calculatorService.GetCalculationHistory();

                if (history == null || !history.Any())
                {
                    Log.Warning("No history records found.");
                    return NotFound("No history records found.");
                }

                Log.Information($"Fetched {history.Count()} records from the calculation history");
                return Ok(history);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while fetching the calculation history");
                return BadRequest(new { message = "An error occurred while fetching the history", details = ex.Message });
            }
        }
    }
}
