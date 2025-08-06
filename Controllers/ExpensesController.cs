using FinanceApp.Data;
using FinanceApp.Data.Service;
using FinanceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Controllers
{
    public class ExpensesController : BaseController
    {
        private readonly IExpensesService _expensesService;
        public ExpensesController(IExpensesService expensesService)
        {
            _expensesService = expensesService;
        }
        
        public async Task<IActionResult> Index()
        {

            if (!IsUserLoggedIn())
            {
                TempData["ErrorMessage"] = "Please log in to view your expenses.";
                return RedirectToAction("Login", "Users");
            }

            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                TempData["ErrorMessage"] = "Unable to identify current user. Please log in again.";
                return RedirectToAction("Login", "Users");
            }

            var expenses = await _expensesService.GetByUserId(userId.Value);
            ViewBag.UserId = userId.Value; 
            
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
            if (!IsUserLoggedIn())
            {
                TempData["ErrorMessage"] = "Please log in to add expenses.";
                return RedirectToAction("Login", "Users");
            }

            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                TempData["ErrorMessage"] = "Unable to identify current user. Please log in again.";
                return RedirectToAction("Login", "Users");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    expense.UserId = userId.Value;

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

                    await _expensesService.Add(expense);
                    TempData["SuccessMessage"] = "Expense added successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving expense: {ex}");
                    var innerMessage = ex.InnerException?.Message ?? "No inner exception";
                    var detailMessage = $"Error: {ex.Message}. Inner: {innerMessage}";

                    ModelState.AddModelError("", detailMessage);
                }
            }
            return View(expense);
        }

        public IActionResult GetChart()
        {
            if (!IsUserLoggedIn())
            {
                return Unauthorized("Please log in to view chart data.");
            }

            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return BadRequest("Unable to identify current user.");
            }

            var chartData = _expensesService.GetChartData(userId.Value);
            return Json(chartData);
        }

    }
}