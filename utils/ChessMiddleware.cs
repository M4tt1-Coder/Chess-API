// TODO - Implement middleware that checks for situation context -> makes sure that you can't enter a page till you haven't fulfilled specific conditions

using System.Buffers;
using Chess_API.Database;
using Microsoft.EntityFrameworkCore;

namespace Chess_API.utils;

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

    /// <summary>
    /// The DB context of the application to access the data of the current run.
    /// </summary>
    private readonly ChessDbContext _context;

    /// <summary>
    /// Logger instance ... 
    /// </summary>
    private readonly ILogger<ChessMiddleware> _logger;
    
    public ChessMiddleware(RequestDelegate next, ChessDbContext context, ILogger<ChessMiddleware> logger)
    {
        _next = next;
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="c"></param>
    public async Task InvokeAsync(HttpContext c)
    {
        // TODO - Should just skip the checks when the user is just in the starting screen
        // circumstances will be checked when specific request urls are given
        
        // current game with all necessary data fetched
        var currentGame = _context.Game.Include(g => g.PlayerOne)
            .Include(g => g.PlayerTwo)
            .Include(g => g.Field)
            .ThenInclude(r => r.Row)
            .ThenInclude(f => f.Content)
            .First();
        
        // user wants to save the game after making all adjustments
        if (c.Request.Path == "settings/options/save")
        {
            // game instance is needed
            if (currentGame is null)
            {
                c.Response.StatusCode = 400;
                await c.Response.WriteAsync("Data couldn't be loaded effectively!");
                _logger.LogError("Game object was deallocated or wasn't created before saving all adjustments the user made!");
                c.Abort();
            }
        }
        
        // player proceeds from options to playing field
        if (c.Request.Path == ""){}
        
        await _next(c);
    }
}

/// <summary>
/// 
/// </summary>
public static class ChessMiddlewareExtension
{
    public static IApplicationBuilder UseChessMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ChessMiddleware>();
    }
}