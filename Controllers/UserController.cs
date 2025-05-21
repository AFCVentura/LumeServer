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
    [Route("api/v1/[controller]")]
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

        // Usuário recebe perguntas extras de perfil geral (com alternativas)
        [HttpGet("get-general-extra-questions")]
        public async Task<List<ExtraQuestion>> GetGeneralExtraQuestions()
        {
            return await _service.FindAllGeneralExtraQuestionsEagerAsync();
        }

        // Usuário recebe perguntas de tema de perfil geral (com alternativas)
        [HttpGet("get-general-theme-questions")]
        public async Task<List<ThemeQuestion>> GetGeneralThemeQuestions()
        {
            return await _service.FindAllGeneralThemeQuestionsEagerAsync();
        }

        // Usuário recebe perguntas extras de perfil geral (com alternativas)
        [HttpGet("get-daily-extra-questions")]
        public async Task<List<ExtraQuestion>> GetDailyExtraQuestions()
        {
            return await _service.FindAllDailyExtraQuestionsEagerAsync();
        }

        // Enviar perguntas de tema de perfil geral (com alternativas)
        [HttpGet("get-daily-theme-questions")]
        public async Task<List<ThemeQuestion>> GetDailyThemeQuestions()
        {
            return await _service.FindAllDailyThemeQuestionsEagerAsync();
        }

        // Usuário recebe 20 filmes famosos (com mais de 15 mil votos) aleatórios
        [HttpGet("get-famous-movies")]
        public async Task<List<Movie>> GetFamousMovies()
        {
            return await _service.GetFamousMoviesAsync();
        }

        // Usuário envia alternativas de tema escolhidas (na criação da conta) 
        [HttpPost("general-theme-answers")]
        public async Task PostChosenGeneralThemeAnswers([FromBody] ChosenThemeAnswersRequestDTO request)
        {
            try
            {
                await _service.PostChosenThemeAnswersAndMoviesAsync(request.ThemeAnswerIds, request.ChosenMovieIds, request.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}. Exceção interna: {ex.InnerException}");
            }
        }
        // Usuário envia alternativas extras escolhidas (na criação da conta)
        [HttpPost("general-extra-answers")]
        public async Task PostChosenGeneralExtraAnswers([FromBody] ChosenExtraAnswersRequestDTO request)
        {
            try
            {
                await _service.PutGeneralProfileExtraPreferencesAsync(request.ExtraAnswerIds, request.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}. Exceção interna: {ex.InnerException}");
            }
        }

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

        [Authorize]
        [HttpDelete("delete-account")]
        public async Task<IActionResult> DeleteAccount()
        {
            var success = await _service.DeleteAccountAsync(User);

            if (!success)
                return BadRequest("Erro ao deletar a conta.");

            return Ok("Conta deletada com sucesso.");
        }


    }
}
