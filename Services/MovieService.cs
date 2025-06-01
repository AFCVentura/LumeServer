using LumeServer.Data;
using LumeServer.DTOs;
using LumeServer.DTOs.LumeAI;
using LumeServer.Models.Movie;
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
    }
}
