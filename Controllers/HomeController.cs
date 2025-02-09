using System.Diagnostics;
using ASP.NETCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NETCore.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
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

    public IActionResult Members()
    {
        return View("Error");
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
