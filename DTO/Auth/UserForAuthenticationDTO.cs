using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Auth
{
    public class UserForAuthenticationDTO
    {
        [Required(ErrorMessage = "The email is required")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "The password is required")]
        public string? Password { get; set; }
    }
}
