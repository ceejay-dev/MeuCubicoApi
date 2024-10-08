﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Auth
{
    public class UserForRegistrationDTO
    {
        public string? Name { get; set; }
        [Required]
        [StringLength(14)]
        public string? BI { get; set; }
        [Required (ErrorMessage = "Phone Number is required")]
        [StringLength(9)]
        public string? PhoneNumber { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "Email is required.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}
