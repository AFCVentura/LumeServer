using LumeServer.Data;
using LumeServer.DTOs;
using LumeServer.DTOs.LumeAI;
using LumeServer.Models.Movie;
using LumeServer.Models.Question;
using LumeServer.Models.User;
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
        public async Task<List<MovieDetailsDTO>> GetFamousMoviesAsync()
        {
            return await _context.Movies
                .AsNoTracking()
                .Where(m => m.VoteCount > 15000)
                .Include(m => m.MovieGenres)
                    .ThenInclude(mg => mg.Genre)
                .Include(m => m.MovieKeywords)
                    .ThenInclude(mk => mk.Keyword)
                .Include(m => m.MovieProductionCountries)
                    .ThenInclude(mpc => mpc.ProductionCountry)
                .Include(m => m.MovieSpokenLanguages)
                    .ThenInclude(mpc => mpc.SpokenLanguage)
                .Include(m => m.MovieProductionCompanies)
                    .ThenInclude(mpc => mpc.ProductionCompany)
                .Select(m => new MovieDetailsDTO
                {
                    Id = m.Id,
                    Title = m.Title,
                    VoteAverage = m.VoteAverage,
                    VoteCount = m.VoteCount,
                    Status = m.Status,
                    ReleaseDate = m.ReleaseDate,
                    Revenue = m.Revenue,
                    Runtime = m.Runtime,
                    Adult = m.Adult,
                    BackdropPath = m.BackdropPath,
                    Budget = m.Budget,
                    Homepage = m.Homepage,
                    ImdbId = m.ImdbId,
                    OriginalLanguage = m.OriginalLanguage,
                    OriginalTitle = m.OriginalTitle,
                    Overview = m.Overview,
                    Popularity = m.Popularity,
                    PosterPath = m.PosterPath,
                    Tagline = m.Tagline,
                    Genres = m.MovieGenres.Select(mg => mg.Genre.Name).ToList(),
                    Keywords = m.MovieKeywords.Select(mk => mk.Keyword.Name).ToList(),
                    ProductionCompanies = m.MovieProductionCompanies.Select(mpc => mpc.ProductionCompany.Name).ToList(),
                    ProductionCountries = m.MovieProductionCountries.Select(mpc => mpc.ProductionCountry.Name).ToList(),
                    SpokenLanguages = m.MovieSpokenLanguages.Select(msl => msl.SpokenLanguage.Name).ToList()
                })
                .OrderBy(x => EF.Functions.Random()) // Aleatório
                .Take(5)
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

            // Pega os filmes escolhidos, seus gêneros e palavras-chave equivalentes.
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


            // Cria a estrutura que a IA vai usar para comparar
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

            // Chama o método do service da IA que faz a vetorização e comparação dos filmes com base nos parâmetros
            var closestClusterIds = await _lumeAIService.GetClosestClusters(inputData);

            // Salva os clusters mais próximos no perfil geral do usuário
            _context.UserGeneralProfileClusters.AddRange(closestClusterIds.Select(c => new UserGeneralProfileCluster
            {
                UserId = Id,
                ClusterId = c.Id,
            }));

            await _context.SaveChangesAsync();

        }

        public async Task PutGeneralProfileExtraPreferencesAsync(List<int> extraAnswerIds, string id)
        {
            if (extraAnswerIds is null || !extraAnswerIds.Any())
                throw new ArgumentException("No answer IDs provided.");

            // Buscar todas as respostas válidas de uma vez
            var extraAnswers = await _context.ExtraAnswers
                .Where(ea => extraAnswerIds.Contains(ea.Id))
                .Include(ea => ea.ExtraAnswerProductionCountries)
                .Include(ea => ea.ExtraAnswerSpokenLanguages)
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

            var userHasFavoriteProductionCountries = await _context.UserGeneralProfileProductionCountries.AnyAsync(ugppc => ugppc.UserId == id);
            var userHasFavoriteSpokenLanguages = await _context.UserGeneralProfileSpokenLanguages.AnyAsync(ugpsl => ugpsl.UserId == id);

            if (userHasFavoriteProductionCountries)
            {
                // Busca os relacionamentos entre o usuário e os países de produção
                var userProductionCountries = await _context.UserGeneralProfileProductionCountries
                    .Where(ugppc => ugppc.UserId == id)
                    .ToListAsync();

                // Remove os relacionamentos, se existirem
                if (userProductionCountries.Any())
                {
                    _context.UserGeneralProfileProductionCountries.RemoveRange(userProductionCountries);
                    await _context.SaveChangesAsync();
                }
            }

            if (userHasFavoriteSpokenLanguages)
            {
                // Busca os relacionamentos entre o usuário e os idiomas falados
                var userSpokenLanguages = await _context.UserGeneralProfileSpokenLanguages
                    .Where(ugpsl => ugpsl.UserId == id)
                    .ToListAsync();
                // Remove os relacionamentos, se existirem
                if (userSpokenLanguages.Any())
                {
                    _context.UserGeneralProfileSpokenLanguages.RemoveRange(userSpokenLanguages);
                    await _context.SaveChangesAsync();
                }
            }

            HashSet<int> productionCountryIds = new HashSet<int>();
            HashSet<int> spokenLanguageIds = new HashSet<int>();
            foreach (var extraAnswer in extraAnswers)
            {
                foreach (var productionCountry in extraAnswer.ExtraAnswerProductionCountries)
                {
                    // Adiciona os IDs dos países de produção ao conjunto
                    productionCountryIds.Add(productionCountry.ProductionCountryId);
                }

                foreach (var spokenLanguage in extraAnswer.ExtraAnswerSpokenLanguages)
                {
                    // Adiciona os IDs dos idiomas falados ao conjunto
                    spokenLanguageIds.Add(spokenLanguage.SpokenLanguageId);
                }
            }

            // Adiciona os países de produção ao perfil geral do usuário
            foreach (var productionCountryId in productionCountryIds)
            {
                _context.UserGeneralProfileProductionCountries.Add(new UserGeneralProfileProductionCountry
                {
                    UserId = id,
                    ProductionCountryId = productionCountryId
                });
            }
            // Adiciona os idiomas falados ao perfil geral do usuário
            foreach (var spokenLanguageId in spokenLanguageIds)
            {
                _context.UserGeneralProfileSpokenLanguages.Add(new UserGeneralProfileSpokenLanguage
                {
                    UserId = id,
                    SpokenLanguageId = spokenLanguageId
                });
            }
            await _context.SaveChangesAsync();
        }

        public async Task PutDailyProfileExtraPreferencesAsync(List<int> extraAnswerIds, string id)
        {
            if (extraAnswerIds is null || !extraAnswerIds.Any())
                throw new ArgumentException("No answer IDs provided.");

            // Buscar todas as respostas válidas de uma vez
            var extraAnswers = await _context.ExtraAnswers
                .Where(ea => extraAnswerIds.Contains(ea.Id))
                .Include(ea => ea.ExtraAnswerProductionCountries)
                .Include(ea => ea.ExtraAnswerSpokenLanguages)
                .ToListAsync();

            // Buscar o usuário
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user is null)
                throw new Exception("User not found.");

            var oldUserDailyProfile = _context.UserDailyProfiles.FirstOrDefault(udp => udp.UserId == id && udp.IsTheLatest);
            if (oldUserDailyProfile is not null)
            {
                // Marca o perfil diário antigo como não sendo o mais recente
                oldUserDailyProfile.IsTheLatest = false;
                _context.UserDailyProfiles.Update(oldUserDailyProfile);
            }

            var newUserDailyProfile = new UserDailyProfile
            {
                UserId = id,
                MinYear = extraAnswers.Max(e => e.MinYear),
                MaxYear = extraAnswers.Min(e => e.MaxYear),
                MinVoteAverage = extraAnswers.Max(e => e.MinVoteAverage),
                MaxVoteAverage = extraAnswers.Min(e => e.MaxVoteAverage),
                MinVoteCount = extraAnswers.Max(e => e.MinVoteCount),
                MaxVoteCount = extraAnswers.Min(e => e.MaxVoteCount),
                MinDuration = extraAnswers.Max(e => e.MinDuration),
                MaxDuration = extraAnswers.Min(e => e.MaxDuration),
                UserDailyProfileClusters = new List<UserDailyProfileCluster>(),
                UserDailyProfileSpokenLanguages = new List<UserDailyProfileSpokenLanguage>(),
                UserDailyProfileProductionCountries = new List<UserDailyProfileProductionCountry>(),
                Timestamp = DateTime.UtcNow,
                IsTheLatest = true
            };

            HashSet<int> productionCountryIds = new HashSet<int>();
            HashSet<int> spokenLanguageIds = new HashSet<int>();
            foreach (var extraAnswer in extraAnswers)
            {
                foreach (var productionCountry in extraAnswer.ExtraAnswerProductionCountries)
                {
                    // Adiciona os IDs dos países de produção ao conjunto
                    productionCountryIds.Add(productionCountry.ProductionCountryId);
                }

                foreach (var spokenLanguage in extraAnswer.ExtraAnswerSpokenLanguages)
                {
                    // Adiciona os IDs dos idiomas falados ao conjunto
                    spokenLanguageIds.Add(spokenLanguage.SpokenLanguageId);
                }
            }

            // Adiciona os países de produção ao perfil geral do usuário
            foreach (var productionCountryId in productionCountryIds)
            {
                newUserDailyProfile.UserDailyProfileProductionCountries.Add(new UserDailyProfileProductionCountry
                {
                    UserDailyProfileId = newUserDailyProfile.Id,
                    ProductionCountryId = productionCountryId
                });
            }
            // Adiciona os idiomas falados ao perfil geral do usuário
            foreach (var spokenLanguageId in spokenLanguageIds)
            {
                newUserDailyProfile.UserDailyProfileSpokenLanguages.Add(new UserDailyProfileSpokenLanguage
                {
                    UserDailyProfileId = newUserDailyProfile.Id,
                    SpokenLanguageId = spokenLanguageId
                });
            }

            _context.Add(newUserDailyProfile);

            await _context.SaveChangesAsync();
        }

        public async Task PutDailyProfileThemePreferencesAsync(List<int> themeAnswerIds, string id)
        {
            // Pega a resposta de tema, seus gêneros e palavras-chave equivalentes.
            var themeAnswers = await _context.ThemeAnswers
                .Where(ta => themeAnswerIds.Contains(ta.Id))
                .Include(ta => ta.ThemeAnswerGenres)
                    .ThenInclude(tag => tag.Genre)
                .Include(ta => ta.ThemeAnswerKeywords)
                    .ThenInclude(tak => tak.Keyword)
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


            // Cria a estrutura que a IA vai usar para comparar
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

            // Chama o método do service da IA que faz a vetorização e comparação dos filmes com base nos parâmetros
            var closestClusterIds = await _lumeAIService.GetClosestClusters(inputData);

            var latestUserDailyProfile = await _context.UserDailyProfiles
                .Where(udp => udp.UserId == id && udp.IsTheLatest)
                .FirstOrDefaultAsync();

            // Salva os clusters mais próximos no perfil geral do usuário
            _context.UserDailyProfileClusters.AddRange(closestClusterIds.Select(c => new UserDailyProfileCluster
            {
                UserDailyProfileId = latestUserDailyProfile.Id,
                ClusterId = c.Id,
            }));

            await _context.SaveChangesAsync();
        }

        #endregion
    }
}
