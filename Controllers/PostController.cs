using System.Diagnostics;
using System.Security.Claims;
using ASP.NETCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NETCore.Controllers;

public class PostController : Controller
{
    private readonly ILogger<PostController> _logger;
    private readonly ISession _session;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;

    public PostController(
        ILogger<PostController> logger,
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor,
        UserManager<User> userManager
    )
    {
        _logger = logger;
        _context = context;
        _session = httpContextAccessor.HttpContext.Session;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        return RedirectToAction(nameof(Index), "Home");
    }

    [HttpPost("Post/New/{threadId}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> New(int threadId, [Bind("content")] string content)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.GetUserAsync(User);
            if (string.IsNullOrEmpty(user?.Id))
                throw new Exception("User not found");
            var thread = await _context.Threads.Where(t => t.Serial == threadId).SingleAsync();
            Post post = new Post(content, user, thread);
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), "Thread", new { id = thread.Serial });
        }
        catch
        {
            return RedirectToAction(nameof(Index), "Home");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        );
    }
}
