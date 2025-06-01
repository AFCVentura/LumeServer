using LumeServer.DTOs;
using LumeServer.Models.Movie;
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
        public async Task<ActionResult<List<CarouselPreviewWishListDTO>>> GetCarouselMovies([FromRoute] string id)
        {
            try
            {
                return Ok(await _service.FindCarouselMovies(id, 10));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}. Exceção interna: {ex.InnerException}");
                return StatusCode(500, "Erro ao processar a solicitação. Por favor, tente novamente mais tarde.");
            }
        }

        // Método que puxa todas as capas e títulos de filmes da wishlist do usuário para aparecer na tela de wishlist
        [HttpGet("wishlist-movies/{id}")]
        public async Task<ActionResult<List<CarouselPreviewWishListDTO>>> GetWishListMovies([FromRoute] string id)
        {
            try
            {
                return Ok(await _service.FindAllWishListMovies(id));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}. Exceção interna: {ex.InnerException}");
                return StatusCode(500, "Erro ao processar a solicitação. Por favor, tente novamente mais tarde.");
            }
        }

        // Método que puxa todos os detalhes de um filme específico da wishlist do usuário
        [HttpGet("wishlist-movies/{userId}/wishlisted-movies/{movieId}")]
        public async Task<ActionResult<MovieDetailsDTO>> GetWishListMovieDetails([FromRoute] string userId, [FromRoute] int movieId)
        {
            try
            {
                return Ok(await _service.FindWishListMovieById(userId, movieId));
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
