using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeuCubico.DTO
{
    public class ExpenseUpdateDTO
    {
        public int ExpenseId { get; set; }
        public string? Description { get; set; }
        public float Price { get; set; }
    }
}
