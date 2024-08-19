using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Table("Activities")]
    public class Activity
    {
        [Key]
        public int ActivityId { get; set; }
        [Required]
        public string ? Description { get; set; }
    }
}
