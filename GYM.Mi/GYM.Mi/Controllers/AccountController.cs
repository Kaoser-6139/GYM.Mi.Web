using GYM.Mi.Infrastructure.Identity;
using GYM.Mi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using GYM.Domain.Services;

namespace GYM.Mi.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IUserService _userService;
       //private readonly IEmailUtility _emailUtility;
        //private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
           // IEmailUtility emailUtility,
            //ITokenService tokenService,
            IConfiguration configuration,
            IUserService userService)
        {
            _userManager = userManager;
            _userStore = userStore;
           _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
           // _emailUtility = emailUtility;
            //_tokenService = tokenService;
            _configuration = configuration;
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAsync(string returnUrl = null)
        {
            var model = new RegisterModel();

            model.ReturnUrl = returnUrl;
            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            return View(model);
        }

        [AllowAnonymous, HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsync(RegisterModel model)
        {
            model.ReturnUrl ??= Url.Content("~/");
            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, model.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, model.Email, CancellationToken.None);

                user.RegistrationDate = DateTime.UtcNow;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;

                var result = await _userManager.CreateAsync(user, model.Password);

                await _userManager.AddToRoleAsync(user, "User");

               

                if (result.Succeeded)
                {
                    //++++++++++++++++++++++++
                    var profileUser = new GYM.Domain.Entities.User
                    {
                        Id = user.Id, //  From Identity 
                        FullName = $"{model.FirstName} {model.LastName}",
                        EntryDate = DateTime.UtcNow,
                       
                    };
                    _userService.AddUser(profileUser);




                    //++++++++++++++++++++++++++==

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = model.ReturnUrl },
                        protocol: Request.Scheme);


                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = model.Email, returnUrl = model.ReturnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);

                        return RedirectToAction("Welcome", "Users", new { area = "Admin", id = user.Id });

                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }


        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync(string returnUrl = null)
        {
            var model = new LoginModel();
            if (!string.IsNullOrEmpty(model.ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, model.ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            model.ReturnUrl = returnUrl;

            return View(model);
        }

        //[AllowAnonymous, HttpPost, ValidateAntiForgeryToken]
        //public async Task<IActionResult> LoginAsync(LoginModel model)
        //{
        //    model.ReturnUrl ??= Url.Content("~/");
        //    model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        //    if (ModelState.IsValid)
        //    {
        //        var result = await _signInManager.PasswordSignInAsync(
        //            model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

        //        if (result.Succeeded)
        //        {
        //            var appUser = await _userManager.FindByEmailAsync(model.Email);
        //            if (appUser == null)
        //            {
        //                ModelState.AddModelError(string.Empty, "User not found.");
        //                return View(model);
        //            }

        //            var roles = await _userManager.GetRolesAsync(appUser);

        //            // Admin, HR, Manager - Dashboard
        //            if (roles.Contains("Admin") || roles.Contains("HR") || roles.Contains("Manager"))
        //            {
        //                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        //            }

        //            // User role handling
        //            if (roles.Contains("User"))
        //            {

        //                var userProfile = _userService.GetUser(appUser.Id);

        //                if (userProfile == null)
        //                {
        //                    ModelState.AddModelError(string.Empty, "User profile not found.");
        //                    return View(model);
        //                }


        //                if (userProfile.EntryDate != DateTime.MinValue)
        //                {
        //                    // Profile completed → Profile page
        //                    return RedirectToAction("Profile", "Users", new { area = "Admin", id = userProfile.Id });
        //                }
        //                else
        //                {
        //                    // Info incomplete → Welcome page
        //                    return RedirectToAction("Welcome", "Users", new { area = "Admin", id = userProfile.Id });
        //                }
        //            }


        //            return LocalRedirect(model.ReturnUrl);
        //        }


        //        if (result.RequiresTwoFactor)
        //        {
        //            return RedirectToPage("./LoginWith2fa", new
        //            {
        //                ReturnUrl = model.ReturnUrl,
        //                RememberMe = model.RememberMe
        //            });
        //        }
        //        if (result.IsLockedOut)
        //        {
        //            _logger.LogWarning("User account locked out.");
        //            return RedirectToPage("./Lockout");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        //            return View(model);
        //        }
        //    }

        //    return View(model);
        //}

        [AllowAnonymous, HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync(LoginModel model)
        {
            model.ReturnUrl ??= Url.Content("~/");
            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var appUser = await _userManager.FindByEmailAsync(model.Email);
                    if (appUser == null)
                    {
                        ModelState.AddModelError(string.Empty, "User not found.");
                        return View(model);
                    }

                    var roles = await _userManager.GetRolesAsync(appUser);
                    
                    if (roles.Contains("Admin") || roles.Contains("Trainer") || roles.Contains("Manager"))
                    {
                        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                    }

                    if (roles.Contains("User"))
                    {
                     
                        var userProfile = _userService.GetUser(appUser.Id);

                        
                        if (userProfile == null)
                        {
                            return RedirectToAction("Welcome", "Users", new { area = "Admin", id = appUser.Id });
                        }

                        if (userProfile.EntryDate != DateTime.MinValue)
                        {
                            
                            return RedirectToAction("Profile", "Users", new { area = "Admin", id = userProfile.Id });
                        }
                        else
                        {
                            
                            return RedirectToAction("Welcome", "Users", new { area = "Admin", id = userProfile.Id });
                        }
                    }

                   
                    return LocalRedirect(model.ReturnUrl);
                }

                
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new
                    {
                        ReturnUrl = model.ReturnUrl,
                        RememberMe = model.RememberMe
                    });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            return View(model);
        }


        [Authorize,HttpPost]
        public async Task<IActionResult> LogoutAsync(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            returnUrl ??= Url.Content("~/");

            return LocalRedirect(returnUrl);
        }
        public IActionResult AccessDenied()
        {
            return View();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
