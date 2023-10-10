using HistoryService.Aggregates;
using HistoryService.DbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HistoryService.Controllers
{
    [ApiController]
    [Route("history")] 
    public class HistoryController : ControllerBase
    {
        private readonly HistoryContext _context;

        public HistoryController(HistoryContext context)
        {
            _context = context;
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddToHistory([FromBody] string operation)
        {
            var history = new CalculationHistory
            {
                Operation = operation,
                Timestamp = DateTime.UtcNow
            };

            _context.Histories.Add(history);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<CalculationHistory>>> GetHistory()
        {
            var histories = await _context.Histories.ToListAsync();
            return Ok(histories);
        }
        
    }

}