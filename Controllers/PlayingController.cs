using Chess_API.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chess_API.Controllers;

public class PlayingController : Controller
{
    // dependencies
    private readonly ChessDbContext _context;

    private readonly ILogger<PlayingController> _logger;

    public PlayingController(ChessDbContext context, ILogger<PlayingController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // TODO - Game board needs to be reorder -> white on top then bottom and then on top again
    // depending which players turn it is; just effective for local playing
    
    // TODO - Add resign & admit defeat functionality
        
    // TODO - Finish universal board interaction endpoint
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("/playing/move")]
    public async Task<IActionResult> UserInteraction([FromBody] IList<int> coordinates)
    {
        // TODO - Rule-Executor instance
        // check if a field with a figure was selected 
        // check if the piece can move to that field -> what are consequences
        return Redirect("/playing");
    }

    /// <summary>
    /// Gets the game model out of memory.
    /// Passes it to the UI as model-binding.
    /// </summary>
    /// <returns>View of the playing field with a game model</returns>
    public async Task<IActionResult> Index()
    {
        // get the game instance
        // add inclusion declarations of properties that forcibly have to take out of the in-memory database 
        // data needs to be included in the entity
        // https://learn.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.entityframeworkqueryableextensions.include?view=efcore-8.0&viewFallbackFrom=net-6.0
        var game = await _context.Game.Include(model => model.PlayerOne)
            .Include(model => model.PlayerTwo)
            .Include(model => model.Field).ThenInclude(row => row.Row).ThenInclude(field => field.Content)
            .FirstAsync();
        
        // 
        
        return View(game);
    }
}