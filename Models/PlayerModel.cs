using System.ComponentModel.DataAnnotations;

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
    
    /// <summary>
    /// When the player sets a time limit, it's stored in here.
    ///
    /// It counts the seconds of the remaining seconds.
    /// </summary>
    public double? Seconds { get; set; }

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

    public PlayerModel(TimeSpan? time, string name, int score)
    {
        Score = score;
        StartingTime = time;
        Pieces = new List<FigureModel>();
        Name = name;
    }

    public PlayerModel()
    {
    }
}