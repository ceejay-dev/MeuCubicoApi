using AutoMapper;
using DTO;
using Model;
using Shared.IRepositories;
using Shared.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository repository;
        private readonly IMapper mapper;

        public ExpenseService(IExpenseRepository expenseRepository, IMapper mapper)
        {
            repository = expenseRepository;
             this.mapper = mapper;
        }

        public async Task<ExpenseDTO> CreateExpense(ExpenseDTO dto)
        {
            var expense = mapper.Map<Expense>(dto);
            expense = await repository.CreateExpense(expense);

            var result = mapper.Map<ExpenseDTO>(expense);
            return result;
        }

        public async Task<ExpenseDTO> GetExpenseById(int id)
        {
            var expense = await repository.GetExpenseById(id);

            if (expense != null) { 
                var dto = mapper.Map<ExpenseDTO>(expense);
                return dto;
            } else
            {
                throw new Exception();
            }
        }
    }
}
