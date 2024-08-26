using DTO;
using MeuCubicoApi.Pagination;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.IServices
{
    public interface IExpenseService
    {
        Task<ExpenseDTO> CreateExpense(ExpenseDTO expense);
        Task<ExpenseDTO> GetExpenseById(int id);
        Task<IEnumerable<ExpenseDTO>> GetAllExpenses(ExpenseParameters expenses);
    }
}
