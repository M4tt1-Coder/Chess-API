using Chess_API.Database;
using Microsoft.EntityFrameworkCore;

namespace Chess_API.utils.Services;

/// <summary>
/// Contains a middleware pipeline component function.
///
/// The middleware function handles different routes and the conditions that need to be fulfilled to access them.
///
/// Source help: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/write?view=aspnetcore-8.0
/// </summary>
public class ChessMiddleware
{
    /// <summary>
    /// The function that gets called when an HTTP request comes in.
    /// </summary>
    private readonly RequestDelegate _next;

    // https://www.youtube.com/watch?v=cu4CUJAcJ-4
    // https://www.youtube.com/watch?v=ToFqISWq4is
    // private readonly ChessDbContext _context;

    private readonly IProtectionService _protector;

    /// <summary>
    /// Logger instance ...
    /// </summary>
    private readonly ILogger<ChessMiddleware> _logger;

    public ChessMiddleware(
        RequestDelegate next,
        ILogger<ChessMiddleware> logger,
        IProtectionService protector
    )
    {
        _next = next;
        _logger = logger;
        _protector = protector;
    }

    /// <summary>
    /// Logic of the middleware.
    ///
    /// Mainly relying on the destination path, it checks what is needed to enter a
    /// specific route.
    ///
    /// It calls the 'RouteProtector' service to look up if the conditions are given.
    /// </summary>
    /// <param name="c">Gets passed with every http request and contains relevant data.</param>
    /// <param name="dbContext">Context of the database.</param>
    public async Task InvokeAsync(HttpContext c, ChessDbContext dbContext)
    {
        // circumstances will be checked when specific request urls are given

        // user wants to save the game after making all adjustments
        if (c.Request.Path == "/settings/options/save")
        {
            var currentGame = dbContext.Game.First();

            // game instance is needed
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (currentGame == null)
            {
                _logger.LogInformation("No game instance was found!");
                c.Response.StatusCode = 400;
                await c.Response.WriteAsync("Data couldn't be loaded effectively!");
                _logger.LogError(
                    "Game object was deallocated or wasn't created before saving all adjustments the user made!"
                );
                c.Abort();
            }
        }

        // player proceeds from options to playing field
        if (c.Request.Path == "/playing")
        {
            // need a game with a complete dataset
            // strictly say that all needed props are included in the query
            var currentGame = dbContext
                .Game.Include(game => game.PlayerOne)
                .Include(game => game.PlayerTwo)
                .Include(game => game.Board)
                .ThenInclude(gameField => gameField.Row)
                .ThenInclude(field => field.Content)
                .First();

            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (currentGame is null)
            {
                _logger.LogError("No game object when entering playing field");
                c.Response.StatusCode = 400;
                await c.Response.WriteAsync("Needed data hasn't been provided.");
                c.Abort();
            }

            bool isReadyToPlay = _protector.HasGameEverythingForPlaying(currentGame);

            if (!isReadyToPlay)
            {
                _logger.LogError("Memory isn't complete");
                c.Response.StatusCode = 400;
                await c.Response.WriteAsync("Required data could not be allocated!");
                c.Abort();
            }
        }

        await _next(c);
    }
}

/// <summary>
/// Add the middleware in the pipeline.
/// </summary>
public static class ChessMiddlewareExtension
{
    public static IApplicationBuilder UseChessMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ChessMiddleware>();
    }
}
