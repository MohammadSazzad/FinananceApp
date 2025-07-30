using FinanceApp.Data;
using FinanceApp.Data.Service;
using FinanceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly IExpensesService _expensesService;
        public ExpensesController(IExpensesService expensesService)
        {
            _expensesService = expensesService;
        }

        public async Task<IActionResult> Index()
        {
            var expenses = await _expensesService.GetAll();
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
            var data = _expensesService.GetChartData();
            return Json(data);
        }

    }
}