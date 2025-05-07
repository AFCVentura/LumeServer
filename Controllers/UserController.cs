using LumeServer.Models.User;
using LumeServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LumeServer.Controllers
{
    // Essa é a classe Controller, ela é responsável por receber as requisições e retornar as respostas.
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _service.LogoutAsync();
            return Ok(new { message = "Logout efetuado com sucesso." });
        }
    }
}
