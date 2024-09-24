using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Auth
{
    public class AuthResponseDTO
    {
        public string? Message { get; set; }
        public string? Status { get; set; }
        public bool Is2FactorRequired { get; set; }
        public string ? Provider { get; set; }
    }   
}
