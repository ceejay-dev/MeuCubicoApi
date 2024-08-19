using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ResidentDTO
    {
        public int ResidentId { get; set; }
        public string? Name { get; set; }
        public string? BI { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Photo { get; set; }
    }
}
