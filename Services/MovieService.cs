using LumeServer.Data;
using LumeServer.DTOs;
using LumeServer.DTOs.LumeAI;
using LumeServer.Models.Movie;
using LumeServer.Models.User;
using Microsoft.EntityFrameworkCore;

namespace LumeServer.Services
{
    public class MovieService
    {
        private readonly LumeDataContext _context;

        public MovieService(LumeDataContext context)
        {
            _context = context;
        }

        #region Métodos da wishlist e carrossel
        public async Task<List<CarouselPreviewWishListDTO>> FindCarouselMovies(string id, int amount = 10)
        {
            return await _context.WishLists
                .Where(wl => wl.UserId == id)
                .OrderByDescending(wl => wl.CreatedAt) // Ordena pela data de criação, do mais recente para o mais antigo
                .Include(wl => wl.Movie)
                .Select(wl => new CarouselPreviewWishListDTO
                {
                    Id = wl.Movie.Id,
                    Title = wl.Movie.Title,
                    PosterPath = wl.Movie.PosterPath
                })
                .Take(amount)
                .ToListAsync();
        }

        public async Task<List<CarouselPreviewWishListDTO>> FindAllWishListMovies(string id)
        {
            return await _context.WishLists
                .Where(wl => wl.UserId == id)
                .OrderByDescending(wl => wl.CreatedAt) // Ordena pela data de criação, do mais recente para o mais antigo
                .Include(wl => wl.Movie)
                .Select(wl => new CarouselPreviewWishListDTO
                {
                    Id = wl.Movie.Id,
                    Title = wl.Movie.Title,
                    PosterPath = wl.Movie.PosterPath
                })
                .ToListAsync();
        }

        public async Task<MovieDetailsDTO> FindWishListMovieById(string userId, int movieId)
        {
            MovieDetailsDTO? movieDetails = await _context.WishLists
            .Where(wl => wl.UserId == userId && wl.MovieId == movieId)
            .Include(wl => wl.Movie)
                .ThenInclude(m => m.MovieGenres)
                    .ThenInclude(mg => mg.Genre)
            .Include(wl => wl.Movie)
                .ThenInclude(m => m.MovieKeywords)
                    .ThenInclude(mk => mk.Keyword)
            .Include(wl => wl.Movie)
                .ThenInclude(m => m.MovieProductionCompanies)
                    .ThenInclude(mpc => mpc.ProductionCompany)
            .Include(wl => wl.Movie)
                .ThenInclude(m => m.MovieProductionCountries)
                    .ThenInclude(mpc => mpc.ProductionCountry)
            .Include(wl => wl.Movie)
                .ThenInclude(m => m.MovieSpokenLanguages)
                    .ThenInclude(msl => msl.SpokenLanguage)
            .Include(wl => wl.Movie)
                .ThenInclude(m => m.Cluster)
            .Select(wl => new MovieDetailsDTO
            {
                Id = wl.Movie.Id,
                Title = wl.Movie.Title,
                VoteAverage = wl.Movie.VoteAverage,
                VoteCount = wl.Movie.VoteCount,
                Status = wl.Movie.Status,
                ReleaseDate = wl.Movie.ReleaseDate,
                Revenue = wl.Movie.Revenue,
                Runtime = wl.Movie.Runtime,
                Adult = wl.Movie.Adult,
                BackdropPath = wl.Movie.BackdropPath,
                Budget = wl.Movie.Budget,
                Homepage = wl.Movie.Homepage,
                ImdbId = wl.Movie.ImdbId,
                OriginalLanguage = wl.Movie.OriginalLanguage,
                OriginalTitle = wl.Movie.OriginalTitle,
                Overview = wl.Movie.Overview,
                Popularity = wl.Movie.Popularity,
                PosterPath = wl.Movie.PosterPath,
                Tagline = wl.Movie.Tagline,
                Genres = wl.Movie.MovieGenres.Select(mg => mg.Genre.Name).ToList(),
                Keywords = wl.Movie.MovieKeywords.Select(mk => mk.Keyword.Name).ToList(),
                ProductionCompanies = wl.Movie.MovieProductionCompanies.Select(mpc => mpc.ProductionCompany.Name).ToList(),
                ProductionCountries = wl.Movie.MovieProductionCountries.Select(mpc => mpc.ProductionCountry.Name).ToList(),
                SpokenLanguages = wl.Movie.MovieSpokenLanguages.Select(msl => msl.SpokenLanguage.Name).ToList()
            })
            .FirstOrDefaultAsync();

            if (movieDetails is null)
            {
                throw new Exception("Filme não encontrado");
            }

            return movieDetails;
        }
        #endregion
        public async Task<List<MovieDetailsDTO>> GetRecommendedMovies(string id)
        {
            Random rnd = new Random();
            // Buscando ids dos clusters

            // Busca os ids dos UserGeneralProfileCluster, ou seja, dos clusters que se encaixam melhor no perfil inicial do usuário
            var generalClusterIds = await _context.UserGeneralProfileClusters.Where(ugpc => ugpc.UserId == id).Select(ugpc => ugpc.ClusterId).ToListAsync();

            // Busca o Id do último perfil de dia a dia do usuário
            var lastDailyProfileId = await _context.UserDailyProfiles.Where(udp => udp.IsTheLatest && udp.UserId == id).OrderByDescending(udp => udp.Timestamp).Select(udp => udp.Id).FirstOrDefaultAsync();

            // Busca os ids dos UserDailyProfileClusters, ou seja, dos clusters que se encaixam melhor no perfil respondido pela última vez pelo usuário
            var dailyClusterIds = await _context.UserDailyProfileClusters.Where(udpc => udpc.UserDailyProfileId == lastDailyProfileId).Select(udpc => udpc.ClusterId).ToListAsync();


            // Buscando parâmetros extras

            // Buscando parâmetros extras gerais para selecionar filmes dos clusters de perfil geral
            var generalExtraParameters = await
                _context
                    .Users
                    .Where(u => u.Id == id)
                    .Include(u => u.GeneralProfileProductionCountries)
                        .ThenInclude(ugpsl => ugpsl.ProductionCountry)
                    .Include(u => u.GeneralProfileSpokenLanguages)
                        .ThenInclude(ugpsl => ugpsl.SpokenLanguage)
                    .Select(u => new UserExtraParametersDTO
                    {
                        MinVoteAverage = u.MinVoteAverage,
                        MaxVoteAverage = u.MaxVoteAverage,
                        MinVoteCount = u.MinVoteCount,
                        MaxVoteCount = u.MaxVoteCount,
                        MinYear = u.MinYear,
                        MaxYear = u.MaxYear,
                        MinDuration = u.MinDuration,
                        MaxDuration = u.MaxDuration,
                        ProductionCountryIds = u.GeneralProfileProductionCountries.Select(ugppc => ugppc.ProductionCountry.Id).ToList(),
                        SpokenLanguageIds = u.GeneralProfileSpokenLanguages.Select(ugpsl => ugpsl.SpokenLanguage.Id).ToList(),
                    })
                    .FirstOrDefaultAsync();

            // Buscando parâmetros extras de dia a dia para selecionar filmes dos clusters de perfil de dia a dia
            var dailyExtraParameters = await
                _context
                    .UserDailyProfiles
                    .Where(udp => udp.Id == lastDailyProfileId)
                    .Include(udp => udp.UserDailyProfileProductionCountries)
                        .ThenInclude(udppc => udppc.ProductionCountry)
                    .Include(udp => udp.UserDailyProfileSpokenLanguages)
                        .ThenInclude(udpsl => udpsl.SpokenLanguage)
                    .Select(udp => new UserExtraParametersDTO
                    {
                        MinVoteAverage = udp.MinVoteAverage,
                        MaxVoteAverage = udp.MaxVoteAverage,
                        MinVoteCount = udp.MinVoteCount,
                        MaxVoteCount = udp.MaxVoteCount,
                        MinYear = udp.MinYear,
                        MaxYear = udp.MaxYear,
                        MinDuration = udp.MinDuration,
                        MaxDuration = udp.MaxDuration,
                        ProductionCountryIds = udp.UserDailyProfileProductionCountries.Select(udppc => udppc.ProductionCountry.Id).ToList(),
                        SpokenLanguageIds = udp.UserDailyProfileSpokenLanguages.Select(udpsl => udpsl.SpokenLanguage.Id).ToList(),
                    })
                    .FirstOrDefaultAsync();


            var usedMovieIds = new HashSet<int>();

            // Selecionando filmes com base nos clusters escolhidos e nos parâmetros extras do perfil de dia a dia do usuário
            var dailyThemeExtraMovies = await
                _context
                    .Movies
                    .Include(m => m.MovieProductionCountries)
                        .ThenInclude(mpc => mpc.ProductionCountry)
                    .Include(m => m.MovieSpokenLanguages)
                        .ThenInclude(mpc => mpc.SpokenLanguage)
                    .Include(m => m.MovieProductionCompanies)
                        .ThenInclude(mpc => mpc.ProductionCompany)
                    .Where(m =>
                        !usedMovieIds.Contains(m.Id) &&
                        !_context.WishLists.Any(w => w.UserId == id && w.MovieId == m.Id) &&
                        !_context.WatchedLists.Any(w => w.UserId == id && w.MovieId == m.Id) &&
                        dailyClusterIds.Contains(m.ClusterId) &&
                        m.VoteAverage > dailyExtraParameters.MinVoteAverage &&
                        m.VoteAverage < dailyExtraParameters.MaxVoteAverage &&
                        m.VoteCount > dailyExtraParameters.MinVoteCount &&
                        m.VoteCount < dailyExtraParameters.MaxVoteCount &&
                        m.ReleaseDate.Value.Year > dailyExtraParameters.MinYear &&
                        m.ReleaseDate.Value.Year < dailyExtraParameters.MaxYear &&
                        m.Runtime > dailyExtraParameters.MinDuration &&
                        m.Runtime < dailyExtraParameters.MaxDuration &&
                        m.MovieProductionCountries.Any(mpc => dailyExtraParameters.ProductionCountryIds.Contains(mpc.ProductionCountryId) &&
                        m.MovieSpokenLanguages.Any(mpc => dailyExtraParameters.SpokenLanguageIds.Contains(mpc.SpokenLanguageId))
                        ))
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
                    .OrderBy(x => Guid.NewGuid())
                    .Take(12)
                    .ToListAsync();

            usedMovieIds.UnionWith(dailyThemeExtraMovies.Select(m => m.Id));


            // Selecionando filmes com base nos clusters escolhidos e nos parâmetros extras do perfil geral do usuário
            var generalThemeExtraMovies = await
                _context
                    .Movies
                    .Include(m => m.MovieProductionCountries)
                        .ThenInclude(mpc => mpc.ProductionCountry)
                    .Include(m => m.MovieSpokenLanguages)
                        .ThenInclude(mpc => mpc.SpokenLanguage)
                    .Include(m => m.MovieProductionCompanies)
                        .ThenInclude(mpc => mpc.ProductionCompany)
                    .Where(m =>
                        !usedMovieIds.Contains(m.Id) &&
                        !_context.WishLists.Any(w => w.UserId == id && w.MovieId == m.Id) &&
                        !_context.WatchedLists.Any(w => w.UserId == id && w.MovieId == m.Id) &&
                        generalClusterIds.Contains(m.ClusterId) &&
                        m.VoteAverage > generalExtraParameters.MinVoteAverage &&
                        m.VoteAverage < generalExtraParameters.MaxVoteAverage &&
                        m.VoteCount > generalExtraParameters.MinVoteCount &&
                        m.VoteCount < generalExtraParameters.MaxVoteCount &&
                        m.ReleaseDate.Value.Year > generalExtraParameters.MinYear &&
                        m.ReleaseDate.Value.Year < generalExtraParameters.MaxYear &&
                        m.Runtime > generalExtraParameters.MinDuration &&
                        m.Runtime < generalExtraParameters.MaxDuration &&
                        m.MovieProductionCountries.Any(mpc => generalExtraParameters.ProductionCountryIds.Contains(mpc.ProductionCountryId) &&
                        m.MovieSpokenLanguages.Any(mpc => generalExtraParameters.SpokenLanguageIds.Contains(mpc.SpokenLanguageId))
                        ))
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
                    .OrderBy(x => Guid.NewGuid())
                    .Take(5)
                    .ToListAsync();

            usedMovieIds.UnionWith(generalThemeExtraMovies.Select(m => m.Id));


            // Selecionando filmes com base apenas nos clusters escolhidos do perfil de dia a dia do usuário
            var dailyThemeMovies = await
                _context
                    .Movies
                    .Include(m => m.MovieProductionCountries)
                        .ThenInclude(mpc => mpc.ProductionCountry)
                    .Include(m => m.MovieSpokenLanguages)
                        .ThenInclude(mpc => mpc.SpokenLanguage)
                    .Include(m => m.MovieProductionCompanies)
                        .ThenInclude(mpc => mpc.ProductionCompany)
                    .Where(m =>
                        !usedMovieIds.Contains(m.Id) &&
                        !_context.WishLists.Any(w => w.UserId == id && w.MovieId == m.Id) &&
                        !_context.WatchedLists.Any(w => w.UserId == id && w.MovieId == m.Id) &&
                        dailyClusterIds.Contains(m.ClusterId)
                        )
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
                    .OrderBy(x => Guid.NewGuid())
                    .Take(3)
                    .ToListAsync();

            usedMovieIds.UnionWith(dailyThemeMovies.Select(m => m.Id));


            // Selecionando filmes com base apenas nos parâmetros extras do perfil de dia a dia do usuário
            var dailyExtraMovies = await
                _context
                    .Movies
                    .Include(m => m.MovieProductionCountries)
                        .ThenInclude(mpc => mpc.ProductionCountry)
                    .Include(m => m.MovieSpokenLanguages)
                        .ThenInclude(mpc => mpc.SpokenLanguage)
                    .Include(m => m.MovieProductionCompanies)
                        .ThenInclude(mpc => mpc.ProductionCompany)
                    .Where(m =>
                        !usedMovieIds.Contains(m.Id) &&
                        !_context.WishLists.Any(w => w.UserId == id && w.MovieId == m.Id) &&
                        !_context.WatchedLists.Any(w => w.UserId == id && w.MovieId == m.Id) &&
                        m.VoteAverage > dailyExtraParameters.MinVoteAverage &&
                        m.VoteAverage < dailyExtraParameters.MaxVoteAverage &&
                        m.VoteCount > dailyExtraParameters.MinVoteCount &&
                        m.VoteCount < dailyExtraParameters.MaxVoteCount &&
                        m.ReleaseDate.Value.Year > dailyExtraParameters.MinYear &&
                        m.ReleaseDate.Value.Year < dailyExtraParameters.MaxYear &&
                        m.Runtime > dailyExtraParameters.MinDuration &&
                        m.Runtime < dailyExtraParameters.MaxDuration &&
                        m.MovieProductionCountries.Any(mpc => dailyExtraParameters.ProductionCountryIds.Contains(mpc.ProductionCountryId) &&
                        m.MovieSpokenLanguages.Any(mpc => dailyExtraParameters.SpokenLanguageIds.Contains(mpc.SpokenLanguageId))
                        ))
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
                    .OrderBy(x => Guid.NewGuid())
                    .Take(3)
                    .ToListAsync();

            usedMovieIds.UnionWith(dailyExtraMovies.Select(m => m.Id));


            // Selecionando filmes com base apenas nos clusters escolhidos do perfil geral do usuário
            var generalThemeMovies = await
                _context
                    .Movies
                    .Include(m => m.MovieProductionCountries)
                        .ThenInclude(mpc => mpc.ProductionCountry)
                    .Include(m => m.MovieSpokenLanguages)
                        .ThenInclude(mpc => mpc.SpokenLanguage)
                    .Include(m => m.MovieProductionCompanies)
                        .ThenInclude(mpc => mpc.ProductionCompany)
                    .Where(m =>
                        !usedMovieIds.Contains(m.Id) &&
                        !_context.WishLists.Any(w => w.UserId == id && w.MovieId == m.Id) &&
                        !_context.WatchedLists.Any(w => w.UserId == id && w.MovieId == m.Id) &&
                        generalClusterIds.Contains(m.ClusterId)
                        )
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
                    .OrderBy(x => Guid.NewGuid())
                    .Take(2)
                    .ToListAsync();

            usedMovieIds.UnionWith(generalThemeMovies.Select(m => m.Id));


            // Selecionando filmes com base apenas nos parâmetros extras do perfil geral do usuário
            var generalExtraMovies = await
                _context
                    .Movies
                    .Include(m => m.MovieProductionCountries)
                        .ThenInclude(mpc => mpc.ProductionCountry)
                    .Include(m => m.MovieSpokenLanguages)
                        .ThenInclude(mpc => mpc.SpokenLanguage)
                    .Include(m => m.MovieProductionCompanies)
                        .ThenInclude(mpc => mpc.ProductionCompany)
                    .Where(m =>
                        !usedMovieIds.Contains(m.Id) &&
                        !_context.WishLists.Any(w => w.UserId == id && w.MovieId == m.Id) &&
                        !_context.WatchedLists.Any(w => w.UserId == id && w.MovieId == m.Id) &&
                        m.VoteAverage > generalExtraParameters.MinVoteAverage &&
                        m.VoteAverage < generalExtraParameters.MaxVoteAverage &&
                        m.VoteCount > generalExtraParameters.MinVoteCount &&
                        m.VoteCount < generalExtraParameters.MaxVoteCount &&
                        m.ReleaseDate.Value.Year > generalExtraParameters.MinYear &&
                        m.ReleaseDate.Value.Year < generalExtraParameters.MaxYear &&
                        m.Runtime > generalExtraParameters.MinDuration &&
                        m.Runtime < generalExtraParameters.MaxDuration &&
                        m.MovieProductionCountries.Any(mpc => generalExtraParameters.ProductionCountryIds.Contains(mpc.ProductionCountryId) &&
                        m.MovieSpokenLanguages.Any(mpc => generalExtraParameters.SpokenLanguageIds.Contains(mpc.SpokenLanguageId))
                        ))
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
                    .OrderBy(x => Guid.NewGuid())
                    .Take(1)
                    .ToListAsync();

            usedMovieIds.UnionWith(generalExtraMovies.Select(m => m.Id));


            // Selecionando filmes totalmente aleatórios
            var randomMovies = await
                _context
                    .Movies
                    .Include(m => m.MovieProductionCountries)
                        .ThenInclude(mpc => mpc.ProductionCountry)
                    .Include(m => m.MovieSpokenLanguages)
                        .ThenInclude(mpc => mpc.SpokenLanguage)
                    .Include(m => m.MovieProductionCompanies)
                        .ThenInclude(mpc => mpc.ProductionCompany)
                    .Where(m =>
                        !usedMovieIds.Contains(m.Id) &&
                        !_context.WishLists.Any(w => w.UserId == id && w.MovieId == m.Id) &&
                        !_context.WatchedLists.Any(w => w.UserId == id && w.MovieId == m.Id))
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
                    .OrderBy(x => Guid.NewGuid())
                    .Take(2)
                    .ToListAsync();

            usedMovieIds.UnionWith(randomMovies.Select(m => m.Id));

            var extraMovies = new List<MovieDetailsDTO>();
            if (usedMovieIds.Count < 25)
            {
                var moreDailyThemeExtraMovies = await
                _context
                    .Movies
                    .Include(m => m.MovieProductionCountries)
                        .ThenInclude(mpc => mpc.ProductionCountry)
                    .Include(m => m.MovieSpokenLanguages)
                        .ThenInclude(mpc => mpc.SpokenLanguage)
                    .Include(m => m.MovieProductionCompanies)
                        .ThenInclude(mpc => mpc.ProductionCompany)
                    .Where(m =>
                        !usedMovieIds.Contains(m.Id) &&
                        !_context.WishLists.Any(w => w.UserId == id && w.MovieId == m.Id) &&
                        !_context.WatchedLists.Any(w => w.UserId == id && w.MovieId == m.Id) &&
                        dailyClusterIds.Contains(m.ClusterId) &&
                        m.VoteAverage > dailyExtraParameters.MinVoteAverage &&
                        m.VoteAverage < dailyExtraParameters.MaxVoteAverage &&
                        m.VoteCount > dailyExtraParameters.MinVoteCount &&
                        m.VoteCount < dailyExtraParameters.MaxVoteCount &&
                        m.ReleaseDate.Value.Year > dailyExtraParameters.MinYear &&
                        m.ReleaseDate.Value.Year < dailyExtraParameters.MaxYear &&
                        m.Runtime > dailyExtraParameters.MinDuration &&
                        m.Runtime < dailyExtraParameters.MaxDuration &&
                        m.MovieProductionCountries.Any(mpc => dailyExtraParameters.ProductionCountryIds.Contains(mpc.ProductionCountryId) &&
                        m.MovieSpokenLanguages.Any(mpc => dailyExtraParameters.SpokenLanguageIds.Contains(mpc.SpokenLanguageId))
                        ))
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
                    .OrderBy(x => Guid.NewGuid())
                    .Take(5)
                    .ToListAsync();

                usedMovieIds.UnionWith(moreDailyThemeExtraMovies.Select(m => m.Id));

                var moreDailyThemeMovies = await
                _context
                    .Movies
                    .Include(m => m.MovieProductionCountries)
                        .ThenInclude(mpc => mpc.ProductionCountry)
                    .Include(m => m.MovieSpokenLanguages)
                        .ThenInclude(mpc => mpc.SpokenLanguage)
                    .Include(m => m.MovieProductionCompanies)
                        .ThenInclude(mpc => mpc.ProductionCompany)
                    .Where(m =>
                        !usedMovieIds.Contains(m.Id) &&
                        !_context.WishLists.Any(w => w.UserId == id && w.MovieId == m.Id) &&
                        !_context.WatchedLists.Any(w => w.UserId == id && w.MovieId == m.Id) &&
                        dailyClusterIds.Contains(m.ClusterId)
                        )
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
                    .OrderBy(x => Guid.NewGuid())
                    .Take(5)
                    .ToListAsync();

                usedMovieIds.UnionWith(moreDailyThemeMovies.Select(m => m.Id));

                var moreDailyExtraMovies = await
                _context
                    .Movies
                    .Include(m => m.MovieProductionCountries)
                        .ThenInclude(mpc => mpc.ProductionCountry)
                    .Include(m => m.MovieSpokenLanguages)
                        .ThenInclude(mpc => mpc.SpokenLanguage)
                    .Include(m => m.MovieProductionCompanies)
                        .ThenInclude(mpc => mpc.ProductionCompany)
                    .Where(m =>
                        !usedMovieIds.Contains(m.Id) &&
                        !_context.WishLists.Any(w => w.UserId == id && w.MovieId == m.Id) &&
                        !_context.WatchedLists.Any(w => w.UserId == id && w.MovieId == m.Id) &&
                        m.VoteAverage > dailyExtraParameters.MinVoteAverage &&
                        m.VoteAverage < dailyExtraParameters.MaxVoteAverage &&
                        m.VoteCount > dailyExtraParameters.MinVoteCount &&
                        m.VoteCount < dailyExtraParameters.MaxVoteCount &&
                        m.ReleaseDate.Value.Year > dailyExtraParameters.MinYear &&
                        m.ReleaseDate.Value.Year < dailyExtraParameters.MaxYear &&
                        m.Runtime > dailyExtraParameters.MinDuration &&
                        m.Runtime < dailyExtraParameters.MaxDuration &&
                        m.MovieProductionCountries.Any(mpc => dailyExtraParameters.ProductionCountryIds.Contains(mpc.ProductionCountryId) &&
                        m.MovieSpokenLanguages.Any(mpc => dailyExtraParameters.SpokenLanguageIds.Contains(mpc.SpokenLanguageId))
                        ))
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
                    .OrderBy(x => Guid.NewGuid())
                    .Take(5)
                    .ToListAsync();

                usedMovieIds.UnionWith(moreDailyExtraMovies.Select(m => m.Id));

                extraMovies = moreDailyThemeExtraMovies
                    .Concat(moreDailyThemeMovies)
                    .Concat(moreDailyExtraMovies)
                    .DistinctBy(m => m.Id)
                    .ToList();
            }

            var recommendedMovies = generalThemeExtraMovies
                .Concat(generalThemeMovies)
                .Concat(generalExtraMovies)
                .Concat(randomMovies)
                .Concat(dailyThemeExtraMovies)
                .Concat(dailyThemeMovies)
                .Concat(dailyExtraMovies)
                .Concat(extraMovies)
                .DistinctBy(m => m.Id)
                .OrderBy(x => Guid.NewGuid())
                .ToList();

            return recommendedMovies;

        }

        internal async Task PostChosenRecommendedMovies(string id, List<UserWatchedOrLikedMovieDTO> movieIds)
        {
            HashSet<Movie> movies = new HashSet<Movie>();
            foreach (var movie in movieIds)
            {
                var movieEntity = await _context.Movies.AsNoTracking().FirstOrDefaultAsync(m => m.Id == movie.MovieId);
                if (movieEntity is null)
                {
                    throw new Exception($"Filme com ID {movie.MovieId} não encontrado.");
                }

                if (movie.Watched)
                {
                    _context.WatchedLists.Add(new WatchedList
                    {
                        UserId = id,
                        MovieId = movie.MovieId
                    });
                }
                else if (movie.Liked)
                {
                    _context.WishLists.Add(new WishList
                    {
                        UserId = id,
                        MovieId = movie.MovieId,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
