
            //otsida [les config asukoht ja sinna sisestada muutujad
            //"EmailHost" : "smtp.gmail.com,
            //"EmailUserName" : "inde9999@gmail.com",
            //"EmailPassword" : "teie salas]na"


using MimeKit;
using MailKit.Net.Smtp;
using JustShop2.Core.ServiceInterface;
using JustShop2.Core.Dto;
using Microsoft.Extensions.Configuration;

namespace JustShop2.ApplicationServices.Services
{
    public class EmailServices : IEmailsServices
    {
        private readonly IConfiguration _configuration;

        public EmailServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Meetod, mis implementeerib MimeMessage tüüpi saatmist
        public void SendEmail(MimeMessage message)
        {
            using var smtp = new SmtpClient();
            smtp.Connect(
                _configuration["EmailSettings:EmailHost"],
                int.Parse(_configuration["EmailSettings:EmailPort"]),
                MailKit.Security.SecureSocketOptions.StartTls
            );

            smtp.Authenticate(
                _configuration["EmailSettings:EmailUserName"],
                _configuration["EmailSettings:EmailPassword"]
            );

            smtp.Send(message);
            smtp.Disconnect(true);
        }

        // Meetod EmailDto tüüpi andmete saatmiseks
        public void SendEmail(EmailDto dto)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration["EmailSettings:EmailUserName"]));
            email.To.Add(MailboxAddress.Parse(dto.To));
            email.Subject = dto.Subject;

            var bodyBuilder = new BodyBuilder { TextBody = dto.Body };

            if (dto.Attachments != null)
            {
                foreach (var file in dto.Attachments)
                {
                    using var stream = file.OpenReadStream();
                    bodyBuilder.Attachments.Add(file.FileName, stream);
                }
            }

            email.Body = bodyBuilder.ToMessageBody();
            SendEmail(email);  // Kutsub MimeMessage meetodit
        }
    }
}
