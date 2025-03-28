using System.Diagnostics;
using ASP.NETCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NETCore.Controllers;

public class BoardController : Controller
{
    private readonly ILogger<BoardController> _logger;
    private readonly ApplicationDbContext _context;

    public BoardController(ILogger<BoardController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("Board/Index/{id}")]
    public async Task<IActionResult> Index(int id)
    {
        var board = await _context
            .Boards.Include(b => b.Threads)
            .ThenInclude(t => t.User)
            .Include(b => b.User)
            .Where(b => b.Serial == id)
            .SingleAsync();
        return View(board);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        );
    }
}
