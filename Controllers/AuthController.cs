using System.Diagnostics;
using System.Threading.Tasks;
using ASP.NETCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NETCore.Controllers;

public class AuthController : Controller
{
    private readonly ILogger<AuthController> _logger;
    private readonly ISession _session;
    private readonly ApplicationDbContext _context;

    public AuthController(
        ILogger<AuthController> logger,
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _logger = logger;
        _context = context;
        _session = httpContextAccessor.HttpContext.Session;
    }

    [HttpPost("Auth/Login")]
    public async Task<IActionResult> Login(
        [Bind("Username")] string username,
        [Bind("Password")] string password
    )
    {
        try
        {
            User user = await _context.Users.Where(u => u.Username == username).SingleAsync();
            bool validPassword = BCrypt.Net.BCrypt.Verify(password, user.Password);
            if (validPassword)
            {
                _session.SetString("loggedIn", username);
            }
            else
            {
                //TODO: show some feedback to the user?
                Console.WriteLine("Invalid Password");
            }
        }
        catch
        {
            //TODO: show some feedback to the user?
            Console.WriteLine("Could not find user in db");
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
    public async Task<IActionResult> Register(
        [Bind("Username")] string username,
        [Bind("Password")] string password,
        [Bind("About")] string about,
        //TODO: Handle profile picture upload
        [Bind("ProfilePicture")]
            string profilePicture
    )
    {
        try
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username), "Cannot be empty");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password), "Cannot be empty");
            if (string.IsNullOrEmpty(profilePicture))
                profilePicture = "/favicon.svg";
            if (string.IsNullOrEmpty(about))
                about = $"Hey! I am {username}";
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            User user = new User(username, hashedPassword, profilePicture, about);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            _session.SetString("loggedIn", username);
        }
        catch
        {
            //TODO: show some feedback to the user?
            Console.WriteLine("Error registering user and setting session");
        }
        return RedirectToAction(nameof(Index), "Home");
    }

    [HttpGet("Auth/Logout")]
    public IActionResult Logout()
    {
        _session.Remove("loggedIn");
        return RedirectToAction(nameof(Index), "Home");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error");
    }
}
