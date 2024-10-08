﻿using AutoMapper;
using DTO;
using MeuCubico.DTO;
using MeuCubicoApi.Pagination;
using Model;
using Pagination;
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

        public async Task<PagedList<ExpenseDTO>> GetAllExpenses(ExpenseParameters expensesParameters)
        {
            var expenses = await repository.GetAllExpenses(expensesParameters);

            if (expenses != null)
            {
                var dtoList = mapper.Map<IEnumerable<ExpenseDTO>>(expenses);

                // Cria um novo PagedList de DTOs, mantendo as informações de paginação
                var dtoPagedList = new PagedList<ExpenseDTO>(
                    dtoList.ToList(),
                    expenses.TotalCount,
                    expenses.CurrentPage,
                    expenses.PageSize);

                return dtoPagedList;
            }
            else
            {
                throw new Exception("No expenses found.");
            }
        }

        public async Task<bool> DeleteExpense(int id)
        {
           return await repository.DeleteExpense(id);
        }

        public async Task<bool> UpdateExpense(ExpenseUpdateDTO dto)
        {
            var expense = await repository.GetExpenseById(dto.ExpenseId);
            if (expense == null)
                throw new Exception($"Expense with ID {dto.ExpenseId} not found.");

            mapper.Map(dto, expense);

            return await repository.UpdateExpense(expense);
        }

    }
}
