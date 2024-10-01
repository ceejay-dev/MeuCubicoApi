using AutoMapper;
using AutoMapper.Configuration;
using DTO.Auth;
using Microsoft.Extensions.Configuration;
using Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Shared.IServices
{
    public interface IAuthService
    {
        JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration _config);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration _config);
        Task<AuthResponseDTO> AuthenticateAdeptoAsync(UserForAuthenticationDTO dto);
        Task<string> GenerateOtpVerificationAsync(User user);
        Task<User> CreateUserAsync(UserForRegistrationDTO dto);
    }
}
