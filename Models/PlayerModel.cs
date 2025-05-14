using System.ComponentModel.DataAnnotations;
using Chess_API.Enums;

namespace Chess_API.Models;

/// <summary>
/// Represents a player of two in the game.
///
/// A player can be an AI instance when the player chooses that specific game-mode.
///
/// It consists of all necessary information for the dashboard.
/// </summary>
public class PlayerModel
{
    /// <summary>
    /// Represents the ID and additionally the player number one or two.
    ///
    /// Can't be null.
    /// </summary>
    [Key]
    public int PlayerId { get; set; }

    /// <summary>
    /// Represents how many rounds a player has won.
    /// </summary>
    public int Score { get; set; }

    /// <summary>
    /// Should represent the choice of the user when he entered a customized time.
    ///
    /// Is null when the player chose a time mode.
    /// </summary>
    public TimeSpan? StartingTime { get; set; }
    
    //public double? Seconds { get; set; }

    /// <summary>
    /// The pieces the player removed from the board of the opponent.
    /// </summary>
    [Required]
    public ICollection<FigureModel>? Pieces { get; set; }

    /// <summary>
    /// A random name the player can set.
    /// </summary>
    [MaxLength(25)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Specifies the color of the chess pieces that the player controls.
    /// Uses the <see cref="Chess_API.Enums.Colors"/> enumeration to define the possible values.
    /// </summary>
    public Colors PieceColor { get; set; }

    public PlayerModel(TimeSpan? time, string name, int score, Colors color)
    {
        PieceColor = color;
        Score = score;
        StartingTime = time;
        Pieces = new List<FigureModel>();
        Name = name;
    }
    

    public PlayerModel()
    {
    }
}