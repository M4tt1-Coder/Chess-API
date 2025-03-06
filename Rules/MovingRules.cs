using System.Runtime.InteropServices.ComTypes;
using Chess_API.Models;

namespace Chess_API.Rules;

// TODO - FIRST: Add all runners which move a piece to a specific new field

/// <summary>
/// Contains all checking functions if a piece can move to a new field.
///
/// Moves a piece to another field.
/// </summary>
public class MovingRules
{
    /// <summary>
    /// Logger for the MovingRules instance
    /// </summary>
    private readonly ILogger<MovingRules> _logger;

    public MovingRules(ILogger<MovingRules> logger)
    {
        _logger = logger;
    }

    // TODO - Implement a functions that checks if a piece can move to a selected field
    // TODO - Add a function that moves a piece to a new field without paying attention to possible checks
    /// <summary>
    /// 
    /// </summary>
    /// <param name="board"></param>
    /// <param name="pieceCoordinates"></param>
    /// <param name="destinationCoordinates"></param>
    /// <returns></returns>
    public bool CanPieceMoveToField(IList<FieldRowModel> board, IList<int> pieceCoordinates,
        IList<int> destinationCoordinates)
    {
        var output = false;
        // simple look if the piece can move to that field using its move patterns
        // check if a piece (of both colors) is in the way
        // field where piece currently is
        var curField = board[pieceCoordinates[1]].Row[pieceCoordinates[0]];
        if (curField.Content is null)
        {
            _logger.LogError("Coordinates of piece point to an empty field! {}", pieceCoordinates);
            return false;
        }

        var piece = curField.Content!;
        var destField = board[destinationCoordinates[1]].Row[destinationCoordinates[0]];
        // when the destination field contains a piece of the same color -> return false
        if (destField.Content is not null && destField.Content.Color == piece.Color)
        {
            return false;
        }
        // return output
        return output;
    }

    /// <summary>
    /// Simple checks if a moving piece can take place at a new field, while throwing an enemy-piece
    /// or can't because it's an own figure.
    ///
    /// Pays attention to the king's position and if it would be in check.
    /// </summary>
    /// <param name="game">Current game</param>
    /// <param name="coordinates">The coordinates of a field on which a specific piece is supposed to move.</param>
    /// <param name="figure">The type of the figure.</param>
    /// <returns>If a piece can take a field where another piece is situated.</returns>
    public bool CanPieceMoveToFieldWithCheck(GameModel game, IList<int> coordinates, FigureModel figure)
    {
        // check for extra conditions regarding the king

        // color of the figure on the field
        var figureColor = game.Field[coordinates[1]].Row[coordinates[0]].Color;
        // different color?
        if (figureColor != figure.Color)
        {
            // check if the figure can move to the new field
            // -> then determine if the own king would be in check
            return true;
        }
        else
        {
            return false;
        }
        // yes -> throw || no -> can't move 
    }
}