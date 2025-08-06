using FinanceApp.Models;

namespace FinanceApp.Data.Service
{
    public interface IExpensesService
    {
        Task<IEnumerable<Expense>> GetAll();
        Task<IEnumerable<Expense>> GetByUserId(int userId);
        Task Add(Expense expense);
        IQueryable GetChartData(int userId);
    }
}