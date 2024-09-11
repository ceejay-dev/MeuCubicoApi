using MeuCubicoApi.Pagination;
using Microsoft.EntityFrameworkCore;
using Model;
using Pagination;
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


        public async Task<PagedList<Expense>> GetAllExpenses(ExpenseParameters expensesParameters)
        {
            var expenses = await dbContext.Expenses
                .OrderBy(e => e.ExpenseId)
                .ToListAsync();

            var expensesOrdered = PagedList<Expense>.ToPagedList(expenses.AsQueryable(), expensesParameters.PageNumber, expensesParameters.PageSize);
            // converting the data collection to an IQueryable because PagedList wait this datatype
            return expensesOrdered;
        }

        public async Task

        //public async Task<IEnumerable<Expense>> GetAllExpenses(ExpenseParameters expenses)
        //{
        //    return await dbContext.Expenses
        //        .OrderBy(e => e.ExpenseId)
        //        .Skip((expenses.PageNumber-1) * expenses.PageSize)
        //        .Take(expenses.PageSize)
        //        .ToListAsync();
        //}
    }
}
