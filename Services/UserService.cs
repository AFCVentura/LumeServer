using LumeServer.Data;
using LumeServer.EmailSender;
using LumeServer.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
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
        private readonly IEmailSender _emailSender;

        public UserService(LumeDataContext context, SignInManager<User> signInManager, UserManager<User> userManager, IEmailSender emailSender)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
        }

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

        // Forgot Password
        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return false;

            // Gera o token numérico de 6 dígitos
            var random = new Random();
            var token = random.Next(100000, 999999).ToString();

            // Salva o token numérico no banco 
            await _context.PasswordResetTokens.AddAsync(new PasswordResetToken
            {
                Email = email,
                Token = token,
                Expiration = DateTime.UtcNow.AddMinutes(10)  // Token expira em 10 minutos
            });
            await _context.SaveChangesAsync();

            // Envia o token por e-mail
            await _emailSender.SendEmailAsync(
                email,
                "Código de Redefinição de Senha - LUME",
                $"Seu código de redefinição de senha é: <b>{token}</b>"
            );

            return true;
        }


        // Reset Password
        public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
        {
            // Verifica se o token existe, é do email certo e ainda está válido
            var tokenEntry = await _context.PasswordResetTokens
                .FirstOrDefaultAsync(t => t.Email == email && t.Token == token && t.Expiration > DateTime.UtcNow);

            if (tokenEntry == null)
                return false;

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return false;

            // Remove a senha antiga só se existir
            if (await _userManager.HasPasswordAsync(user))
            {
                var removePasswordResult = await _userManager.RemovePasswordAsync(user);
                if (!removePasswordResult.Succeeded)
                    return false; // Falha ao remover a senha, aborta
            }

            // Adiciona a nova senha
            var addPasswordResult = await _userManager.AddPasswordAsync(user, newPassword);
            if (!addPasswordResult.Succeeded)
                return false; // Falha ao adicionar a nova senha

            // Apaga o token depois de usar
            _context.PasswordResetTokens.Remove(tokenEntry);
            await _context.SaveChangesAsync();

            return true;
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


        public async Task<User?> GetUserByClaimsAsync(ClaimsPrincipal userClaims)
        {
            return await _userManager.GetUserAsync(userClaims);
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
