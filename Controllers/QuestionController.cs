using LumeServer.DTOs;
using LumeServer.Models.Movie;
using LumeServer.Models.Question;
using LumeServer.Services;
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

        // Usuário recebe perguntas extras de perfil geral (com alternativas)
        [HttpGet("general-extra-questions")]
        public async Task<List<ExtraQuestion>> GetGeneralExtraQuestions()
        {
            return await _service.FindAllGeneralExtraQuestionsEagerAsync();
        }

        // Usuário recebe perguntas de tema de perfil geral (com alternativas)
        [HttpGet("general-theme-questions")]
        public async Task<List<ThemeQuestion>> GetGeneralThemeQuestions()
        {
            return await _service.FindAllGeneralThemeQuestionsEagerAsync();
        }

        // Usuário recebe perguntas extras de perfil geral (com alternativas)
        [HttpGet("daily-extra-questions")]
        public async Task<List<ExtraQuestion>> GetDailyExtraQuestions()
        {
            return await _service.FindAllDailyExtraQuestionsEagerAsync();
        }

        // Enviar perguntas de tema de perfil geral (com alternativas)
        [HttpGet("daily-theme-questions")]
        public async Task<List<ThemeQuestion>> GetDailyThemeQuestions()
        {
            return await _service.FindAllDailyThemeQuestionsEagerAsync();
        }

        // Usuário recebe 20 filmes famosos (com mais de 15 mil votos) aleatórios
        [HttpGet("famous-movies")]
        public async Task<List<Movie>> GetFamousMovies()
        {
            return await _service.GetFamousMoviesAsync();
        }

        // Usuário envia alternativas de tema escolhidas (na criação da conta) 
        [HttpPost("general-theme-answers")]
        public async Task PostChosenGeneralThemeAnswers([FromBody] ChosenThemeAnswersAndMoviesRequestDTO request)
        {
            try
            {
                await _service.PostChosenThemeAnswersAndMoviesAsync(request.ThemeAnswerIds, request.ChosenMovieIds, request.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}. Exceção interna: {ex.InnerException}");
                StatusCode(500, "Erro ao processar a solicitação. Por favor, tente novamente mais tarde.");
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
                StatusCode(500, "Erro ao processar a solicitação. Por favor, tente novamente mais tarde.");
            }
        }
        // Usuário envia alternativas de tema escolhidas (no uso do dia a dia)
        [HttpPost("daily-theme-answers")]
        public async Task PostChosenDailyThemeAnswers([FromBody] ChosenThemeAnswersRequestDTO request)
        {
            try
            {
                await _service.PutDailyProfileThemePreferencesAsync(request.ThemeAnswerIds, request.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}. Exceção interna: {ex.InnerException}");
                StatusCode(500, "Erro ao processar a solicitação. Por favor, tente novamente mais tarde.");
            }
        }
        // Usuário envia alternativas extras escolhidas (no uso do dia a dia)
        [HttpPost("daily-extra-answers")]
        public async Task PostChosenDailyExtraAnswers([FromBody] ChosenExtraAnswersRequestDTO request)
        {
            try
            {
                await _service.PutDailyProfileExtraPreferencesAsync(request.ExtraAnswerIds, request.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}. Exceção interna: {ex.InnerException}");
                StatusCode(500, "Erro ao processar a solicitação. Por favor, tente novamente mais tarde.");
            }
        }

        #endregion
    }
}
