using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace JustShop2.Models.Accounts
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name ="Current password")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required]
        [DataType (DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not mach")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
