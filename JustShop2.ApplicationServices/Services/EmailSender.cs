using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace JustShop2.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly string _smtpServer = "smtp.gmail.com"; // Asenda enda SMTP serveriga
        private readonly int _smtpPort = 587; // SMTP port (587 TLS või 465 SSL)
        private readonly string _fromEmail = "raul.kaskpeit@gmail.com"; // Sinu e-mail
        private readonly string _fromPassword = "tlfr xegw mato trno"; // Sinu e-maili salasõna

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            using (var client = new SmtpClient(_smtpServer, _smtpPort))
            {
                client.Credentials = new NetworkCredential(_fromEmail, _fromPassword);
                client.EnableSsl = true; // Kasuta SSL/TLS ühendust

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_fromEmail),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true, // HTML-sisu lubamine
                };

                mailMessage.To.Add(email); // Lisame sihtkoha aadressi

                try
                {
                    await client.SendMailAsync(mailMessage);
                }
                catch (Exception ex)
                {
                    // Logi viga, kui e-maili saatmine ebaõnnestub
                    Console.WriteLine($"Email sending failed: {ex.Message}");
                    throw;
                }
            }
        }
    }
}