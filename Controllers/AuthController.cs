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
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly S3UploadService _s3UploadService;

    public AuthController(
        ILogger<AuthController> logger,
        ApplicationDbContext context,
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        S3UploadService s3UploadService
    )
    {
        _logger = logger;
        _context = context;
        _signInManager = signInManager;
        _userManager = userManager;
        _s3UploadService = s3UploadService;
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
        if (!ModelState.IsValid)
        {
            Console.WriteLine("Error with model state");
            Console.WriteLine(ModelState.ErrorCount);
        }
        if (ModelState.IsValid)
        {
            string Username = model.Username;
            string About = model.About;
            string? profilePictureUrl = null;

            if (model.ProfilePicture != null && model.ProfilePicture.Length > 0)
            {
                profilePictureUrl = await _s3UploadService.UploadFileAsync(model.ProfilePicture);

                if (profilePictureUrl == null)
                {
                    ModelState.AddModelError("ProfilePicture", "File upload failed");
                    return View(model);
                }
            }
            var user = new User();
            user.UserName = Username;
            user.About = About;
            user.ProfilePicture = profilePictureUrl;

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");
                await _userManager.AddToRoleAsync(user, "User");
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
    public async Task<IActionResult> Logout(string? returnUrl = null)
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MakeAdmin(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return View("Error");
        }
        var result = await _userManager.AddToRoleAsync(user, "Admin");
        if (!result.Succeeded)
            return View("Error");
        return RedirectToAction(nameof(MemberController.Index), "Member");
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
