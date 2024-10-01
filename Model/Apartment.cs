using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Table("Apartments")]
    public class Apartment
    {
        public int Id { get; set; }
        [Required]
        public float Debt { get; set; }
        [Required]
        public float Balance { get; set; }
        public Guid UserFk { get; set; }
        public User ? User { get; set; }
        public int ExpenseId { get; set; }
        public Expense ? Expense { get; set; }
        public int ActivityId { get; set; }
        public Activity ? Activity { get; set; }
    }
}
