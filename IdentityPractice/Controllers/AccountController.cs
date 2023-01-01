using IdentityPractice.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Internal;

namespace IdentityPractice.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<FoodUser> _signinManager;
        private readonly UserManager<FoodUser> _userManager;
        private readonly RoleManager<FoodRole> _roleManager;
        private static string roleName = "GoodRole";
        public AccountController(SignInManager<FoodUser> signInManager, UserManager<FoodUser> userManager, RoleManager<FoodRole> roleManager)
        {
            _signinManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index() => View();
        public IActionResult Login() => View();
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var checkUser = await
                _userManager.FindByNameAsync(username);
            if (checkUser is null)
            {
                return BadRequest($"Unable to find user with name {username}");
            }
            var signinResult = await _signinManager.PasswordSignInAsync(checkUser, password, false, false);
            if (!signinResult.Succeeded)
            {
                return BadRequest($"Incorrect password for user {username}");
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Register() => View();
        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            var user = new FoodUser() { UserName = username };
            var createUserResult = await
                _userManager.CreateAsync(user, password);
            if (createUserResult.Succeeded)
            {
                if (username.Contains("good"))
                {
                    try
                    {
                        await CreateRole();
                        var role = await _roleManager.FindByNameAsync(roleName);
                        await _userManager.AddToRoleAsync(user, role.Name);
                    } catch(CreateRoleFailureException rex)
                    {
                        return Problem("Failed to create role " + roleName);
                    }
                }
                return RedirectToAction("Login");
            }
            return ValidationProblem(GetModelStateErrors(createUserResult.Errors));
            
        }

        public async Task<IActionResult> Logout()
        {
            await _signinManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied() => View();

        private ModelStateDictionary GetModelStateErrors(IEnumerable<IdentityError> errors)
        {
            ModelStateDictionary dict = new ModelStateDictionary();
            foreach (var err in errors)
            {
                dict.AddModelError(err.Code, err.Description);
            }

            return dict;
        }

        private async Task CreateRole()
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                var goodRole = new FoodRole() { Name = roleName };
                var createRoleResult = await _roleManager.CreateAsync(goodRole);
                if (!createRoleResult.Succeeded)
                {
                    throw new CreateRoleFailureException();
                }
            }
        }

        
    }

    public class CreateRoleFailureException : Exception { }
}
