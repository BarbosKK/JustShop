using Microsoft.AspNetCore.Mvc;
using JustShop2.Core.Dto;
using JustShop2.Core.ServiceInterface;
using JustShop2.Models.Emails;
using MimeKit;
using MailKit.Net.Smtp;

namespace JustShop2.Controllers
{
    public class EmailsController : Controller
    {
        private readonly IEmailsServices _emailServices;

        // Konstruktoris süstime EmailServices
        public EmailsController(IEmailsServices emailServices)
        {
            _emailServices = emailServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Meetod e-kirja saatmiseks

        [HttpPost]
        public IActionResult SendEmail(EmailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Loo MimeMessage objekt
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Sinu Nimi", "sinu@email.com"));
                message.To.Add(new MailboxAddress("", model.To));
                message.Subject = model.Subject;

                // Lisa e-kirja sisu ja manused
                var bodyBuilder = new BodyBuilder { TextBody = model.Body };

                if (model.Attachments != null && model.Attachments.Any())
                {
                    foreach (var file in model.Attachments)
                    {
                        using var stream = file.OpenReadStream();
                        bodyBuilder.Attachments.Add(file.FileName, stream);
                    }
                }

                message.Body = bodyBuilder.ToMessageBody();

                // Kasuta e-posti teenust
                _emailServices.SendEmail(message);

                ViewBag.Message = "E-kiri saadetud edukalt!";
            }
            else
            {
                ViewBag.Message = "E-kirja saatmine ebaõnnestus.";
            }

            return View("Index");
        }
    }
}
