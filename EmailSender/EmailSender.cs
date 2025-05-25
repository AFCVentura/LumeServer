using System.Net;
using System.Net.Mail;

namespace LumeServer.EmailSender
{
    public class EmailSender : IEmailSender
    {
        private readonly string _email;
        private readonly string _password;

        public EmailSender(IConfiguration config)
        {
            _email = config["Email:Address"]!;
            _password = config["Email:Password"]!;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            Console.WriteLine($"Enviando e-mail para: {email}");

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_email, _password)
            };

            var mail = new MailMessage(_email, email, subject, htmlMessage)
            {
                IsBodyHtml = true
            };

            await client.SendMailAsync(mail);
        }
    }
}
