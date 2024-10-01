using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Auth
{
    public class AuthResponseDTO
    {
        public bool Succeeded { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
        public string? Message { get; set; }
        public bool Is2FactorRequired { get; set; }
        public string ? Provider { get; set; }
        public string? RedirectUrl { get; set; }
    }   
}
