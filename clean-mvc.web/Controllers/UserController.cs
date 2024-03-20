using clean_mvc.Domain.Contants;
using clean_mvc.Domain.Entities;
using clean_mvc.web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace clean_mvc.web.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Login(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            LoginViewModel model = new LoginViewModel()
            {
                ReturnUrl = returnUrl
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.Username);

                    //if (user != null && await _userManager.IsInRoleAsync(user, "Admin"))
                    //{
                    //    return RedirectToAction("Index", "Dashboard");
                    //}


                    if (string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return LocalRedirect(model.ReturnUrl);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt");
                }
            }

            return View(model);
        }
        public IActionResult Register()
        {

            if (!_roleManager.RoleExistsAsync(RoleDefault.Role_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(RoleDefault.Role_Admin)).Wait();
                _roleManager.CreateAsync(new IdentityRole(RoleDefault.Role_Public)).Wait();
                _roleManager.CreateAsync(new IdentityRole(RoleDefault.Role_Staff)).Wait();
                _roleManager.CreateAsync(new IdentityRole(RoleDefault.Role_Manager)).Wait();
                _roleManager.CreateAsync(new IdentityRole(RoleDefault.Role_HeadOfManager)).Wait();
                _roleManager.CreateAsync(new IdentityRole(RoleDefault.Role_Suppervisor)).Wait();
                _roleManager.CreateAsync(new IdentityRole(RoleDefault.Role_Director)).Wait();
            }

            RegisterViewModel registerVM = new RegisterViewModel()
            {
                RoleList = _roleManager.Roles.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id
                })
            };
            return View(registerVM);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            ApplicationUser user = new ApplicationUser()
            {
                UserName = model.Username,
                CreatedDate = DateTime.Now,
                EmplCode = model.Username,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.Role))
                {
                    await _userManager.AddToRoleAsync(user, model.Role);
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, RoleDefault.Role_Staff);
                }

                await _signInManager.SignInAsync(user, isPersistent: false);

                if (string.IsNullOrEmpty(model.ReturnUrl))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return LocalRedirect(model.ReturnUrl);
                }
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            model.RoleList = _roleManager.Roles.Select(x => x.Name)
                .Select(m => new SelectListItem
                {
                    Text = m,
                    Value = m
                });

            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Index), "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
