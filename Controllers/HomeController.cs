using System.Diagnostics;
using System.Resources;
using Microsoft.AspNetCore.Mvc;
using Chess_API.Database;
using Chess_API.Models;
using Chess_API.utils;
using Microsoft.EntityFrameworkCore;

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

    public async Task<IActionResult> GameField()
    {
        GameModel game = GameHandler.Default();

        _context.Game.Add(game);
        await _context.SaveChangesAsync();

        GameModel testGame = await _context.Game.FirstAsync();      
        
        return View(testGame);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}