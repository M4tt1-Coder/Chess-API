using System.ComponentModel.DataAnnotations.Schema;
using Chess_API.Enums;
using Chess_API.Models;

namespace Chess_API.Database;

/// <summary>
/// Its purpose is to be there as entity class for the in-memory database.
/// 
/// Biggest instance in the type tree.
///
/// Represents the whole game.
/// </summary>
public class GameModelEntity
{
    /// <summary>
    /// Stands for the player who is everytime a person not dependent on the game mode.
    /// </summary>
    public PlayerModel? PlayerOne { get; set; }

    /// <summary>
    /// Can be the Ai instance with the default name "Paul",
    /// or another normal player.
    /// </summary>
    public PlayerModel? PlayerTwo { get; set; }

    /// <summary>
    /// The actual game field the player interacts with.
    /// </summary>
    public List<FieldRowModel>? Field { get; set; }

    /// <summary>
    /// Counts how many rounds have been played in the specific playing mode.
    /// </summary>
    public int Round { get; set; }

    /// <summary>
    /// When some games were played there will be a score with the win proportion.
    /// </summary>
    [NotMapped]
    public int[]? Score { get; set; }

    /// <summary>
    /// It's content is the chosen game mode of the player one.
    /// </summary>
    public PlayingMode Mode { get; set; }

    /// <summary>
    /// Sets if some player won, nobody won or the game haven't been decided already.
    /// </summary>
    public Winner Winner { get; set; }
}