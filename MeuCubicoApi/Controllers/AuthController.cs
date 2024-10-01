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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;

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
            var authResult = await _authService.AuthenticateAdeptoAsync(dto);

            if (authResult.Succeeded)
            {
                return Ok(new
                {
                    Token = authResult.Token,
                    RefreshToken = authResult.RefreshToken,
                    Expiration = authResult.Expiration
                });
            }

            return Unauthorized(new AuthResponseDTO { Succeeded = false, Message = "Credenciais incorrectas." });
        }

        [HttpPost("Signup")]
        public async Task<IActionResult> Signup([FromBody] UserForRegistrationDTO dto)
        {
            try
            {
                var user = await _authService.CreateUserAsync(dto);
                var otp = await _authService.GenerateOtpVerificationAsync(user);
                return Ok(new AuthResponseDTO
                {
                    Succeeded = true,
                    Is2FactorRequired = true,
                    Provider = TokenOptions.DefaultPhoneProvider,
                    Message = "Usuário criado com sucesso. Por favor, verifique seu número de telefone."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new AuthResponseDTO
                {
                    Succeeded = false,
                    Message = ex.ToString()
                });
            }
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyOtp([FromBody] OtpVerificationDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.OtpCode))
            {
                return BadRequest(new AuthResponseDTO { Succeeded = false, Message = "Número de telefone e código OTP são obrigatórios." });
            }

            var user = await userManager.FindByNameAsync(dto.Email);
            if (user == null)
            {
                return NotFound(new AuthResponseDTO { Succeeded = false, Message = "Usuário não encontrado." });
            }

            var isValid = await userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultPhoneProvider, dto.OtpCode);
            if (!isValid)
            {
                return BadRequest(new AuthResponseDTO { Succeeded = false, Message = "Código OTP inválido." });
            }

            user.PhoneNumberConfirmed = true;
            await userManager.UpdateAsync(user);

            return Ok(new AuthResponseDTO { Succeeded = true, Message = "Código OTP verificado com sucesso." });
        }

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
        public async Task<IActionResult> Revoke(string username)
        {
            var user = await userManager.FindByNameAsync(username);

            if (user == null) return BadRequest("Invalid username");

            user.RefreshToken = null;

            await userManager.UpdateAsync(user);
            return NoContent();
        }
    }
}
