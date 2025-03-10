using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    /// Starting field for the move.
    /// </summary>
    [NotMapped]
    public IList<int> From { get; set; }
    
    /// <summary>
    /// Destination field for the move.
    /// </summary>
    [NotMapped]
    public IList<int> To { get; set; }
    
    /// <summary>
    /// Player who made the move.
    /// </summary>
    public int PlayerId { get; set; }
    
    public MoveModel(int moveNum, int figureId, IList<int> from, IList<int> to, int playerId)
    {
        MoveNum = moveNum;
        FigureId = figureId;
        From = from;
        To = to;
        PlayerId = playerId;
    }
    
    public MoveModel()
    {
    }
}