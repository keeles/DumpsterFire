using System.Diagnostics;
using ASP.NETCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NETCore.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ISession _session;
    private readonly ApplicationDbContext _context;

    public HomeController(
        ILogger<HomeController> logger,
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _logger = logger;
        _context = context;
        _session = httpContextAccessor.HttpContext.Session;
    }

    public async Task<IActionResult> Index()
    {
        var boards = await _context
            .Boards.Include(b => b.Threads)
            .Include(b => b.User)
            .ToListAsync();
        return View(boards);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Search()
    {
        return View("Error");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error");
    }
}
