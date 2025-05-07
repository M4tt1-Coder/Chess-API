using Chess_API.Enums;
using Chess_API.Interfaces;
using Chess_API.Models;

namespace Chess_API.utils;

/// <summary>
/// Includes all methods to execute a step of a figure.
///
/// Single steps are executed by the methods.
///
/// Whole step repetitions are executed by the MovePatternExecutor.
/// </summary>
public static class StepExecutor
{
    /// <summary>
    /// Depending on the piece type, the move pattern is executed.
    ///
    /// Generally a possible check is ignored.
    /// </summary>
    /// <param name="game">Current game instance</param>
    /// <param name="movePattern">All move patterns of a specific piece</param>
    /// <param name="curField">Current position of the piece</param>
    /// <param name="newField">Destined field for the figure</param>
    /// <param name="type">Type of the figure</param>
    /// <returns>True, when a move pattern lead to the new field</returns>
    public static bool ExecuteMovePattern(GameModel game, IMovePattern movePattern, FieldModel curField, FieldModel newField,
        FigureType type)
    {
        var output = false;

        // check if the field is empty -> must contain a figure
        // when the field is the same as the new field -> return false
        if(curField.Content is null || curField == newField)
        {
            return false;
        }
        
        // Queen, Bishop & Rook can move over the whole board
        // King, Pawn & Knight have special move patterns and conditions
        // create 4 pattern executor methods
        
        // differentiate between repetitive and terminated move patterns -> infinite or not 
        if (movePattern.AreMovesInfinite)
        {
            foreach (var pattern in movePattern.Patterns)
            {
                foreach (var move in pattern)
                {
                    var canStillMove = true;
                    var nextField = FieldHandler.CopyField(curField);

                    while (canStillMove)
                    {
                        var previous = nextField;
                        // make a step
                        nextField = GoStepStraight(move, game, previous);
                        // when the next field is similar to the newField -> break
                        if (nextField == newField)
                        {
                            canStillMove = false;
                            output = true;
                        }
                        // check if the next field is the same as the previous one
                        if (nextField == previous)
                        {
                            canStillMove = false;
                        }
                    }
                }
            }    
        }
        else
        {
            var piece = curField.Content!;
            foreach (var pattern in movePattern.Patterns)
            {
                if (piece.Type == FigureType.Pawn)
                {
                    switch (Math.Abs(curField.Y - newField.Y))
                    {
                        case 2:
                        {
                            // check if the pawn has already been moved
                            if(MoveHistoryManager.HasPieceAlreadyMoved(game.MoveHistory, piece.FigureId))
                            {
                                break;
                            }
                            var nextField = FieldHandler.CopyField(curField);
                            foreach (var move in pattern)
                            {
                                nextField = GoStepPawn(move, game, nextField, piece.Color);
                                if (nextField == newField)
                                {
                                    output = true;
                                }
                            }
                            break;
                        }
                        case 1:
                        {
                            foreach (var move in pattern)
                            {
                                var nextField = GoStepPawn(move, game, FieldHandler.CopyField(curField), piece.Color);
                                if (nextField == newField)
                                {
                                    output = true;
                                }
                            }
                            break;
                        }
                    }
                }
                else if (piece.Type == FigureType.Knight)
                {
                    var nextField = FieldHandler.CopyField(curField);
                    foreach (var move in pattern)
                    {
                        nextField = GoStepKnight(move, game, nextField, piece.Color);
                        if (nextField == newField)
                        {
                            output = true;
                        }
                    }
                }
                else if (piece.Type == FigureType.King)
                {
                    foreach(var move in pattern)
                    {
                        var nextField = GoStepKing(move, game, FieldHandler.CopyField(curField), piece.Color);
                        if (nextField == newField)
                        {
                            output = true;
                        }
                    }
                }
            }
        }

        return output;
    }

    /// <summary>
    /// The knight has eight moving possibilities.
    /// 
    /// Checks if each of the eight fields is accessible. 
    /// </summary>
    /// <param name="move">The to going step.</param>
    /// <param name="game">Current game</param>
    /// <param name="curField">On which field we are standing.</param>
    /// <param name="knightColor">Color of the knight.</param>
    /// <param name="ignoreOpponentPieces">When the pieces of the opponent should be considered in the checking process or not</param>
    /// <returns>The next field of the pattern.</returns>
    public static FieldModel GoStepKnight(Moves move, GameModel game, FieldModel curField, Colors knightColor, bool ignoreOpponentPieces = false)
    {
        var output = new FieldModel();
        List<int> newCoordinates;
        
        // can jump over other figures

        bool sameColor;
        FieldModel? newField;
        switch (move)
        {
            case Moves.Up:
                // if (border check + is there a piece in the way -> stay on the field [possible throw])
                newCoordinates = new List<int>() { curField.X, curField.Y - 1 };
                output = newCoordinates[1] < 0 ? curField : FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                break;
            case Moves.Down:
                newCoordinates = new List<int>() { curField.X, curField.Y + 1 };
                output = newCoordinates[1] > 7 ? curField : FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                break;
            case Moves.Left:
                newCoordinates = new List<int>() { curField.X - 1, curField.Y };
                output = newCoordinates[0] < 0 ? curField : FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                break;
            case Moves.Right:
                newCoordinates = new List<int>() { curField.X + 1, curField.Y };
                output = newCoordinates[0] > 7 ? curField : FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                break;
            case Moves.DiagonalUpLeft:
                // is next field free? if not can the figure be thrown
                newCoordinates = new List<int>() { curField.X - 1, curField.Y - 1 };
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                sameColor = false;
                // check if the field is empty & if the knight can throw a piece
                if (newField.Content is not null && !ignoreOpponentPieces)
                {
                    sameColor = newField.Content.Color == knightColor;
                }
                
                if (newCoordinates[0] < 0 || newCoordinates[1] < 0 || sameColor)
                {
                    output = curField;
                }
                else
                {
                    output = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                }
                break;
            case Moves.DiagonalUpRight:
                newCoordinates = new List<int>() { curField.X + 1, curField.Y - 1 };
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                sameColor = false;
                if (newField.Content is not null && !ignoreOpponentPieces)
                {
                    sameColor = newField.Content.Color == knightColor;
                }

                if (newCoordinates[0] > 7 || newCoordinates[1] < 0 || sameColor)
                {
                    output = curField;
                }
                else
                {
                    output = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                }
                break;
            case Moves.DiagonalDownLeft:
                newCoordinates = new List<int>() { curField.X - 1, curField.Y + 1 };
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                sameColor = false;
                if (newField.Content is not null && !ignoreOpponentPieces)
                {
                    sameColor = newField.Content.Color == knightColor;
                }
                if (newCoordinates[0] < 0 || newCoordinates[1] > 7 || sameColor)
                {
                    output = curField;
                }
                else
                {
                    output = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                }
                break;
            case Moves.DiagonalDownRight:
                newCoordinates = new List<int>() { curField.X + 1, curField.Y + 1 };
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                sameColor = false;
                if (newField.Content is not null && !ignoreOpponentPieces)
                {
                    sameColor = newField.Content.Color == knightColor;
                }
                if (newCoordinates[0] > 7 || newCoordinates[1] > 7 || sameColor)
                {
                    output = curField;
                }
                else
                {
                    output = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                }
                break;
        }

        return output;
    }
    
    /// <summary>
    /// Executes one step of a pawn.
    ///
    /// Returns the current field when the move was not possible.
    /// </summary>
    /// <param name="move">Move of the pawn</param>
    /// <param name="game">Current game instance</param>
    /// <param name="curField">Current field of the pawn</param>
    /// <param name="pawnColor">Color of the pawn</param>
    /// <returns>The field the pawn is on in the next move</returns>
    private static FieldModel GoStepPawn(Moves move, GameModel game, FieldModel curField, Colors pawnColor)
    {
        // according to the color of the pawn, the direction of the movement changes
        if (!MovesPawnInRightDirection(pawnColor, move, game.Direction))
        {
            return curField;
        }
        
        var output = new FieldModel();
        List<int> newCoordinates;
        FieldModel newField;
        
        // cover all valid moves of the pawn
        switch (move)
        {
            case Moves.Up:
                // new coordinates
                newCoordinates = new List<int>() { curField.X, curField.Y - 1 };
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                // check if the field is empty + border check
                if (newCoordinates[1] < 0 || newField.Content is not null)
                {
                    output = curField;
                }
                else
                {
                    output = newField;
                }
                break;
            case Moves.Down:
                // new coordinates
                newCoordinates = new List<int>() { curField.X, curField.Y + 1 };
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                // check if the field is empty + border check
                if (newCoordinates[1] > 7 || newField.Content is not null)
                {
                    output = curField;
                }
                else
                {
                    output = newField;
                }
                break;
            case Moves.DiagonalUpLeft:
                newCoordinates = new List<int>() { curField.X - 1, curField.Y - 1 };
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                if (newCoordinates[0] < 0 || newCoordinates[1] < 0 || newField.Content is null ||
                    newField.Content.Color == pawnColor)
                {
                    output = curField;
                }
                else
                {
                    output = newField;
                }
                break;
            case Moves.DiagonalUpRight:
                newCoordinates = new List<int>() { curField.X + 1, curField.Y - 1 };
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                if (newCoordinates[0] > 7 || newCoordinates[1] < 0 || newField.Content is null ||
                    newField.Content.Color == pawnColor)
                {
                    output = curField;
                }
                else
                {
                    output = newField;
                }
                break;
            case Moves.DiagonalDownLeft:
                newCoordinates = new List<int>() { curField.X - 1, curField.Y + 1 };
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                if (newCoordinates[0] < 0 || newCoordinates[1] > 7 || newField.Content is null ||
                    newField.Content.Color == pawnColor)
                {
                    output = curField;
                }
                else
                {
                    output = newField;
                }
                break;
            case Moves.DiagonalDownRight:
                newCoordinates = new List<int>() { curField.X + 1, curField.Y + 1 };
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                if (newCoordinates[0] > 7 || newCoordinates[1] > 7 || newField.Content is null ||
                    newField.Content.Color == pawnColor)
                {
                    output = curField;
                }
                else
                {
                    output = newField;
                }
                break;
            case Moves.Left:
            case Moves.Right:   
                output = curField;
                break;
        }
        
        return output;
    }
    
    /// <summary>
    /// Executes one step of a king.
    ///
    /// Doesn't pay attention to the check.
    /// </summary>
    /// <param name="move">The move of the king</param>
    /// <param name="game">The current game instance</param>
    /// <param name="curField">The field the king is theoretically situated</param>
    /// <param name="kingColor">Color of the king</param>
    /// <returns>The new field of the kings position</returns>
    private static FieldModel GoStepKing(Moves move, GameModel game, FieldModel curField, Colors kingColor)
    {
        var output = new FieldModel();
        List<int> newCoordinates;
        FieldModel newField;

        switch (move)
        {
            case Moves.Up:
                newCoordinates = new List<int>() { curField.X, curField.Y - 1 };
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                if (newCoordinates[1] < 0 || (newField.Content is not null && newField.Content.Color == kingColor))
                {
                    output = curField;
                }
                else
                {
                    output = newField;
                }
                break;
            case Moves.Down:
                newCoordinates = new List<int>() { curField.X, curField.Y + 1 };
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                if (newCoordinates[1] > 7 || (newField.Content is not null && newField.Content.Color == kingColor))
                {
                    output = curField;
                }
                else
                {
                    output = newField;
                }
                break;
            case Moves.Left:
                newCoordinates = new List<int>() { curField.X- 1, curField.Y };
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                if (newCoordinates[0] < 0 || (newField.Content is not null && newField.Content.Color == kingColor))
                {
                    output = curField;
                }
                else
                {
                    output = newField;
                }
                break;
            case Moves.Right:
                newCoordinates = new List<int>() { curField.X + 1, curField.Y };
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                if (newCoordinates[0] > 7 || (newField.Content is not null && newField.Content.Color == kingColor))
                {
                    output = curField;
                }
                else
                {
                    output = newField;
                }
                break;
            case Moves.DiagonalUpLeft:
                newCoordinates = new List<int>() { curField.X - 1, curField.Y - 1 };
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                if (newCoordinates[0] < 0 || newCoordinates[1] < 0 || (newField.Content is not null && newField.Content.Color == kingColor))
                {
                    output = curField;
                }
                else
                {
                    output = newField;
                }
                break;
            case Moves.DiagonalUpRight:
                newCoordinates = new List<int>() { curField.X + 1, curField.Y - 1 };
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                if (newCoordinates[0] > 7 || newCoordinates[1] < 0 ||
                    (newField.Content is not null && newField.Content.Color == kingColor))
                {
                    output = curField;
                }
                else
                {
                    output = newField;
                }
                break;
            case Moves.DiagonalDownLeft:
                newCoordinates = new List<int>() { curField.X - 1, curField.Y + 1 };
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                if (newCoordinates[0] < 0 || newCoordinates[1] > 7 ||
                    (newField.Content is not null && newField.Content.Color == kingColor))
                {
                    output = curField;
                }
                else
                {
                    output = newField;
                }
                break;
            case Moves.DiagonalDownRight:
                newCoordinates = new List<int>() { curField.X + 1, curField.Y + 1 };
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                if (newCoordinates[0] > 7 || newCoordinates[1] > 7 ||
                    (newField.Content is not null && newField.Content.Color == kingColor))
                {
                    output = curField;
                }
                else
                {
                    output = newField;
                }
                break;
        }
        
        return output;
    }
    
    /// <summary>
    /// This function is for the BISHOP, ROOK and QUEEN!
    /// With straight movements.
    /// They have no special rules or steps, just their moving.
    /// </summary>
    /// <param name="move">Which move should executed.</param>
    /// <param name="game">Current game</param>
    /// <param name="curField">The field on which the checker is theoretically on.</param>
    /// <returns>The field the checker is currently on.</returns>
    public static FieldModel GoStepStraight(Moves move, GameModel game, FieldModel curField)
    {
        var output = new FieldModel();
        List<int> newCoordinates;
        // check for board borders + other pieces
        switch (move)
        {
            case Moves.Up:
                // if (border check + is there a piece i=n the way -> stay on the field [possible throw])
                newCoordinates = new List<int>() { curField.X, curField.Y - 1 };
                if (newCoordinates[1] < 0 || curField.Content is not null)
                {
                    output = curField;
                }
                else
                {
                    output = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                }
                break;
            case Moves.Down:
                newCoordinates = new List<int>() { curField.X, curField.Y + 1 };
                if (newCoordinates[1] > 7 || curField.Content is not null)
                {
                    output = curField;
                }
                else
                {
                    output = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                }
                break;
            case Moves.Left:
                newCoordinates = new List<int>() { curField.X - 1, curField.Y };
                if (newCoordinates[0] < 0 || curField.Content is not null)
                {
                    output = curField;
                }
                else
                {
                    output = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                }
                break;
            case Moves.Right:
                newCoordinates = new List<int>() { curField.X + 1, curField.Y };
                if (newCoordinates[0] > 7 || curField.Content is not null)
                {
                    output = curField;
                }
                else
                {
                    output = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                }
                break;
            case Moves.DiagonalUpLeft:
                newCoordinates = new List<int>() { curField.X - 1, curField.Y - 1 };
                if (newCoordinates[0] < 0 || newCoordinates[1] < 0 || curField.Content is not null)
                {
                    output = curField;
                }
                else
                {
                    output = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                }
                break;
            case Moves.DiagonalUpRight:
                newCoordinates = new List<int>() { curField.X + 1, curField.Y - 1 };
                if (newCoordinates[0] > 7 || newCoordinates[1] < 0 || curField.Content is not null)
                {
                    output = curField;
                }
                else
                {
                    output = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                }
                break;
            case Moves.DiagonalDownLeft:
                newCoordinates = new List<int>() { curField.X - 1, curField.Y + 1 };
                if (newCoordinates[0] < 0 || newCoordinates[1] > 7 || curField.Content is not null)
                {
                    output = curField;
                }
                else
                {
                    output = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                }
                break;
            case Moves.DiagonalDownRight:
                newCoordinates = new List<int>() { curField.X - 1, curField.Y + 1 };
                if (newCoordinates[0] > 7 || newCoordinates[1] > 7 || curField.Content is not null)
                {
                    output = curField;
                }
                else
                {
                    output = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                }
                break;
        }

        return output;
    }
    
    // helper functions _______

    
    
    /// <summary>
    /// Determines if a pawn moves in the right direction.
    ///
    /// Makes sure that a pawn can't move side ways.
    /// </summary>
    /// <param name="pawnColor">Color of the pawn</param>
    /// <param name="move">What move the has made</param>
    /// <param name="direction">In which direction both colors go (up OR down)</param>
    /// <returns>True, when the pawn moves in the right direction.</returns>
    /// <exception cref="ArgumentOutOfRangeException">When an invalid enum value was entered</exception>
    private static bool MovesPawnInRightDirection(Colors pawnColor, Moves move, PlayingDirection direction)
    {
        // a pawn can't move side ways
        if (move is Moves.Left or Moves.Right)
        {
            return false;
        }
        var output = false;

        switch (pawnColor)
        {
            case Colors.White:
                if ((move is Moves.Up or Moves.DiagonalUpLeft or Moves.DiagonalUpRight &&
                     direction == PlayingDirection.WhiteBottom) ||
                    (move is Moves.Down or Moves.DiagonalDownLeft or Moves.DiagonalDownRight &&
                     direction == PlayingDirection.WhiteTop))
                {
                    output = true;
                }
                break;
            case Colors.Black:
                if ((move is Moves.Up or Moves.DiagonalUpLeft or Moves.DiagonalUpRight &&
                     direction == PlayingDirection.WhiteTop) ||
                    (move is Moves.Down or Moves.DiagonalDownLeft or Moves.DiagonalDownRight &&
                     direction == PlayingDirection.WhiteBottom))
                {
                    output = true;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(pawnColor), pawnColor, null);
        }

        return output;
    }
}