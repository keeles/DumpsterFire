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
}
