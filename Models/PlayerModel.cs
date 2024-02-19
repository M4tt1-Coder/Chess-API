using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Chess_API.Models;

/// <summary>
/// Represents a player of two in on game.
///
/// A player can be an AI-instance when the player chooses that specific game-mode.
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
    /// When the player sets a time limit it's stored in here.
    /// </summary>
    public TimeSpan? Time { get; set; }

    /// <summary>
    /// The pieces the player removed from the board of the opponent.
    /// </summary>
    [Required]
    public List<FigureModel> Pieces { get; set; }

    /// <summary>
    /// A random name the player can set.
    /// </summary>
    [MaxLength(25)]
    public string Name { get; set; }

    public PlayerModel(TimeSpan? time, string name, int score)
    {
        Score = score;
        Time = time;
        Pieces = new List<FigureModel>();
        Name = name;
    }

    public PlayerModel()
    {
    }
}