using Azure;
using DTO.Auth;
using MeuCubico.DTO.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model;
using ServiceStack.Messaging;
using Shared.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MeuCubicoApi.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;

        public AuthController(IAuthService authService, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _authService = authService;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserForAuthenticationDTO dto)
        {
            var user = !string.IsNullOrEmpty(dto.Username)
            ? await userManager.FindByNameAsync(dto.Username!)
            : await userManager.FindByNameAsync(dto.Email!);

            if (user is not null && await userManager.CheckPasswordAsync(user, dto.Password!))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = _authService.GenerateAccessToken(authClaims, configuration);

                var refreshToken = _authService.GenerateRefreshToken();

                _ = int.TryParse(configuration["JWT:RefreshTokenValidityInMinutes"],
                    out int refreshTOkenValidityInMinutes);

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTOkenValidityInMinutes);

                await userManager.UpdateAsync(user);
                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        [HttpPost("Signup")]
        public async Task<IActionResult> Signup([FromBody] UserForRegistrationDTO dto)
        {
            var userExists = await userManager.FindByNameAsync(dto.PhoneNumber!);

            if (userExists != null)
            {
                return Conflict( new AuthResponseDTO{ Status = "Error", Message = "User already exists! "});
            }

            User user = new()
            {
                Name = dto.Name,
                Email = dto.Email,
                BI = dto.BI,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = dto.PhoneNumber,
                PhoneNumber = dto.PhoneNumber
            };
            var result = await userManager.CreateAsync(user, dto.Password!);
            //await userManager.SetTwoFactorEnabledAsync(user, true);

            //if (await userManager.GetTwoFactorEnabledAsync(user))
            //{
            //    return await GenerateOtpVerification(user);
            //}

            if (!result.Succeeded) {
                return StatusCode(StatusCodes.Status500InternalServerError, new AuthResponseDTO{ Status = "Error", Message = "User creation failed. "});
            }
            return Ok(new AuthResponseDTO { Status = "Success", Message = "User created successfully!" });
        }

        //private async Task<IActionResult> GenerateOtpVerification(User user)
        //{
        //    var token = await userManager.GenerateTwoFactorTokenAsync(user, "Phone");

        //    // 
        //    await SendSmsAsync(user.PhoneNumber, $"Your OTP is: {token}");

        //    return Ok(new AuthResponseDTO
        //    {
        //        Status = "Success",
        //        Message = "User created successfully! Please verify your phone number.",
        //        Is2FactorRequired = true,
        //        Provider = "Phone"
        //    });
        //}

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid client request");
            }

            string? accessToken = dto.AccessToken ?? throw new ArgumentException(nameof(dto));
            string? refreshToken = dto.RefreshToken ?? throw new ArgumentException(nameof(dto));

            var principal = _authService.GetPrincipalFromExpiredToken(accessToken!, configuration);

            if (principal == null)
            {
                return BadRequest("Invalid access token/refresh token");
            }

            string username = principal.Identity.Name;

            var user = await userManager.FindByNameAsync(username!);

            if (user == null || user.RefreshToken != refreshToken
                             || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Inalid access token/refresh token");
            }

            var newAccessToken = _authService.GenerateAccessToken(principal.Claims.ToList(), configuration);
            var newRefreshToken = _authService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await userManager.UpdateAsync(user);

            return new ObjectResult(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken,
            });
        }
        [Authorize]
        [HttpPost("revoke/{username}")]
        public async Task<IActionResult> Revoke (string username)
        {
            var user = await userManager.FindByNameAsync (username);

            if (user == null) return BadRequest("Invalid username");

            user.RefreshToken = null;

            await userManager.UpdateAsync(user);
            return NoContent();
        }
    }
}

