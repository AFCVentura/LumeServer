using LumeServer.DTOs;
using LumeServer.Models.Movie;
using LumeServer.Models.Question;
using LumeServer.Models.User;
using LumeServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LumeServer.Controllers
{
    // Essa é a classe Controller, ela é responsável por receber as requisições e retornar as respostas.
    [ApiController]
    [Route("api/v1/users")]
    public class UserController : ControllerBase
    {
        private UserService _service;

        public UserController(UserService service)
        {
            _service = service;
        }


        // Exemplo de action (método que recebe uma requisição)
        // Esse método lida com a url /api/user
        [HttpGet]
        [Authorize] // Exige que o usuário esteja autenticado
        public List<User> GetAll()
        {
            return _service.GetAllUsers();
        }


        #region Métodos de autenticação
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _service.LogoutAsync();
            return Ok(new { message = "Logout efetuado com sucesso." });
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
            var baseUrl = $"{Request.Scheme}://{Request.Host}/reset-password"; // ou configure no appsettings
            var result = await _service.ForgotPasswordAsync(email, baseUrl);

            if (!result) return NotFound("Usuário não encontrado");
            return Ok("E-mail de redefinição enviado.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var result = await _service.ResetPasswordAsync(dto.Email, dto.Token, dto.NewPassword);
            if (!result.Succeeded) return BadRequest("Erro ao redefinir senha.");

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
