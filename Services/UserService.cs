using LumeServer.Data;
using LumeServer.Models.User;

namespace LumeServer.Services
{
    // No controller, você manda esse UserService mexer no banco com seus métodos respectivos
    public class UserService
    {
        // Aqui vamos implementar o UserService, que vai ser responsável por fazer a comunicação com o banco de dados.
        // Ele vai ter métodos para criar, ler, atualizar e deletar usuários.
        // Ele vai usar o LumeDataContext para fazer isso.
        // O LumeDataContext é o contexto do banco de dados, ele é responsável por fazer a comunicação com o banco de dados.
        // Aqui vamos criar o construtor do UserService, que vai receber o LumeDataContext como parâmetro.
        private readonly UserDataContext _context;
        public UserService(UserDataContext context)
        {
            _context = context;
        }

        // Exemplo de método que manipula o banco de dados
        public List<User> GetAllUsers()
        {
            // Aqui vamos fazer uma consulta no banco de dados para pegar todos os usuários.
            // _context é a representação do banco
            // Users é a representação da tabela
            // ToList é para converter o resultado em uma lista
            var users = _context.Users.ToList();
            // Aqui vamos transformar a lista de usuários em uma string e retornar.
            return users;
        }
    }
}
