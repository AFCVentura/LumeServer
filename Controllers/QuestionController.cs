using LumeServer.DTOs;
using LumeServer.Models.Movie;
using LumeServer.Models.Question;
using LumeServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LumeServer.Controllers
{
    [ApiController]
    [Route("api/v1/questions")]
    public class QuestionController : ControllerBase
    {
        private QuestionService _service;

        public QuestionController(QuestionService service)
        {
            _service = service;
        }


        #region Métodos de perguntas e respostas

        // Usuário recebe perguntas de perfil geral (com alternativas)
        [HttpGet("general-questions")]
        public async Task<ActionResult<List<ThemeAndExtraQuestions>>> GetGeneralExtraQuestions()
        {
            try
            {
                var extraQuestions = await _service.FindAllGeneralExtraQuestionsEagerAsync();
                var themeQuestions = await _service.FindAllGeneralThemeQuestionsEagerAsync();

                var result = new ThemeAndExtraQuestions
                {
                    ExtraQuestions = extraQuestions,
                    ThemeQuestions = themeQuestions
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}. Exceção interna: {ex.InnerException}");
                return StatusCode(500, "Erro ao processar a solicitação. Por favor, tente novamente mais tarde.");
            }
        }

        // Usuário recebe perguntas de perfil diário (com alternativas)
        [HttpGet("daily-questions")]
        public async Task<ActionResult<List<ThemeAndExtraQuestions>>> GetDailyExtraQuestions()
        {
            try
            {
                var extraQuestions = await _service.FindAllDailyExtraQuestionsEagerAsync();
                var themeQuestions = await _service.FindAllDailyThemeQuestionsEagerAsync();
                var result = new ThemeAndExtraQuestions
                {
                    ExtraQuestions = extraQuestions,
                    ThemeQuestions = themeQuestions
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}. Exceção interna: {ex.InnerException}");
                return StatusCode(500, "Erro ao processar a solicitação. Por favor, tente novamente mais tarde.");
            }
        }

        // Usuário recebe 20 filmes famosos (com mais de 15 mil votos) aleatórios
        [HttpGet("famous-movies")]
        public async Task<ActionResult<List<MovieDetailsDTO>>> GetFamousMovies()
        {
            try
            {
                return Ok(await _service.GetFamousMoviesAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}. Exceção interna: {ex.InnerException}");
                return StatusCode(500, "Erro ao processar a solicitação. Por favor, tente novamente mais tarde.");
            }
        }

        // Usuário envia alternativas de tema e extras escolhidas (na criação da conta) 
        [HttpPost("general-answers/{id}")]
        public async Task<ActionResult> PostChosenGeneralThemeAnswers([FromRoute] string id, [FromBody] ChosenThemeAndExtraAnswersAndMoviesRequestDTO request)
        {
            try
            {
                await _service.PutGeneralProfileExtraPreferencesAsync(request.ExtraAnswerIds, id);
                await _service.PostChosenThemeAnswersAndMoviesAsync(request.ThemeAnswerIds, request.ChosenMovieIds, id);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}. Exceção interna: {ex.InnerException}");
                return StatusCode(500, "Erro ao processar a solicitação. Por favor, tente novamente mais tarde.");
            }
        }

        // Usuário envia alternativas de tema e extras escolhidas (no uso do dia a dia)
        [Authorize]
        [HttpPost("daily-answers/{id}")]
        public async Task<ActionResult> PostChosenDailyThemeAnswers([FromRoute] string id, [FromBody] ChosenThemeAndExtraAnswersRequestDTO request)
        {
            try
            {
                await _service.PutDailyProfileExtraPreferencesAsync(request.ExtraAnswerIds, id);
                await _service.PutDailyProfileThemePreferencesAsync(request.ThemeAnswerIds, id);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}. Exceção interna: {ex.InnerException}");
                return StatusCode(500, "Erro ao processar a solicitação. Por favor, tente novamente mais tarde.");
            }
        }


        #endregion
    }
}
