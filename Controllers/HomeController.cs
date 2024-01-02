using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Chess_API.Models;

namespace Chess_API.Controllers;

public class HomeController : Controller
{
    private readonly ChessDbContext _context;
    
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, ChessDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View(_context.Game);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult GameField()
    {
        _logger.LogInformation("We are here!");
        return View();
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}