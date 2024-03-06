using Chess_API.Database;
using Chess_API.Models;
using Chess_API.utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chess_API.Controllers;

public class SettingsController : Controller
{
    private readonly ChessDbContext _context;

    private readonly ILogger<SettingsController> _logger;

    public SettingsController(ChessDbContext context, ILogger<SettingsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Represents the index page of the settings route.
    ///
    /// The user sets his game mode choice here.
    /// </summary>
    /// <returns>The result of an action method.</returns>
    public IActionResult Index()
    {
        return View();
    }

    //TODO - Add Saving-function for the prepared game of the player
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("/settings/options/save")]
    public async Task<IActionResult> SaveGameOptions([FromBody]GameModel gameModel)
    {
        GameModel game = await _context.Game.FirstAsync();

        return RedirectToAction("Options");
    }

    /// <summary>
    /// All other playing choice will be made here.
    ///
    /// Needs the playing mode which the player wants to play before the game.
    /// </summary>
    /// <returns>The view for the options page.</returns>
    [HttpGet]
    //[Route("/settings/{modeId:int}")]
    public async Task<IActionResult> Options(int? modeId)
    {
        if (modeId is null)
        {
            return NotFound();
        }

        //Create the game instance with the first option of the game mode
        GameModel game = GameHandler.GameOnPlayingMode(modeId);

        //save it 
        _context.Game.Add(game);
        await _context.SaveChangesAsync();

        return View(game);
    }
}