using FinanceApp.Controllers;
using FinanceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Data.Service
{
    public class ExpenseService : IExpensesService
    {
        private readonly FinanceAppContext _context;

        public ExpenseService(FinanceAppContext context)
        {
            _context = context;
        }
        
        public async Task Add(Expense expense)
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

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
        }
        
        public async Task<IEnumerable<Expense>> GetAll()
        {
            var expenses = await _context.Expenses
                .Include(e => e.User) 
                .OrderByDescending(e => e.Date)
                .ToListAsync();
            return expenses;
        }

        public async Task<IEnumerable<Expense>> GetByUserId(int userId)
        {
            return await _context.Expenses
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.Date)
                .ToListAsync();
        }

        public IQueryable GetChartData(int userId)
        {
            return _context.Expenses
                .Where(e => e.UserId == userId)
                .GroupBy(e => e.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    TotalAmount = g.Sum(e => e.Amount)
                });
        }
    }
}