using LumeServer.DTOs;
using LumeServer.Models.Question;
using LumeServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace LumeServer.Controllers
{
    [ApiController]
    [Route("api/v1/movies")]
    public class MovieController : ControllerBase
    {
        private MovieService _service;

        public MovieController(MovieService service)
        {
            _service = service;
        }

        #region Métodos da wishlist
        // Método que puxa 10 capas e títulos de filmes da wishlist do usuário para aparecer no carrossel
        [HttpGet("carousel-movies/{id}")]
        public async Task<List<CarouselPreviewWishListDTO>> GetCarouselMovies([FromRoute] string id)
        {
            return await _service.FindCarouselMovies(id, 10);
        }
        #endregion
    }
}
