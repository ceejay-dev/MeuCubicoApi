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
        [Key] 
        public int APId { get; set; }
        [Required]
        public float Debt { get; set; }
        [Required]
        public float Balance { get; set; }
        public int FkResident { get; set; }
        [ForeignKey(nameof(FkResident))]
        public Resident ? Resident { get; set; }
        public int FkExpense { get; set; }
        [ForeignKey(nameof(FkExpense))]
        public Expense ? Expense { get; set; }
        public int FkActivity { get; set; }
        [ForeignKey(nameof(FkActivity))]
        public Activity ? Activity { get; set; }
    }
}
