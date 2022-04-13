using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using GB.ASPNET.WebStore.ViewModels.Identity;
using GB.ASPNET.WebStore.Domain.Entities.Identity;

namespace GB.ASPNET.WebStore.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<User> _UserManager;
    private readonly SignInManager<User> _SignInManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(UserManager<User> userManager, SignInManager<User> signinManager, ILogger<AccountController> logger)
    {
        _UserManager = userManager;
        _SignInManager = signinManager;
        _logger = logger;
    }

    public IActionResult AccessDenied()
    {
        return View();
    }

    public IActionResult Login(string url) => View(new LoginUserVM { ReturnUrl = url });
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginUserVM viewmodel)
    {
        if (!ModelState.IsValid) return View(viewmodel);
        
        var loginResult = await _SignInManager.PasswordSignInAsync(
            viewmodel.UserName,
            viewmodel.Password,
            isPersistent: viewmodel.RememberMe,
            lockoutOnFailure: true);

        if (loginResult.Succeeded)
        {
            _logger.LogInformation("Пользователь {0}: успешный вход в систему.", viewmodel.UserName);
            //if (Url.IsLocalUrl(viewmodel.ReturnUrl)) return Redirect(viewmodel.ReturnUrl);
            //else return RedirectToAction(actionName: "Index", controllerName: "Home");

            return LocalRedirect(viewmodel.ReturnUrl ?? "/");
        }
        //else if (loginResult.RequiresTwoFactor) { }
        //else if (loginResult.IsLockedOut) { }
        else
        {
            _logger.LogWarning("Пользователь {0}: ошибка при входе в систему.", viewmodel.UserName);
            ModelState.AddModelError(string.Empty, "Неверные имя пользователя и/или пароль.");
            return View(loginResult);
        }
    }

    public async Task<IActionResult> Logout()
    {
        string? username = User.Identity!.Name;
        await _SignInManager.SignOutAsync();
        _logger.LogInformation("Пользователь {0}: выход из системы.", username);
        return RedirectToAction(actionName: "Index", controllerName: "Home");
    }

    public IActionResult Register() => View(new RegisterUserVM());
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterUserVM viewmodel)
    {
        if (!ModelState.IsValid) return View(viewmodel);

        var user = new User { UserName = viewmodel.UserName };
        IdentityResult? creationResult = await _UserManager.CreateAsync(user, viewmodel.Password);
        if (creationResult.Succeeded)
        {
            _logger.LogInformation("Пользователь {0}: зарегистрирован.", viewmodel.UserName);
            await _SignInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction(actionName: "Index", controllerName: "Home");
        }
        else
        {
            _logger.LogWarning("Пользователь {0}: ошибки при регистрации ({1}).",
                viewmodel.UserName,
                String.Join(", ", creationResult.Errors.Select(el => el.Description)));

            foreach (var error in creationResult.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return View(viewmodel);
        }
    }
}