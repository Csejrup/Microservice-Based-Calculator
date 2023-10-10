using Microsoft.AspNetCore.Mvc;

namespace AdditionService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdditionController : ControllerBase
    {
        [HttpGet("{a:double}/{b:double}")]
        public double Add(double a, double b)
        {
            return a + b;
        }
    }
}