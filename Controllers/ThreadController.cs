using System.Diagnostics;
using ASP.NETCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NETCore.Controllers;

public class ThreadController : Controller
{
    private readonly ILogger<ThreadController> _logger;
    private readonly ApplicationDbContext _context;

    public ThreadController(ILogger<ThreadController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("Thread/Index/{id}")]
    public async Task<IActionResult> Index(int id)
    {
        var thread = await _context
            .Threads.Include(t => t.Posts)
            .ThenInclude(p => p.User)
            .Include(t => t.User)
            .Where(t => t.Serial == id)
            .SingleAsync();
        return View(thread);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        );
    }

    [HttpGet]
    public async Task<IActionResult> Create(int id)
    {
        var board = await _context
            .Boards.Include(b => b.Threads)
            .ThenInclude(t => t.User)
            .Where(b => b.Serial == id)
            .SingleAsync();
        return View(board);
    }

    [HttpPost("Thread/New/{boardId}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> New(int boardId, [Bind("Title")] string title)
    {
        var user = await _context.Users.Where(u => u.Serial == 1).SingleAsync();
        var board = await _context.Boards.Where(b => b.Serial == boardId).SingleAsync();
        Thread newThread = new Thread(user, title, board);
        await _context.Threads.AddAsync(newThread);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index), "Thread", new { id = newThread.Serial });
    }
}
