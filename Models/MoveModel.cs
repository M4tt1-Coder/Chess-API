using System.ComponentModel.DataAnnotations;

namespace Chess_API.Models;

/// <summary>
/// Represents a move in the game.
///
/// When a player made a valid a move it will be stored in the database.
/// </summary>
public class MoveModel
{
    /// <summary>
    /// Represents the identifier for a move.
    ///
    /// Can't be null.
    /// </summary>
    [Key]
    public int MoveId { get; set; }

    /// <summary>
    /// Number of the current move.
    /// </summary>
    public int MoveNum { get; set; }

    /// <summary>
    /// Id of the piece which moved.
    /// </summary>
    public int FigureId { get; set; }

    /// <summary>
    /// Represents the X-coordinate of the starting position for a move.
    /// </summary>
    public int FromX { get; set; }

    /// <summary>
    /// Represents the Y-coordinate of the starting position of a chess piece in a move.
    /// </summary>
    public int FromY { get; set; }

    /// <summary>
    /// Represents the X-coordinate of the destination position in a move.
    /// Indicates where a piece is moved along the horizontal axis.
    /// </summary>
    public int ToX { get; set; }

    /// <summary>
    /// Represents the vertical coordinate of the destination position for a move.
    /// </summary>
    public int ToY { get; set; }

    /// <summary>
    /// Player who made the move.
    /// </summary>
    public int PlayerId { get; set; }

    public MoveModel(int moveNum, int figureId, IList<int> from, IList<int> to, int playerId)
    {
        MoveNum = moveNum;
        FigureId = figureId;
        FromX = from[0];
        FromY = from[1];
        ToX = to[0];
        ToY = to[1];
        PlayerId = playerId;
    }

    public MoveModel() { }
}
