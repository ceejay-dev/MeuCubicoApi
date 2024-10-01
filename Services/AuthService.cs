using AutoMapper;
using DTO.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Model;
using Shared.IServices;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<User> userManager;

        public AuthService(IConfiguration configuration, UserManager<User> userManager)
        {
            this.configuration = configuration;
            this.userManager = userManager;
        }
        //Criacao token de acesso
        public JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration _config)
        {
            var jwtConfig = _config.GetSection("JWT");
            var key = jwtConfig["SecretKey"] ?? throw new InvalidOperationException("Invalid secret key");
            var privateKey = Encoding.UTF8.GetBytes(key);
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(privateKey), SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(jwtConfig.GetValue<double>("TokenValidityInMinutes")),
                Audience = jwtConfig["ValidAudience"],
                Issuer = jwtConfig["ValidIssuer"],
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            return token;
        }

        //Metodo para renovar a sessao do utilizador na app (token de acesso)
        public string GenerateRefreshToken()
        {
            var secureRandomBytes = new byte[128];

            using var randomNumberGenerator = RandomNumberGenerator.Create();

            randomNumberGenerator.GetBytes(secureRandomBytes);

            var refreshToken = Convert.ToBase64String(secureRandomBytes);
            return refreshToken;
        }

        //Criacao e Validacao do token de acesso expirado
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration _config)
        {
            var secretKey = _config["JWT:SecretKey"] ?? throw new InvalidOperationException("Invalid Key");

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid Token");
            }
            return principal;
        }

        public async Task<AuthResponseDTO> AuthenticateAdeptoAsync(UserForAuthenticationDTO dto)
        {
            var user = await userManager.FindByNameAsync(dto.Username ?? dto.Email);
            if (user == null || !await userManager.CheckPasswordAsync(user, dto.Password!))
            {
                return new AuthResponseDTO { Succeeded = false };
            }

            var userRoles = await userManager.GetRolesAsync(user);
            var authClaims = GenerateAuthClaims(user, userRoles);

            var accessToken = GenerateAccessToken(authClaims, configuration);
            var refreshToken = GenerateRefreshToken();

            await UpdateUserRefreshTokenAsync(user, refreshToken);

            return new AuthResponseDTO
            {
                Succeeded = true,
                Token = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = refreshToken,
                Expiration = accessToken.ValidTo
            };
        }

        private List<Claim> GenerateAuthClaims(User user, IList<string> userRoles)
        {
            var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
            authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));
            return authClaims;
        }

        private async Task UpdateUserRefreshTokenAsync(User user, string refreshToken)
        {
            if (int.TryParse(configuration["JWT:RefreshTokenValidityInMinutes"], out int refreshTokenValidityInMinutes))
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);
                await userManager.UpdateAsync(user);
            }
        }

        //Metodo para gerar o codigo otp para validacao do registo do adepto
        public async Task<string> GenerateOtpVerificationAsync(User user)
        {
            var token = await userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultPhoneProvider);
            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("Falha ao gerar o token OTP.");
            }

            //await _smsService.SendSmsAsync(user.PhoneNumber!, $"Código de verificação do registo: {token} \n-SOGEST");
            return token;
        }

        //Metodo para a criacao do utilizador 
        public async Task<User> CreateUserAsync(UserForRegistrationDTO dto)
        {
            var userExists = await userManager.FindByNameAsync(dto.PhoneNumber!);
            if (userExists != null)
            {
                throw new Exception("Um utilizador com este número de telefone já existe.");
            }

            var user = new User
            {
                Name = dto.Name,
                BI = dto.BI,
                Email = dto.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = dto.PhoneNumber,
                PhoneNumber = dto.PhoneNumber
            };

            var result = await userManager.CreateAsync(user, dto.Password!);
            if (!result.Succeeded)
            {
                throw new Exception("Falha na criação do utilizador.");
            }

            await userManager.SetTwoFactorEnabledAsync(user, true);
            return user;
        }
    }
}
