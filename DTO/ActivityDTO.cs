using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ActivityDTO
    {
        public int ActivityId { get; set; }
        public string? Description { get; set; }
    }
}
