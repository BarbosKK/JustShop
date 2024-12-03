using Microsoft.AspNetCore.Identity;
namespace JustShop2.Core.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public required string City { get; set; }
    }
}
