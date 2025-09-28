using Chess_API.Database;
using Chess_API.Enums;
using Chess_API.Models;
using Chess_API.utils;
using Chess_API.utils.Executors;
using Chess_API.utils.Handlers;
using Chess_API.utils.Services;
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

    // TODO - Add the marked field functionality to the endpoint

    /// <summary>
    /// Universal endpoint for user interaction.
    ///
    /// Handles selected fields and moves.
    ///
    /// Sets a field as selected or moves a piece to a field.
    ///
    /// Fields are marked as selected or movable.
    /// </summary>
    /// <returns>Redirect to the playing page</returns>
    [HttpPost]
    [Route("/playing/move")]
    public async Task<IActionResult> UserInteraction()
    {
        var fieldCoordinates = HttpContext.Request.Form["fieldCoordinates"];
        if (fieldCoordinates.Count == 0 || fieldCoordinates[0] is null)
        {
            return BadRequest("No field coordinates provided.");
        }
        // convert the string to a list of integers
        var fieldCoordinatesList = ConverterHelper.ConvertStringToIntsList(fieldCoordinates[0]!);
        // check if a field with a figure was selected
        var game = await _context
            .Game.Include(model => model.PlayerOne)
            .Include(model => model.PlayerTwo)
            .Include(model => model.Board)
            .ThenInclude(row => row.Row)
            .ThenInclude(field => field.Content)
            .Include(model => model.MoveHistory)
            .FirstAsync();

        game = RulesExecutor.HasGameEnded(game);

        // immediately return if the game has ended
        if (game.Winner is not Winner.Default)
        {
            await _context.SaveChangesAsync();
            return Redirect("/playing");
        }

        var fieldSelectedCheckResult = FieldHandler.IsAFieldSelected(game);
        if (fieldSelectedCheckResult.IsThereSelectedField)
        {
            // get the fields
            var curField = FieldHandler.GetSpecificFieldByCoordinates(
                game,
                new List<int>
                {
                    fieldSelectedCheckResult.X!.Value,
                    fieldSelectedCheckResult.Y!.Value,
                }
            );
            var selectedField = FieldHandler.GetSpecificFieldByCoordinates(
                game,
                fieldCoordinatesList
            );
            game = RulesExecutor.ValidateMove(game, curField, selectedField);
            // unselect all fields
            game = FieldHandler.UnselectAllFields(game);
            game = BoardHandler.UnmarkAllFieldsWherePieceCouldMoveTo(game);
        }
        else
        {
            // select a field
            game = FieldHandler.SetFieldSelected(game, fieldCoordinatesList);
            game = BoardHandler.MarkAllFieldsWherePieceCanMoveTo(game);
        }

        await _context.SaveChangesAsync();
        return Redirect("/playing");
    }

    // TODO - Add a endpoint for resigning and offering a draw

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
        var game = _context.Game.First();

        return View(game);
    }
}
