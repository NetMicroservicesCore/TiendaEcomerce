using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Security.Claims;
using TiendaEcomerce.Models;

namespace TiendaEcomerce.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser>? _userManager;
        private readonly SignInManager<ApplicationUser>? 
            _signInManager;
        private readonly IEmailSender? _emailSender;
        private readonly RoleManager<IdentityRole>? _roleManager;

        public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender,
        RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        #region Registro de Usuarios

        [HttpGet]
        public IActionResult Registro() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new ApplicationUser { UserName = model.Email, 
                Email = model.Email, FirstName = model.FirstName, 
                LastName = model.LastName };
            var result = await _userManager!.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // enviar email confirmación
                var token = await 
                    _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmLink = Url.Action(nameof(ConfirmEmail), "Account", 
                    new { userId = user.Id, token }, Request.Scheme);
                await _emailSender!.SendEmailAsync(user.Email,"Confirm your"+
                    "email", $"Por favor confirma tu cuenta " +
                    $"<a href=\"{confirmLink}\">aquí</a>.");
                // opcional: asignar rol default
                await _userManager.AddToRoleAsync(user, "User");
                return RedirectToAction("RegisterConfirmation");
            }
            foreach (var e in result.Errors) ModelState.AddModelError("",
                e.Description);
            return View(model);
        }

        #endregion
        #region Login de Acceso a Usuarios
        [HttpGet]
        public IActionResult Login(string returnUrl = null) =>
            View(new LoginViewModel { ReturnUrl = returnUrl });

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            // lockoutOnFailure = true
            var result = await _signInManager!.PasswordSignInAsync(model.Email,
                model.Password, model.RememberMe, lockoutOnFailure: true);
            if (result.Succeeded)
                return LocalRedirect(model.ReturnUrl ?? "/");
            if (result.RequiresTwoFactor)
                return RedirectToAction(nameof(LoginWith2fa), new { model.ReturnUrl, model.RememberMe });
            if (result.IsLockedOut) return View("Lockout");
            ModelState.AddModelError("", "Intento de login inválido.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager!.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region  Confirmacion de Correo

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null) return RedirectToAction("Index", "Home");
            var user = await _userManager!.FindByIdAsync(userId);
            if (user == null) return NotFound();
            var result = await _userManager.ConfirmEmailAsync(user, token);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }
        #endregion


        #region External Login
        // External login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager!.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null) return RedirectToAction(nameof(Login));

            var info = await _signInManager!.GetExternalLoginInfoAsync();
            if (info == null) return RedirectToAction(nameof(Login));

            // Sign in the user with this external login provider if the user already has a login.
            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (signInResult.Succeeded) return LocalRedirect(returnUrl ?? "/");

            // If the user does not have an account, then ask the user to create an account.
            var email = info.Principal.FindFirstValue(System.Security.Claims.ClaimTypes.Email);
            var user = new ApplicationUser { UserName = email, Email = email, EmailConfirmed = true };
            var result = await _userManager!.CreateAsync(user);
            if (result.Succeeded)
            {
                result = await _userManager.AddLoginAsync(user, info);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl ?? "/");
                }
            }
            return RedirectToAction(nameof(Login));
        }


        #endregion

    }
}
