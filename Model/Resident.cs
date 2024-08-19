using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Table("Residents")]
    public class Resident
    {
        [Key] 
        public int ResidentId { get; set; }
        [Required]
        public string ? Name { get; set; }
        [Required]
        [StringLength(14)]
        public string ? BI { get; set; }
        [Required]
        public string ? Username { get; set; }
        [Required]
        public string? Password { get; set; }
        public string? Photo { get; set; }
    }
}
