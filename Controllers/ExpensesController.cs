using FinanceApp.Data;
using FinanceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly FinanceAppContext _context;
        public ExpensesController(FinanceAppContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var expenses = await _context.Expenses.OrderByDescending(e => e.Date).ToListAsync();
            return View(expenses);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Expense expense)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Ensure the date is set properly - use UTC to avoid timezone issues
                    if (expense.Date == default || expense.Date == DateTime.MinValue)
                    {
                        expense.Date = DateTime.UtcNow;
                    }
                    else
                    {
                        // Convert to UTC if it's not already
                        expense.Date = expense.Date.Kind == DateTimeKind.Unspecified 
                            ? DateTime.SpecifyKind(expense.Date, DateTimeKind.Utc)
                            : expense.Date.ToUniversalTime();
                    }
                    
                    _context.Expenses.Add(expense);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Log the full exception details to console
                    Console.WriteLine($"Error saving expense: {ex}");
                    
                    // Get the inner exception details
                    var innerMessage = ex.InnerException?.Message ?? "No inner exception";
                    var detailMessage = $"Error: {ex.Message}. Inner: {innerMessage}";
                    
                    ModelState.AddModelError("", detailMessage);
                }
            }
            
            // If we got this far, something failed, redisplay form
            return View(expense);
        }

        // Action to seed sample data
        public async Task<IActionResult> SeedData()
        {
            // Check if we already have data
            if (await _context.Expenses.AnyAsync())
            {
                return RedirectToAction("Index");
            }

            // Add sample expenses
            var sampleExpenses = new List<Expense>
            {
                new Expense
                {
                    Description = "Grocery Shopping",
                    Amount = 85.50m,
                    Category = "Food",
                    Date = DateTime.Now.AddDays(-2)
                },
                new Expense
                {
                    Description = "Gas Station",
                    Amount = 45.00m,
                    Category = "Transportation",
                    Date = DateTime.Now.AddDays(-1)
                },
                new Expense
                {
                    Description = "Coffee Shop",
                    Amount = 12.75m,
                    Category = "Food",
                    Date = DateTime.Now
                },
                new Expense
                {
                    Description = "Internet Bill",
                    Amount = 65.00m,
                    Category = "Utilities",
                    Date = DateTime.Now.AddDays(-5)
                }
            };

            _context.Expenses.AddRange(sampleExpenses);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}