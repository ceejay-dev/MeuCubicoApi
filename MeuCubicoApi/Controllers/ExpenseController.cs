using DTO;
using MeuCubicoApi.Pagination;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shared.IServices;

namespace MeuCubicoApi.Controllers
{
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService service;

        public ExpenseController(IExpenseService expenseService)
        {
             service = expenseService;
        }

        [HttpPost("Expenses")]
        public async Task<ActionResult<ExpenseDTO>> CreateExpense(ExpenseDTO dto)
        {
            try
            {
                if (service == null)
                {
                    return StatusCode(500, "Service is not initialized.");
                }

                var expenseAdded = await service.CreateExpense(dto);
                return Ok(dto);
            }
            catch (Exception ex) {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("Expense/{id}")]
        public async Task<ActionResult<ExpenseDTO>> GetExpenseById(int id)
        {
            try
            {
                if (service == null)
                {
                    return StatusCode(500, "Service is not initialized.");
                }

                var dto = await service.GetExpenseById(id);

                if (dto == null)
                {
                    return NotFound("Expense not found");
                }
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("Expenses/pagination")]
        public async Task<ActionResult<IEnumerable<ExpenseDTO>>> GetAllExpenses([FromQuery]ExpenseParameters expenseParameters)
        {
            var expenses = await service.GetAllExpenses(expenseParameters);

            return Ok(expenses);
        }

    }
}
