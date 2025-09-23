using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Chess_API.Enums;
using Chess_API.utils;
using Chess_API.utils.Handlers;

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
    /// Uses the <see cref="Color"/> enumeration to define the possible values.
    /// </summary>
    public Color PieceColor { get; set; }

    public PlayerModel(TimeSpan? time, string name, int score, Color color)
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

    /// <summary>
    /// Prepares a player for a new game by setting their starting time
    /// and any other necessary initializations based on the selected time mode.
    /// </summary>
    /// <param name="timeMode">The time mode selected for the game that determines the player's starting time.</param>
    public void PrepPlayerForNewGame(PlayTimeMode timeMode)
    {
        // starting time 
        StartingTime = GameHandler.GetPlayerStartingTime(timeMode);
        // pieces
        Pieces = new List<FigureModel>();  
        // piece color
        PieceColor = PieceColor == Color.White ? Color.Black : Color.White;
    }
}