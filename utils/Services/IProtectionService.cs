using Chess_API.Models;

namespace Chess_API.utils;

/// <summary>
/// A service type to add checking functions to the pipeline.
///
/// It contains all necessary methods for rule execution.
/// <code>
///     // can be with more query conditions
///     var game = _context.Game.First();
///
///     bool answer = HasGameEverythingForPlaying(game);
/// </code>
/// </summary>
public interface IProtectionService
{
    public bool HasGameEverythingForPlaying(GameModel? game);
}
