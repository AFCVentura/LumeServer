using LumeServer.Data;
using LumeServer.Models.Question;
using LumeServer.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
        private readonly LumeDataContext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public UserService(LumeDataContext context, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        #region Métodos para as perguntas e respostas
        // Enviar perguntas extras de perfil geral
        // Eager: Carrega as alternativas também
        public async Task<List<ExtraQuestion>> FindAllGeneralExtraQuestionsEagerAsync()
        {
            return await _context.ExtraQuestions.Where(eq => eq.IsGeneralProfile).Include(eq => eq.Answers).ToListAsync();
        }

        // Enviar perguntas de tema de perfil geral
        // Eager: Carrega as alternativas também
        public async Task<List<ThemeQuestion>> FindAllGeneralThemeQuestionsEagerAsync()
        {
            return await _context.ThemeQuestions.Where(eq => eq.IsGeneralProfile).Include(tq => tq.ThemeAnswers).ToListAsync();
        }



        // Enviar perguntas extras de perfil do momento
        // Eager: Carrega as alternativas também
        public async Task<List<ExtraQuestion>> FindAllDailyExtraQuestionsEagerAsync()
        {
            return await _context.ExtraQuestions.Where(eq => !eq.IsGeneralProfile).Include(eq => eq.Answers).ToListAsync();
        }

        // Enviar perguntas de tema de perfil do momento
        // Eager: Carrega as alternativas também
        public async Task<List<ThemeQuestion>> FindAllDailyThemeQuestionsEagerAsync()
        {
            return await _context.ThemeQuestions.Where(eq => !eq.IsGeneralProfile).Include(tq => tq.ThemeAnswers).ToListAsync();
        }
        #endregion

        // Logout
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        // Change DisplayName
        public async Task<bool> ChangeDisplayNameAsync(ClaimsPrincipal userClaims, string newDisplayName)
        {
            var user = await GetUserByClaimsAsync(userClaims);
            if (user == null)
                return false;

            user.DisplayName = newDisplayName;
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }

        // Change Password
        public async Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        // Delete Accounts
        public async Task<bool> DeleteAccountAsync(ClaimsPrincipal userPrincipal)
        {
            var user = await _userManager.GetUserAsync(userPrincipal);

            if (user == null)
                return false;

            var result = await _userManager.DeleteAsync(user);
            await _signInManager.SignOutAsync();
            return result.Succeeded;
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


        public async Task<User?> GetUserByClaimsAsync(ClaimsPrincipal userClaims)
        {
            return await _userManager.GetUserAsync(userClaims);
        }

    }
}
