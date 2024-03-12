using System.ComponentModel.DataAnnotations;
using Chess_API.Enums;

namespace Chess_API.Models;

/// <summary>
/// Biggest instance in the type tree.
///
/// Represents the whole game.
/// </summary>
public class GameModel
{
    /// <summary>
    /// Represents the identifier of the current game instance in the database.
    /// </summary>
    [Key]
    public int GameId { get; set; }
    
    /// <summary>
    /// Stands for the player who is everytime a person not dependent on the game mode.
    /// </summary>
    public PlayerModel PlayerOne { get; set; }

    /// <summary>
    /// Can be the Ai instance with the default name "Paul",
    /// or another normal player.
    /// </summary>
    public PlayerModel PlayerTwo { get; set; }

    /// <summary>
    /// The actual game field the player interacts with.
    /// </summary>
    
    public List<FieldRowModel> Field { get; set; }

    /// <summary>
    /// Counts how many rounds have been played in the specific playing mode.
    /// </summary>
    public int Round { get; set; }

    /// <summary>
    /// It's content is the chosen game mode of the player one.
    /// </summary>
    public PlayingMode Mode { get; set; }

    /// <summary>
    /// Sets if some player won, nobody won or the game haven't been decided already.
    /// </summary>
    public Winner Winner { get; set; }

    /// <summary>
    /// Is null when the players want to customize their time OR when they want no time limit.
    /// </summary>
    public PlayTimeMode PlayTimeMode { get; set; }
    
    public GameModel(PlayerModel playerOne, PlayerModel playerTwo, List<FieldRowModel> field, int round,
        PlayingMode mode, Winner winner)
    {
        PlayerOne = playerOne;
        PlayerTwo = playerTwo;
        Field = field;
        Round = round;
        Mode = mode;
        Winner = winner;
    }

    public GameModel()
    {
    }
}