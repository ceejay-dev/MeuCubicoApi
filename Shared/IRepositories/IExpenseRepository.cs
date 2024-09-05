using MeuCubicoApi.Pagination;
using Model;
using Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.IRepositories
{
    public interface IExpenseRepository
    {
        Task<Expense> CreateExpense(Expense expense);
        Task<Expense> GetExpenseById(int id);
        Task<PagedList<Expense>> GetAllExpenses(ExpenseParameters expenses);
    }
}
