using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class User : IdentityUser
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        [StringLength(14)]
        public string? BI { get; set; }
        public string? Photo { get; set; }
        public string? Position { get; set; } = "regular";
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }   
}
