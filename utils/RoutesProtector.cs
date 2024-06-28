using Chess_API.Models;

namespace Chess_API.utils;

/// <summary>
/// Is an API service for the app middleware that includes all necessary
/// functions to check for simple data completion or state changes, for example.  
/// </summary>
public class RoutesProtector : IProtectionService
{
    /// <summary>
    /// The logger for important middleware services.
    /// </summary>
    private readonly ILogger<RoutesProtector> _logger;

    public RoutesProtector(ILogger<RoutesProtector> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// So that the playing field can be rendered, data conditions need to be fulfilled.
    /// The game needs a field and player objects, not looking at changes of the user. Default settings are applied.
    /// </summary>
    /// <returns>Boolean determining if the requirements are all given to enter the playing page</returns>
    public bool HasGameEverythingForPlaying(GameModel? game)
    {
        // when the game is eventually null instantly return false
        if (game is null)
        {
            _logger.LogError("Game was null when entering the '/playing'-route");
            return false;
        }
        
        
        // the game needs a field prop and then a complete one
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (game.Field is null)
        {
            _logger.LogError("Game instance didn't have a field");
            return false;
        }
        
        // when a field instance is there, there is the opportunity that the rows don't include all fields 
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (game.Field.Any(row => row.Row.Any(field => field is null)))
        {
            _logger.LogError("Rows did not have all required fields");
            return false;
        }
        
        // two players 
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (game.PlayerOne is null)
        {
            _logger.LogError("Couldn't find player one");
            return false;
        }
        
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (game.PlayerTwo is null)
        {
            _logger.LogError("Couldn't find player two");
            return false;
        }
        
        return true;
    }
}