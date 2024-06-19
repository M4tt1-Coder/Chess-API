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

    /// <summary>
    /// Receives the new set game instance and saves it.
    ///
    /// Applies the time mode handling based on the decision of the user on the time mode.
    ///
    /// Fails when the game object is null.
    /// </summary>
    /// <returns>Result of the saving process.</returns>
    [HttpPost]
    [Route("settings/options/save")]
    public async Task<IActionResult> SaveGameOptions([FromForm] GameModel gameModel) //using [FromBody] didn't work
    {
        var game = await _context.Game.Include(model => model.PlayerOne)
            .Include(model => model.PlayerTwo).FirstAsync();

        //Just apply props that have been changed by the user.
        game.ApplyUserChanges(gameModel.Mode, gameModel.PlayTimeMode, gameModel.PlayerOne.Name,
            gameModel.PlayerTwo.Name, gameModel.PlayerOne.StartingTime, gameModel.PlayerTwo.StartingTime);

        await _context.SaveChangesAsync();

        return Redirect("/playing");
    }

    /// <summary>
    /// All other playing choices will be made here.
    ///
    /// Needs the playing mode which the player wants to play before the game.
    /// </summary>
    /// <returns>The view for the option page.</returns>
    [HttpGet]
    //[Route("/settings/{modeId:int}")]
    public async Task<IActionResult> Options(int? modeId)
    {
        if (modeId is null)
        {
            return NotFound();
        }

        //Create the game instance with the first option of the game mode
        var game = GameHandler.GameOnPlayingMode(modeId);

        //save it 
        _context.Game.Add(game);
        await _context.SaveChangesAsync();

        return View(game);
    }
}