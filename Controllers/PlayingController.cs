using Microsoft.AspNetCore.Mvc;

namespace Chess_API.Controllers;

public class PlayingController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    // public async Task<IActionResult> GameField()
    // {
    //     GameModel game = GameHandler.Default();
    //
    //     _context.Game.Add(game);
    //     await _context.SaveChangesAsync();
    //
    //     GameModel testGame = await _context.Game.FirstAsync();      
    //     
    //     return View(testGame);
    // }
    public IActionResult GameField()
    {
        return View();
    }
}