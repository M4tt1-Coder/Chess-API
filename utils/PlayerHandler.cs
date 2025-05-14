using Chess_API.Enums;
using Chess_API.Models;

namespace Chess_API.utils;

/// <summary>
/// Represents a utility class that is responsible for handling player-related
/// operations in the context of the Chess API.
/// </summary>
/// <remarks>
/// This class may include methods and functionality for creating, updating,
/// retrieving, and managing player data or states. It is designed to
/// encapsulate the logic required for player management within the API
/// infrastructure.
/// </remarks>
public static class PlayerHandler
{
    /// <summary>
    /// Creates a new instance of the <see cref="PlayerModel"/> class by copying the data
    /// from an existing player instance.
    /// </summary>
    /// <param name="player">An instance of <see cref="PlayerModel"/> containing the player data to be copied.</param>
    /// <returns>A new <see cref="PlayerModel"/> instance with the same data as the provided player.</returns>
    public static PlayerModel CopyPlayer(PlayerModel player)
    {
        return new PlayerModel(
            player.StartingTime,
            player.Name,
            player.Score,
            player.PieceColor
        );
    }
    
    /// <summary>
    /// Determines the ID of the player whose turn it currently is in the provided game instance.
    /// </summary>
    /// <param name="game">An instance of the <see cref="GameModel"/> representing the current state of the chess game.</param>
    /// <returns>An integer representing the ID of the player whose turn it is.</returns>
    public static int GetPlayerIdOnTurn(GameModel game)
    {
        return game.PlayerTurn switch
        {
            PlayerTurn.White => game.PlayerOne.PieceColor == Colors.White
                ? game.PlayerOne.PlayerId
                : game.PlayerTwo.PlayerId,
            PlayerTurn.Black => game.PlayerOne.PieceColor == Colors.Black
                ? game.PlayerOne.PlayerId
                : game.PlayerTwo.PlayerId,
            _ => throw new Exception("Invalid player turn state.")
        };
    }

    /// <summary>
    /// Switches the current player's turn in the provided game instance.
    /// </summary>
    /// <param name="game">An instance of the <see cref="GameModel"/> representing the current state of the chess game.</param>
    public static void ChangePlayerTurn(GameModel game)
    {
        game.PlayerTurn = game.PlayerTurn == PlayerTurn.White ? PlayerTurn.Black : PlayerTurn.White;
    }
}