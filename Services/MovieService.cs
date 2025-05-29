using LumeServer.Data;
using LumeServer.DTOs;
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
            // DEPOIS DE ADICIONAR COLUNA DE TIMESTAMP NA TABELA WISHLISTS, ORDENAR POR DATA E PEGAR SÓ RECENTES
            return await _context.WishLists
                .Where(wl => wl.UserId == id)
                .Include(wl => wl.Movie)
                .Select(wl => new CarouselPreviewWishListDTO
                {
                    Id = wl.Movie.Id,
                    Title = wl.Movie.Title,
                    PosterPath = wl.Movie.PosterPath
                })
                .Take(amount).ToListAsync();
        }
    }
}
