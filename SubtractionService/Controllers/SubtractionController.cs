using Microsoft.AspNetCore.Mvc;

namespace SubtractionService.Controllers;

[ApiController]
[Route("[controller]")]
public class SubtractionController : ControllerBase
{
    [HttpGet("{a}/{b}")]
    public double Subtract(double a, double b)
    {
        return a - b;
    }
}