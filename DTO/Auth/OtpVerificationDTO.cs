﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeuCubico.DTO.Auth
{
    public class OtpVerificationDTO
    {
        [Required]
        public string ? Email { get; set; }
        public string ? OtpCode { get; set; }
    }
}
