using Azure.Identity;
using JustShop2.Core.Domain;
using JustShop2.Models.Accounts;
using JustShop2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;

namespace JustShop2.Controllers
{
    public class AccountsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly IEmailSender _emailSender;

        public AccountsController(UserManager<ApplicationUser> userManager,
                                  SignInManager<ApplicationUser> signInManager,
                                  IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender; // Kasutame siin tegelikku EmailSender teenust
        }

        public ApplicationUser user { get; private set; }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if(ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = vm.Email,
                    Name = vm.Name,
                    Email = vm.Email,
                    City = vm.City,
                };
                var result = await _userManager.CreateAsync(user, vm.Password);

                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action("ConfirmEmail", "Accounts", new {userId =user.Id, token = token}, Request.Scheme);

                    if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers", "Administrations");
                    }

                    ViewBag.ErrorTittle = "Registration succesful";
                    ViewBag.ErrorMessage = "Before you can Login, please confirm you " +
                        "email, by clicking on the confirmation link we have emailed you";

                    return View("EmailError");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string? returnUrl)
        {
            LoginViewModel vm = new()
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList(),

            };

            return View(vm);
        }

        [HttpPost]
        [AllowAnonymous]

        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
        {
            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null && !user.EmailConfirmed &&
                    (await _userManager.CheckPasswordAsync(user, model.Password)))
                {
                    ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                if (result.IsLockedOut)
                {
                    return View("AccountLocked");
                }

                ModelState.AddModelError("", "Invalid Login Attempt");
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"The User ID {userId} is not valid";
                return View("NotFound");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View();
            }

            ViewBag.ErrorTitle = "Email cannot be confirmed";
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Leia kasutaja andmebaasist
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null && await _userManager.IsEmailConfirmedAsync(user))
                {
                    // Genereeri parooli reset token
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    // Loo reset link
                    var resetLink = Url.Action("ResetPassword", "Accounts", new { token, email = model.Email }, Request.Scheme);

                    // Saada e-mail kasutajale
                    await _emailSender.SendEmailAsync(model.Email, "Reset Your Password",
                        $"Please reset your password by clicking <a href='{resetLink}'>here</a>.");

                    // Tagasta kinnituse vaade
                    return View("ForgotPasswordConfirmation");
                }

                // Ära anna vihjet, et kasutajat ei leitud
                return View("ForgotPasswordConfirmation");
            }

            // Kui sisend on vigane, tagasta sama vaade
            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid password reset token.");
            }
            return View(new ResetPasswordViewModel { Token = token, Email = email });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Leia kasutaja
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            // Proovi parooli resettida
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            // Kuva vead
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        { 
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                if (user != null)
                {
                    return RedirectToAction("Login");
                }

                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                if(!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    return View();
                }

                await _signInManager.RefreshSignInAsync(user);
                return View("ChangePasswordConfirmation");
            
            }
            return View(model);
        }

       // private class _emailSender
       
        
        
        //{
         //   internal static async Task SendEmailAsync(string email, string v1, string v2)
           // {
              //  throw new NotImplementedException();
            //}
        //}
    }
}
