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
        [Required(ErrorMessage = "The username is required")]
        public string? Username { get; set; }
        public string? Email { get; set; }
        [Required(ErrorMessage = "The password is required")]
        public string? Password { get; set; }
    }
}
