using JustShop2.Core.Dto;
using MimeKit;

namespace JustShop2.Core.ServiceInterface
{
    public interface IEmailsServices
    {
        void SendEmail(EmailDto dto);
        void SendEmail(MimeMessage message);
    }
}
