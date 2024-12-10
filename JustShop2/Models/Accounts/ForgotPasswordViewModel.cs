using System.ComponentModel.DataAnnotations;

namespace JustShop2.Models.Accounts
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]

        public string Email { get; set; }
    }
}
