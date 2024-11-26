using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace JustShop2.Models.Emails
{
    public class EmailsViewModel
    {
        public string To { get; set; } = string.Empty;      // Saaja e-post
        public string Subject { get; set; } = string.Empty;  // E-kirja teema
        public string Body { get; set; } = string.Empty;     // E-kirja sisu
        public IEnumerable<IFormFile> Attachments { get; set; }  // Mitme faili lisamine
    }
}
