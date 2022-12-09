using GameStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GameStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await _userManager.FindByNameAsync(model.UserLogin);
                if (user != null)
                {
                    await _signInManager.SignOutAsync();

                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                    if (result.Succeeded)
                        return Redirect(returnUrl ?? "/");
                }

                ModelState.AddModelError(nameof(LoginViewModel.UserLogin), "Invalid login or password");
            }

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Registration(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View(new RegistrationViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _userManager.FindByNameAsync(model.UserLogin) != null)
                {
                    ModelState.AddModelError(nameof(RegistrationViewModel.UserLogin), "This user already exists in the system");
                    return View(model);
                }

                var user = new IdentityUser
                {
                    Id = System.Guid.NewGuid().ToString(),
                    UserName = model.UserLogin,
                    NormalizedUserName = model.UserLogin.ToUpper(),
                    Email = model.Email,
                    NormalizedEmail = model.Email.ToUpper(),
                    PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, model.Password)
                };

                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }

                else
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(nameof(error), error.Description);

                    return View(model);
                }
            }

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}