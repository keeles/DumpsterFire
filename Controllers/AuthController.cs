using System.Diagnostics;
using System.Threading.Tasks;
using ASP.NETCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NETCore.Controllers;

public class AuthController : Controller
{
    private readonly ILogger<AuthController> _logger;
    private readonly ISession _session;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AuthController(
        ILogger<AuthController> logger,
        ApplicationDbContext context,
        UserManager<User> userManager,
        SignInManager<User> signInManager
    )
    {
        _logger = logger;
        _context = context;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpPost("Auth/Login")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        model.ReturnUrl ??= Url.Content("~/");
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false
            );

            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                return RedirectToAction(nameof(Index), "Home");
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToPage(
                    "./LoginWith2fa",
                    new { ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe }
                );
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                return RedirectToPage("./Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return RedirectToAction(nameof(Index), "Home");
            }
        }

        // If we got this far, something failed, redisplay form
        return RedirectToAction(nameof(Index), "Home");
    }

    [HttpGet("Auth/Register")]
    public IActionResult Register()
    {
        var viewModel = new RegisterViewModel();
        return View(viewModel);
    }

    [HttpPost("Auth/Register")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model, IFormFile profilePicture)
    {
        ModelState.Remove("ProfilePicture");
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new
                {
                    Key = x.Key,
                    Errors = x.Value.Errors.Select(e => e.ErrorMessage).ToList(),
                })
                .ToList();

            // Log errors to console
            Console.WriteLine("Validation Errors:");
            foreach (var error in errors)
            {
                Console.WriteLine($"Property: {error.Key}");
                foreach (var errorMessage in error.Errors)
                {
                    Console.WriteLine($"- {errorMessage}");
                }
            }

            // Alternatively, you can log to your logger
            foreach (var error in errors)
            {
                _logger.LogWarning(
                    $"Validation Error in {error.Key}: {string.Join(", ", error.Errors)}"
                );
            }
        }
        if (ModelState.IsValid)
        {
            string ProfilePicture = "";
            string Username = model.Username;
            string About = string.IsNullOrEmpty(model.About)
                ? $"Hey! I am {model.Username}"
                : model.About;
            // Handle profile picture upload
            if (profilePicture != null && profilePicture.Length > 0)
            {
                var uploadPath = Path.Combine("wwwroot", "uploads", "profiles");
                Directory.CreateDirectory(uploadPath);
                var fileName = $"{Guid.NewGuid()}_{profilePicture.FileName}";
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await profilePicture.CopyToAsync(stream);
                }

                ProfilePicture = $"/uploads/profiles/{fileName}";
            }
            else
            {
                ProfilePicture = "/favicon.svg";
            }
            var user = new User();
            user.UserName = Username;
            user.About = About;
            user.ProfilePicture = ProfilePicture;

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout(string returnUrl = null)
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out.");

        if (returnUrl != null)
        {
            return LocalRedirect(returnUrl);
        }
        else
        {
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error");
    }
}
