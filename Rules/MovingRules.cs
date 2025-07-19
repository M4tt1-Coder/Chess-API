using Chess_API.Enums;
using Chess_API.Interfaces;
using Chess_API.Models;
using Chess_API.MovePatterns;
using Chess_API.utils;

namespace Chess_API.Rules;

/// <summary>
/// Contains all checking functions if a piece can move to a new field.
///
/// Moves a piece to another field.
/// </summary>
public static class  MovingRules
{
    // TODO - Check if the validation of the move is correct
    
    // check rules doesnt work the right way
    
    /// <summary>
    /// Depending on the piece type, the function checks if a piece can move to a new field.
    ///
    /// Executes the move pattern of the piece.
    /// </summary>
    /// <param name="game">Current game instance</param>
    /// <param name="pieceCoordinates">Current coordinates of the piece</param>
    /// <param name="destinationCoordinates">Field coordinates of the new field</param>
    /// <returns>True, when the piece theoretically can move to a new field.</returns>
    private static bool CanPieceMoveToField(GameModel game, IList<int> pieceCoordinates,
        IList<int> destinationCoordinates)
    {
        // simple look if the piece can move to that field using its move patterns
        // check if a piece (of both colors) is in the way
        // field where piece currently is
        var curField = game.Board[pieceCoordinates[1]].Row[pieceCoordinates[0]];
        if (curField.Content is null)
        {
            return false;
        }

        var piece = curField.Content!;
        var destField = game.Board[destinationCoordinates[1]].Row[destinationCoordinates[0]];
        // execute pattern
        IMovePattern movePattern = piece.Type switch
        {
            FigureType.Pawn => new PawnMovePatterns(),
            FigureType.Rook => new RookMovePattern(),
            FigureType.Knight => new KnightMovePattern(),
            FigureType.Bishop => new BishopMovePattern(),
            FigureType.Queen => new QueenMovePatterns(),
            FigureType.King => new KingMovePatterns(),
            _ => new PawnMovePatterns()
        }; 
        
        // return
        return StepExecutor.ExecuteMovePattern(game, movePattern, curField, destField, piece.Type);
    }

    /// <summary>
    /// Simple checks if a moving piece can take place at a new field, while throwing an enemy-piece
    /// or can't because it's an own figure.
    ///
    /// Pays attention to the king's position and if it would be in check.
    /// </summary>
    /// <param name="game">Current game</param>
    /// <param name="pieceCoordinates">The coordinates of a field on which a specific piece is supposed to move.</param>
    /// <param name="newCoordinates">Coordinates of the new field</param>
    /// <returns>If a piece can take a field where another piece is situated.</returns>
    public static bool CanPieceMoveToFieldWithCheck(GameModel game, IList<int> pieceCoordinates, IList<int> newCoordinates)
    {
        // the field where is supposed to be can't be empty
        if (game.Board[pieceCoordinates[1]].Row[pieceCoordinates[0]].Content is null)
        {
            return false;
        }
        var output = false;
        // check if the piece can move to the new field
        var canMove = CanPieceMoveToField(game, pieceCoordinates, newCoordinates);
        // check if the own king would be in check
        var figureColor = game.Board[pieceCoordinates[1]].Row[pieceCoordinates[0]].Content!.Color;
        var kingInCheck = RulesExecutor.CheckChecker(game,figureColor, pieceCoordinates, newCoordinates);
        if (canMove && !kingInCheck)
        {
            output = true;
        }
        return output;
    }
}