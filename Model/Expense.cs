using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Table("Expenses")]
    public class Expense
    {
        [Key]  
        public int ExpenseId { get; set; }
        [Required]
        public string ? Description { get; set; }
        [Required]
        public float Price { get; set; }
    }
}
