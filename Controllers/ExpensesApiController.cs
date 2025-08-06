using FinanceApp.Data.Service;
using FinanceApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Controllers
{
    [ApiController]
    [Route("api/expenses")]
    [Produces("application/json")]
    public class ExpensesApiController : ControllerBase
    {
        private readonly IExpensesService _expensesService;

        public ExpensesApiController(IExpensesService expensesService)
        {
            _expensesService = expensesService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Expense>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpenses(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID.");
            }
            try
            {
                var expenses = await _expensesService.GetByUserId(userId);
                return Ok(expenses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Expense), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Expense>> GetExpense(int id)
        {
            try
            {
                var expenses = await _expensesService.GetAll();
                var expense = expenses.FirstOrDefault(e => e.Id == id);
                
                if (expense == null)
                {
                    return NotFound($"Expense with ID {id} not found.");
                }
                
                return Ok(expense);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(Expense), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Expense>> CreateExpense([FromBody] Expense expense)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (expense.Date == default || expense.Date == DateTime.MinValue)
                {
                    expense.Date = DateTime.UtcNow;
                }
                else
                {
                    expense.Date = expense.Date.Kind == DateTimeKind.Unspecified
                        ? DateTime.SpecifyKind(expense.Date, DateTimeKind.Utc)
                        : expense.Date.ToUniversalTime();
                }

                expense.UserId = 0; 

                await _expensesService.Add(expense);
                return CreatedAtAction(nameof(GetExpense), new { id = expense.Id }, expense);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("chart")]
        [ProducesResponseType(typeof(object), 200)]
        public ActionResult GetChartData(int userId)
        {
            try
            {
                var data = _expensesService.GetChartData(userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("summary")]
        [ProducesResponseType(typeof(object), 200)]
        public async Task<ActionResult> GetSummary()
        {
            try
            {
                var expenses = await _expensesService.GetAll();
                var summary = new
                {
                    TotalExpenses = expenses.Count(),
                    TotalAmount = expenses.Sum(e => e.Amount),
                    AverageAmount = expenses.Any() ? expenses.Average(e => e.Amount) : 0,
                    Categories = expenses.GroupBy(e => e.Category)
                                       .Select(g => new { Category = g.Key, Count = g.Count(), Total = g.Sum(e => e.Amount) })
                                       .ToList()
                };
                
                return Ok(summary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
