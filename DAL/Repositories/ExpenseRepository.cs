using Microsoft.EntityFrameworkCore;
using Model;
using Shared.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly MeuCubicoDbContext dbContext;

        public ExpenseRepository (MeuCubicoDbContext meuCubicoDbContext)
        {
            dbContext = meuCubicoDbContext;
        }

        public async Task<Expense> CreateExpense (Expense expense)
        {
            var expe = await dbContext.AddAsync(expense);
            await dbContext.SaveChangesAsync();
            return expe.Entity;
        }

        public async Task<Expense> GetExpenseById (int id)
        {
            var expense = await dbContext.Expenses.FirstOrDefaultAsync(e => e.ExpenseId == id);

            return expense;            
        }
    }
}
