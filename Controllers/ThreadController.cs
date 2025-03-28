using System.Diagnostics;
using System.Security.Claims;
using ASP.NETCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NETCore.Controllers;

public class ThreadController : Controller
{
    private readonly ILogger<ThreadController> _logger;
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDbContext _context;

    public ThreadController(
        ILogger<ThreadController> logger,
        ApplicationDbContext context,
        UserManager<User> userManager
    )
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
    }

    [HttpGet("Thread/Index/{id}")]
    public async Task<IActionResult> Index(int id)
    {
        try
        {
            var thread = await _context
                .Threads.Include(t => t.Posts)
                .ThenInclude(p => p.User)
                .Include(t => t.User)
                .Where(t => t.Serial == id)
                .SingleAsync();
            var tuple = new Tuple<Thread, Post>(thread, new Post());
            return View(tuple);
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

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Create(int id)
    {
        try
        {
            var board = await _context
                .Boards.Include(b => b.Threads)
                .ThenInclude(t => t.User)
                .Where(b => b.Serial == id)
                .SingleAsync();
            var viewModel = new ThreadCreateViewModel(board);
            return View(viewModel);
        }
        catch
        {
            return RedirectToAction(nameof(Index), "Home");
        }
    }

    [HttpPost("Thread/New/{boardId}")]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> New(int boardId, [Bind("Title")] string title)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.GetUserAsync(User);
            if (string.IsNullOrEmpty(user?.Id))
                throw new Exception("User not found");
            var board = await _context.Boards.Where(b => b.Serial == boardId).SingleAsync();
            Thread newThread = new Thread(user, title, board);
            await _context.Threads.AddAsync(newThread);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), "Thread", new { id = newThread.Serial });
        }
        catch
        {
            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
