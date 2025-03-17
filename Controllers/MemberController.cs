using System.Diagnostics;
using ASP.NETCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NETCore.Controllers;

public class MemberController : Controller
{
    private readonly ILogger<MemberController> _logger;
    private readonly ISession _session;
    private readonly ApplicationDbContext _context;

    public MemberController(
        ILogger<MemberController> logger,
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
        var users = await _context.Users.ToListAsync();
        throw new Exception();
        return View(users);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error");
    }
}
