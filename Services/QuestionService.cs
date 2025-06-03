using LumeServer.Data;
using LumeServer.DTOs.LumeAI;
using LumeServer.Models.Movie;
using LumeServer.Models.Question;
using Microsoft.EntityFrameworkCore;

namespace LumeServer.Services
{
    public class QuestionService
    {
        private readonly LumeDataContext _context;
        private readonly LumeAIService _lumeAIService;

        public QuestionService(LumeDataContext context, LumeAIService lumeAIService)
        {
            _context = context;
            _lumeAIService = lumeAIService;
        }

        #region Métodos para as perguntas e respostas
        // Enviar perguntas extras de perfil geral
        // Eager: Carrega as alternativas também
        public async Task<List<ExtraQuestion>> FindAllGeneralExtraQuestionsEagerAsync()
        {
            return await _context.ExtraQuestions.AsNoTracking().Where(eq => eq.IsGeneralProfile).Include(eq => eq.Answers).ToListAsync();
        }

        // Enviar perguntas de tema de perfil geral
        // Eager: Carrega as alternativas também
        public async Task<List<ThemeQuestion>> FindAllGeneralThemeQuestionsEagerAsync()
        {
            return await _context.ThemeQuestions.AsNoTracking().Where(eq => eq.IsGeneralProfile).Include(tq => tq.ThemeAnswers).ToListAsync();
        }



        // Enviar perguntas extras de perfil do momento
        // Eager: Carrega as alternativas também
        public async Task<List<ExtraQuestion>> FindAllDailyExtraQuestionsEagerAsync()
        {
            return await _context.ExtraQuestions.AsNoTracking().Where(eq => !eq.IsGeneralProfile).Include(eq => eq.Answers).ToListAsync();
        }

        // Enviar perguntas de tema de perfil do momento
        // Eager: Carrega as alternativas também
        public async Task<List<ThemeQuestion>> FindAllDailyThemeQuestionsEagerAsync()
        {
            return await _context.ThemeQuestions.AsNoTracking().Where(eq => !eq.IsGeneralProfile).Include(tq => tq.ThemeAnswers).ToListAsync();
        }


        // Enviar 20 filmes famosos (com mais de 15 mil votos) aleatórios
        public async Task<List<Movie>> GetFamousMoviesAsync()
        {
            return await _context.Movies
                .AsNoTracking()
                .Where(m => m.VoteCount > 15000)
                .Include(m => m.MovieGenres)
                    .ThenInclude(mg => mg.Genre)
                .OrderBy(x => EF.Functions.Random()) // Aleatório
                .Take(20)
                .ToListAsync();
        }

        public async Task PostChosenThemeAnswersAndMoviesAsync(List<int> themeAnswerIds, List<int> chosenMoviesId, string Id)
        {
            // Pega a resposta de tema, seus gêneros e palavras-chave equivalentes.
            var themeAnswers = await _context.ThemeAnswers
                .Where(ta => themeAnswerIds.Contains(ta.Id))
                .Include(ta => ta.ThemeAnswerGenres)
                    .ThenInclude(tag => tag.Genre)
                .Include(ta => ta.ThemeAnswerKeywords)
                    .ThenInclude(tak => tak.Keyword)
                .ToListAsync();

            var movies = await _context.Movies
                .Where(m => chosenMoviesId.Contains(m.Id))
                .Include(m => m.MovieGenres)
                    .ThenInclude(mg => mg.Genre)
                .Include(m => m.MovieKeywords)
                    .ThenInclude(mg => mg.Keyword)
                .ToListAsync();


            var genres = new HashSet<string>();
            var keywords = new HashSet<string>();

            // Adiciona os gêneros e palavras-chave das respostas de tema (VER UMA FORMA DE NÃO ADICIONAR OS DUPLICADOS)
            foreach (var answer in themeAnswers)
            {
                foreach (var genre in answer.ThemeAnswerGenres.Select(g => g.Genre.Name))
                    genres.Add(genre);
                foreach (var keyword in answer.ThemeAnswerKeywords.Select(k => k.Keyword.Name))
                    keywords.Add(keyword);
            }

            foreach (var movie in movies)
            {
                foreach (var genre in movie.MovieGenres.Select(g => g.Genre.Name))
                    genres.Add(genre);
                foreach (var keyword in movie.MovieKeywords.Select(k => k.Keyword.Name))
                    keywords.Add(keyword);
            }


            var inputData = new MovieData
            {
                Genres = string.Join(",", genres),
                Keywords = string.Join(",", keywords),
                Title = "", // Pode ser vazio
                Overview = "", // Pode ser vazio
                OriginalLanguage = "", // Pode ser vazio
                ProductionCountries = "", // Pode ser vazio
                ProductionCompanies = "", // Pode ser vazio
            };

            var closestClusterIds = _lumeAIService.GetClosestClusters(inputData);


            // TESTAR ISSO DEPOIS
            Console.WriteLine($"Clusters mais próximos: {closestClusterIds[0]}, {closestClusterIds[1]}, {closestClusterIds[2]}");

        }

        public async Task PutGeneralProfileExtraPreferencesAsync(List<int> extraAnswerIds, string id)
        {
            if (extraAnswerIds is null || !extraAnswerIds.Any())
                throw new ArgumentException("No answer IDs provided.");

            // Buscar todas as respostas válidas de uma vez
            var extraAnswers = await _context.ExtraAnswers
                .Where(ea => extraAnswerIds.Contains(ea.Id))
                .ToListAsync();


            // Buscar o usuário
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user is null)
                throw new Exception("User not found.");


            user.MinYear = extraAnswers.Max(e => e.MinYear);
            user.MaxYear = extraAnswers.Min(e => e.MaxYear);

            user.MinVoteAverage = extraAnswers.Max(e => e.MinVoteAverage);
            user.MaxVoteAverage = extraAnswers.Min(e => e.MaxVoteAverage);

            user.MinVoteCount = extraAnswers.Max(e => e.MinVoteCount);
            user.MaxVoteCount = extraAnswers.Min(e => e.MaxVoteCount);

            user.MinDuration = extraAnswers.Max(e => e.MinDuration);
            user.MaxDuration = extraAnswers.Min(e => e.MaxDuration);


            await _context.SaveChangesAsync();
        }

        #endregion
    }
}
