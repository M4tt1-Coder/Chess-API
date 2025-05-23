using Chess_API.Enums;
using Chess_API.Models;

namespace Chess_API.utils;

/// <summary>
/// At the beginning of a game, it will provide a default settings game-object.
///
/// Includes functions to handle the game object during the playing process and at the end of it.
/// </summary>
public static class GameHandler
{
    //constants

    /// <summary>
    /// Represents that the first round is always 1.
    /// </summary>
    private static readonly int DefaultRound = 1;

    /// <summary>
    /// It's there to create a completely new game object for another playing round.
    ///
    /// It gets the default field layout from a default field component.
    /// </summary>
    /// <returns>
    /// A normal starter game instance with default properties. 
    /// </returns>
    public static GameModel Default()
    {
        return new GameModel(
            new PlayerModel(null, "", 0, Colors.White),
            new PlayerModel(null, "", 0, Colors.Black),
            FieldHandler.Default(),
            DefaultRound,
            PlayingMode.Default,
            Winner.Default,
            PlayingDirection.WhiteBottom
            
        );
    }

    /// <summary>
    /// Creates a temporary copy of the current game instance.
    /// </summary>
    /// <param name="game">The current game instance</param>
    /// <returns>A temporary copy of the current game instance</returns>
    public static GameModel CopyGame(GameModel game)
    {
        // assign all properties of the game object to a new game object
        return new GameModel(
            PlayerHandler.CopyPlayer(game.PlayerOne),
            PlayerHandler.CopyPlayer(game.PlayerTwo),
            game.CopyBoard(),
            game.Round,
            game.Mode,
            game.Winner,
            game.Direction
        );
    }
    
    /// <summary>
    /// Prepares the game for a new game in the same game mode as the last round.
    ///
    /// Edit those properties that need to be touched.
    /// </summary>
    /// <param name="game">Game instance after a playing round.</param>
    /// <returns>A reset game object.</returns>
    public static GameModel Reset(GameModel game)
    {
        game.Board = FieldHandler.Default();
        game.Winner = Winner.Default;
        //gaming mode stays the same the player wanted to play a new game in the same playing mode as last time 
        //game.Mode = PlayingMode.Default;
        game.Round++;
        game.PlayerOne.PrepPlayerForNewGame(game.PlayTimeMode); 
        game.PlayerTwo.PrepPlayerForNewGame(game.PlayTimeMode);
        return game;
    }

    /// <summary>
    /// Creates a default game instance for the start of the game based on the play-mode the user chose to play.
    ///
    /// It's the entrypoint for the game object.
    ///
    /// Changes the Mode-prop-dependent on the choice.
    /// </summary>
    /// <param name="modeId">Represents which game mode the player wants to play.</param>
    /// <returns>The initialization game object.</returns>
    public static GameModel GameOnPlayingMode(int? modeId = 3)
    {
        var output = Default();

        output.Mode = modeId switch
        {
            0 => PlayingMode.UserVsUserLocal,
            1 => PlayingMode.UserVsAi,
            2 => PlayingMode.UserVsUserOnline,
            3 => PlayingMode.Default,
            _ => output.Mode
        };

        return output;
    }

    /// <summary>
    /// A keypoint function where the user can set all adjustable properties of the game.
    ///
    /// Defines the time mode, player attributes and a possible new wanted playing mode by the user.
    ///
    /// Fails when an invalid argument was passed and not filtered by the compiler.
    /// </summary>
    /// <param name="game">The current game instance.</param>
    /// <param name="newPlayingMode">If the user set a new playing mode, it will be stored here.</param>
    /// <param name="timeMode">Can't be null! Represents the time mode for the game.</param>
    /// <param name="playerOneName">Custom player 1 name to use in the game.</param>
    /// <param name="playerTwoName">Custom player 2 name to use in the game.</param>
    /// <param name="playerOneTime">Custom time of player 1.</param>
    /// <param name="playerTwoTime">If the user entered a custom time for 'player two,'
    /// then it will be stored here.</param>
    public static void ApplyUserChanges(this GameModel game, PlayingMode newPlayingMode, PlayTimeMode timeMode,
        string? playerOneName, string? playerTwoName, TimeSpan? playerOneTime, TimeSpan? playerTwoTime)
    {
        //game mode
        if (game.Mode != newPlayingMode)
        {
            game.Mode = newPlayingMode;
        }

        //time mode
        if (game.PlayTimeMode != timeMode)
        {
            game.PlayTimeMode = timeMode;
        }

        switch (timeMode)
        {
            case PlayTimeMode.Custom:
                if (playerOneTime != null && playerTwoTime != null)
                {
                    game.PlayerOne.StartingTime = playerOneTime;
                    game.PlayerTwo.StartingTime = playerTwoTime;
                }
                break;
            case PlayTimeMode.ThreeMinutes:
                game.PlayerOne.StartingTime = new TimeSpan(0,3,0);
                game.PlayerTwo.StartingTime = new TimeSpan(0, 3, 0);
                break;
            case PlayTimeMode.TenMinutes:
                game.PlayerOne.StartingTime = new TimeSpan(0, 10, 0);
                game.PlayerTwo.StartingTime = new TimeSpan(0, 10, 0);
                break;
            case PlayTimeMode.ThirtyMinutes:
                game.PlayerOne.StartingTime = new TimeSpan(0, 30, 0);
                game.PlayerTwo.StartingTime = new TimeSpan(0, 30, 0);
                break;
            case PlayTimeMode.NoTimeLimit:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(timeMode), timeMode, null);
        }

        //player 1 name
        game.PlayerOne.Name = !string.IsNullOrEmpty(playerOneName) ? playerOneName : "Player 1";

        //player 2 name
        game.PlayerTwo.Name = !string.IsNullOrEmpty(playerTwoName) ? playerTwoName : "Player 2";
    }

    /// <summary>
    /// Determines the starting time for a player based on the selected play time mode.
    /// </summary>
    /// <param name="timeMode">The play time mode indicating the duration of the game.</param>
    /// <returns>
    /// A TimeSpan representing the starting time for a player.
    /// Returns a zero TimeSpan if the time mode has no associated limit.
    /// </returns>
    public static TimeSpan? GetPlayerStartingTime(PlayTimeMode timeMode)
    {
        return timeMode switch
        {
            PlayTimeMode.Custom => TimeSpan.Zero,
            PlayTimeMode.ThreeMinutes => new TimeSpan(0, 3, 0),
            PlayTimeMode.TenMinutes => new TimeSpan(0, 10, 0),
            PlayTimeMode.ThirtyMinutes => new TimeSpan(0, 30, 0),
            PlayTimeMode.NoTimeLimit => null,
            _ => throw new ArgumentOutOfRangeException(nameof(timeMode), timeMode, null)
        };
    }
}