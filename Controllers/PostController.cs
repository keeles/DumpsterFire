using System.Diagnostics;
using ASP.NETCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NETCore.Controllers;

public class PostController : Controller
{
    private readonly ILogger<PostController> _logger;
    private readonly ISession _session;
    private readonly ApplicationDbContext _context;

    public PostController(
        ILogger<PostController> logger,
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _logger = logger;
        _context = context;
        _session = httpContextAccessor.HttpContext.Session;
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
            var username = _session.GetString("loggedIn");
            User user = await _context.Users.Where(u => u.Username == username).SingleAsync();
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
