using DTO;
using MeuCubico.DTO;
using MeuCubicoApi.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pagination;
using Shared.IServices;

namespace MeuCubicoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService service;

        public ExpenseController(IExpenseService expenseService)
        {
            service = expenseService;
        }

        [HttpPost]
        public async Task<ActionResult<ExpenseDTO>> CreateExpense(ExpenseDTO dto)
        {
            try
            {
                var expenseAdded = await service.CreateExpense(dto);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseDTO>> GetExpenseById(int id)
        {
            try
            {
                var dto = await service.GetExpenseById(id);

                if (dto == null)
                {
                    return StatusCode(404, $"Expense not found");
                }
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(404, $"Expense not found {ex.Message}");
            }
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<ExpenseDTO>>> GetAllExpenses([FromQuery] ExpenseParameters expenseParameters)
        {
            try
            {
                var expenses = await service.GetAllExpenses(expenseParameters);

                var metadata = new
                {
                    expenses.TotalCount,
                    expenses.PageSize,
                    expenses.CurrentPage,
                    expenses.TotalPages,
                    expenses.HasNext,
                    expenses.HasPreviuos
                };
                Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(expenses);
            }
            catch (Exception ex)
            {

                return StatusCode(404, $"Not found error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteExpense(int id)
        {
            try
            {
                var result = await service.DeleteExpense(id);
                if (result)
                    return NoContent();
                else
                    return NotFound($"Expense with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateExpense( ExpenseUpdateDTO expenseUpdateDTO)
        {
            try
            {
                var result = await service.UpdateExpense(expenseUpdateDTO);
                if (result)
                    return NoContent();
                else
                    return NotFound($"Expense with ID not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
