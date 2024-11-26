using Microsoft.AspNetCore.Http;
using System.Collections.Generic;


namespace JustShop2.Core.Dto
{
    public class EmailDto
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public IEnumerable<IFormFile> Attachments { get; set; }  // Mitme faili tugi
    }
}
