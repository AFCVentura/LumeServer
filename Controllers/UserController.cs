using LumeServer.DTOs;
using LumeServer.Models.Movie;
using LumeServer.Models.Question;
using LumeServer.Models.User;
using LumeServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LumeServer.Controllers
{
    // Essa é a classe Controller, ela é responsável por receber as requisições e retornar as respostas.
    [ApiController]
    [Route("api/v1/users")]
    public class UserController : ControllerBase
    {
        private UserService _service;
        private readonly UserManager<User> _userManager;

        public UserController(UserService service, UserManager<User> userManager)
        {
            _service = service;
            _userManager = userManager;
        }


        #region Métodos de autenticação
        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            // Pega o email do claim (caso não tenha o ID no token)
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (email == null)
                return Unauthorized();

            // Busca o usuário no banco pelo email
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return NotFound();

            return Ok(new
            {
                id = user.Id,
                email = user.Email,
                // Qualquer outro dado que quiser retornar
            });
        }

        [Authorize]
        [HttpGet("{id}/do-i-have-recommendations")]
        public async Task<IActionResult> GetDoIHaveRecommendations([FromRoute] string id)
        {
            bool doIHaveRecommendations = await _service.DoIHaveRecommendations(id);

            if (!doIHaveRecommendations)
                return Ok(false);

            return Ok(true);
        }


        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _service.LogoutAsync();
                return Ok(new { message = "Logout efetuado com sucesso." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}. Exceção interna: {ex.InnerException}");
                return StatusCode(500, "Erro ao processar a solicitação. Por favor, tente novamente mais tarde.");
            }
        }

        [HttpPatch("change-username")]
        [Authorize]
        public async Task<IActionResult> ChangeUserName([FromBody] string newDisplayName)
        {
            var success = await _service.ChangeDisplayNameAsync(User, newDisplayName);
            if (!success)
                return BadRequest(new { message = "Falha ao alterar nome de usuário." });

            return Ok(new { message = "Nome de usuário alterado com sucesso." });
        }

        [HttpPatch("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var user = await _service.GetUserByClaimsAsync(User);
            if (user == null)
                return NotFound(new { message = "Usuário não encontrado." });

            var result = await _service.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    message = "Erro ao alterar a senha.",
                    errors = result.Errors.Select(e => e.Description)
                });
            }

            return Ok(new { message = "Senha alterada com sucesso." });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            var result = await _service.ForgotPasswordAsync(email);

            if (!result) return NotFound("Usuário não encontrado");

            return Ok("Código de redefinição enviado para seu e-mail.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var result = await _service.ResetPasswordAsync(dto.Email, dto.Token, dto.NewPassword);
            if (!result) return BadRequest("Erro ao redefinir senha.");

            return Ok("Senha redefinida com sucesso.");
        }

        [Authorize]
        [HttpDelete("delete-account")]
        public async Task<IActionResult> DeleteAccount()
        {
            var success = await _service.DeleteAccountAsync(User);

            if (!success)
                return BadRequest("Erro ao deletar a conta.");

            return Ok("Conta deletada com sucesso.");
        }
        #endregion

    }
}
