using Chess_API.Enums;
using Chess_API.Models;

namespace Chess_API.utils;

/// <summary>
/// At the beginning of a game it will provide a default settings game-object.
///
/// Includes functions to handle the game object during the playing-process and at the end of it.
/// </summary>
public static class GameHandler
{
    /// <summary>
    /// It's there to create a completely new game object for another playing round.
    ///
    /// It gets the default field layout from default field component.
    /// </summary>
    /// <returns>
    /// A normal starter game instance with default properties. 
    /// </returns>
    public static GameModel Default()
    {
        return new GameModel(
            new PlayerModel(null, ""),
            new PlayerModel(null, ""),
            FieldHandler.Default(),
            new int[] { 0, 0 },
            0,
            PlayingMode.Default,
            Winner.Default
        );
    }

    /// <summary>
    /// Prepares the game for a new game in the same game mode as last round.
    ///
    /// Just edits those properties that need to be touched.
    /// </summary>
    /// <param name="game">Game instance after a playing round.</param>
    /// <returns>A reset game object.</returns>
    public static GameModel Reset(GameModel game)
    {
        game.Field = FieldHandler.Default();
        game.Winner = Winner.Default;
        //gaming mode stays the same the player wanted to play a new game in the same playing mode as last time 
        //game.Mode = PlayingMode.Default;
        game.Round++;
        //game.PlayerOne ... -> stays the same
        //game.PlayerTwo ... - || -
        //game.Score = new int[] { }; -> updated in checker
        return game;
    }
}