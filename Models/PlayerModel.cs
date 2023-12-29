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
    /// When the player sets a time limit it's stored in here.
    /// </summary>
    public TimeSpan Time { get; set; }

    /// <summary>
    /// The pieces the player removed from the board of the opponent.
    /// </summary>
    public List<FigureModel>? Pieces { get; set; }

    /// <summary>
    /// A random name the player can set.
    /// </summary>
    public string? Name { get; set; }

    public PlayerModel(TimeSpan time, string name)
    {
        Time = time;
        Pieces = new List<FigureModel>();
        Name = name;
    }
}