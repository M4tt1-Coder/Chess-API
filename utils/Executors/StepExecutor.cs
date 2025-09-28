using Chess_API.Enums;
using Chess_API.Interfaces;
using Chess_API.Models;
using Chess_API.MovePatterns;
using Chess_API.Rules;
using Chess_API.utils.Handlers;

namespace Chess_API.utils.Executors;

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
    public static bool ExecuteMovePattern(
        GameModel game,
        IMovePattern movePattern,
        FieldModel curField,
        FieldModel newField,
        FigureType type
    )
    {
        var output = false;

        // check if the field is empty -> must contain a figure
        // when the field is the same as the new field -> return false
        if (
            curField.Content is null
            || curField == newField
            ||
            // make sure that the potential piece on both fields don't have the same color
            (newField.Content is not null && newField.Content.Color == curField.Content.Color)
        )
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
                    if (output)
                        continue;
                    var canStillMove = true;
                    var nextField = FieldHandler.CopyField(curField);
                    var iterationCounter = 0;

                    // go one step before entering the loop -------
                    nextField = GoStepStraight(move, game, nextField, curField.Content.Color, true);

                    // also check if the first step was on the destination field
                    if (nextField == newField)
                    {
                        canStillMove = false;
                        output = true;
                    }

                    while (canStillMove)
                    {
                        var previous = nextField;
                        // make a step -------
                        nextField = GoStepStraight(move, game, previous, curField.Content.Color);
                        iterationCounter++;

                        // when the next field is similar to the newField -> break
                        if (nextField == newField)
                        {
                            canStillMove = false;
                            output = true;
                        }
                        // check if the next field is the same as the previous one
                        if (nextField == previous || iterationCounter >= 7)
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
                switch (piece.Type)
                {
                    case FigureType.Pawn:
                        switch (Math.Abs(curField.Y - newField.Y))
                        {
                            case 2:
                            {
                                // check if the pawn has already been moved
                                if (
                                    MoveHistoryHandler.HasPieceAlreadyMoved(
                                        game.MoveHistory,
                                        piece.FigureId
                                    )
                                )
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
                                    var nextField = GoStepPawn(
                                        move,
                                        game,
                                        FieldHandler.CopyField(curField),
                                        piece.Color
                                    );
                                    if (nextField == newField)
                                    {
                                        output = true;
                                    }
                                }
                                break;
                            }
                        }

                        break;
                    case FigureType.Knight:
                    {
                        var nextField = FieldHandler.CopyField(curField);
                        var stepCounter = 0;

                        foreach (var move in pattern)
                        {
                            var previous = nextField;
                            nextField = GoStepKnight(move, game, nextField, piece.Color);
                            stepCounter++;
                            if (nextField == newField && previous != nextField && stepCounter == 2)
                            {
                                output = true;
                            }
                        }

                        break;
                    }
                    case FigureType.King:
                    {
                        foreach (var move in pattern)
                        {
                            var nextField = GoStepKing(
                                move,
                                game,
                                FieldHandler.CopyField(curField),
                                piece.Color
                            );
                            if (nextField == newField)
                            {
                                output = true;
                            }
                        }

                        break;
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
    public static FieldModel GoStepKnight(
        Move move,
        GameModel game,
        FieldModel curField,
        Color knightColor,
        bool ignoreOpponentPieces = false
    )
    {
        var output = new FieldModel();
        List<int> newCoordinates;

        // can jump over other figures

        bool sameColor;
        FieldModel? newField;
        switch (move)
        {
            case Move.Up:
                // if (border check + is there a piece in the way -> stay on the field [possible throw])
                newCoordinates = [curField.X, curField.Y - 1];
                output =
                    newCoordinates[1] < 0
                        ? curField
                        : FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                break;
            case Move.Down:
                newCoordinates = [curField.X, curField.Y + 1];
                output =
                    newCoordinates[1] > 7
                        ? curField
                        : FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                break;
            case Move.Left:
                newCoordinates = [curField.X - 1, curField.Y];
                output =
                    newCoordinates[0] < 0
                        ? curField
                        : FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                break;
            case Move.Right:
                newCoordinates = [curField.X + 1, curField.Y];
                output =
                    newCoordinates[0] > 7
                        ? curField
                        : FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                break;
            case Move.DiagonalUpLeft:
                // is next field free? if not can the figure be thrown
                newCoordinates = [curField.X - 1, curField.Y - 1];
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
            case Move.DiagonalUpRight:
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
            case Move.DiagonalDownLeft:
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
            case Move.DiagonalDownRight:
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
    private static FieldModel GoStepPawn(
        Move move,
        GameModel game,
        FieldModel curField,
        Color pawnColor
    )
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
            case Move.Up:
                // new coordinates
                newCoordinates = [curField.X, curField.Y - 1];
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
            case Move.Down:
                // new coordinates
                newCoordinates = [curField.X, curField.Y + 1];
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
            case Move.DiagonalUpLeft:
                newCoordinates = [curField.X - 1, curField.Y - 1];
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                if (
                    newCoordinates[0] < 0
                    || newCoordinates[1] < 0
                    || newField.Content is null
                    || newField.Content.Color == pawnColor
                )
                {
                    output = curField;
                }
                else
                {
                    output = newField;
                }
                break;
            case Move.DiagonalUpRight:
                newCoordinates = [curField.X + 1, curField.Y - 1];
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                if (
                    newCoordinates[0] > 7
                    || newCoordinates[1] < 0
                    || newField.Content is null
                    || newField.Content.Color == pawnColor
                )
                {
                    output = curField;
                }
                else
                {
                    output = newField;
                }
                break;
            case Move.DiagonalDownLeft:
                newCoordinates = [curField.X - 1, curField.Y + 1];
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                if (
                    newCoordinates[0] < 0
                    || newCoordinates[1] > 7
                    || newField.Content is null
                    || newField.Content.Color == pawnColor
                )
                {
                    output = curField;
                }
                else
                {
                    output = newField;
                }
                break;
            case Move.DiagonalDownRight:
                newCoordinates = [curField.X + 1, curField.Y + 1];
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                if (
                    newCoordinates[0] > 7
                    || newCoordinates[1] > 7
                    || newField.Content is null
                    || newField.Content.Color == pawnColor
                )
                {
                    output = curField;
                }
                else
                {
                    output = newField;
                }
                break;
            case Move.Left:
            case Move.Right:
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
    public static FieldModel GoStepKing(
        Move move,
        GameModel game,
        FieldModel curField,
        Color kingColor
    )
    {
        var output = new FieldModel();
        List<int> newCoordinates;
        FieldModel newField;

        switch (move)
        {
            case Move.Up:
                newCoordinates = [curField.X, curField.Y - 1];
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                if (
                    newCoordinates[1] < 0
                    || (newField.Content is not null && newField.Content.Color == kingColor)
                )
                {
                    output = curField;
                }
                else
                {
                    output = newField;
                }
                break;
            case Move.Down:
                newCoordinates = [curField.X, curField.Y + 1];
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                if (
                    newCoordinates[1] > 7
                    || (newField.Content is not null && newField.Content.Color == kingColor)
                )
                {
                    output = curField;
                }
                else
                {
                    output = newField;
                }
                break;
            case Move.Left:
                newCoordinates = [curField.X - 1, curField.Y];
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                if (
                    newCoordinates[0] < 0
                    || (newField.Content is not null && newField.Content.Color == kingColor)
                )
                {
                    output = curField;
                }
                else
                {
                    output = newField;
                }
                break;
            case Move.Right:
                newCoordinates = [curField.X + 1, curField.Y];
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                if (
                    newCoordinates[0] > 7
                    || (newField.Content is not null && newField.Content.Color == kingColor)
                )
                {
                    output = curField;
                }
                else
                {
                    output = newField;
                }
                break;
            case Move.DiagonalUpLeft:
                newCoordinates = [curField.X - 1, curField.Y - 1];
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                if (
                    newCoordinates[0] < 0
                    || newCoordinates[1] < 0
                    || (newField.Content is not null && newField.Content.Color == kingColor)
                )
                {
                    output = curField;
                }
                else
                {
                    output = newField;
                }
                break;
            case Move.DiagonalUpRight:
                newCoordinates = new List<int>() { curField.X + 1, curField.Y - 1 };
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                if (
                    newCoordinates[0] > 7
                    || newCoordinates[1] < 0
                    || (newField.Content is not null && newField.Content.Color == kingColor)
                )
                {
                    output = curField;
                }
                else
                {
                    output = newField;
                }
                break;
            case Move.DiagonalDownLeft:
                newCoordinates = new List<int>() { curField.X - 1, curField.Y + 1 };
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                if (
                    newCoordinates[0] < 0
                    || newCoordinates[1] > 7
                    || (newField.Content is not null && newField.Content.Color == kingColor)
                )
                {
                    output = curField;
                }
                else
                {
                    output = newField;
                }
                break;
            case Move.DiagonalDownRight:
                newCoordinates = new List<int>() { curField.X + 1, curField.Y + 1 };
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                if (
                    newCoordinates[0] > 7
                    || newCoordinates[1] > 7
                    || (newField.Content is not null && newField.Content.Color == kingColor)
                )
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
    /// <param name="ignoreCurrentFieldContent">Whether to ignore the current file or not</param>
    /// <param name="figureColor">Color of the figure that should move</param>
    /// <returns>The field the checker is currently on.</returns>
    public static FieldModel GoStepStraight(
        Move move,
        GameModel game,
        FieldModel curField,
        Color figureColor,
        bool ignoreCurrentFieldContent = false
    )
    {
        FieldModel output;
        FieldModel newField;
        List<int> newCoordinates;
        // check for board borders + other pieces
        switch (move)
        {
            case Move.Up:
                // if (border check + is there a piece i=n the way -> stay on the field [possible throw])
                newCoordinates = new List<int>() { curField.X, curField.Y - 1 };
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                if (
                    newCoordinates[1] < 0
                    || (curField.Content is not null && !ignoreCurrentFieldContent)
                    || (
                        newField.Content is not null
                        && newField.Content.Color == figureColor
                        && !ignoreCurrentFieldContent
                    )
                )
                {
                    output = curField;
                }
                else
                {
                    output = newField;
                }
                break;
            case Move.Down:
                newCoordinates = [curField.X, curField.Y + 1];
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                if (
                    newCoordinates[1] > 7
                    || (curField.Content is not null && !ignoreCurrentFieldContent)
                    || (
                        newField.Content is not null
                        && newField.Content.Color == figureColor
                        && !ignoreCurrentFieldContent
                    )
                )
                {
                    output = curField;
                }
                else
                {
                    output = newField;
                }
                break;
            case Move.Left:
                newCoordinates = [curField.X - 1, curField.Y];
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                if (
                    newCoordinates[0] < 0
                    || (curField.Content is not null && !ignoreCurrentFieldContent)
                    || (
                        newField.Content is not null
                        && newField.Content.Color == figureColor
                        && !ignoreCurrentFieldContent
                    )
                )
                {
                    output = curField;
                }
                else
                {
                    output = newField;
                }
                break;
            case Move.Right:
                newCoordinates = [curField.X + 1, curField.Y];
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                if (
                    newCoordinates[0] > 7
                    || (curField.Content is not null && !ignoreCurrentFieldContent)
                    || (
                        newField.Content is not null
                        && newField.Content.Color == figureColor
                        && !ignoreCurrentFieldContent
                    )
                )
                {
                    output = curField;
                }
                else
                {
                    output = newField;
                }
                break;
            case Move.DiagonalUpLeft:
                newCoordinates = [curField.X - 1, curField.Y - 1];
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                if (
                    newCoordinates[0] < 0
                    || newCoordinates[1] < 0
                    || (curField.Content is not null && !ignoreCurrentFieldContent)
                    || (
                        newField.Content is not null
                        && newField.Content.Color == figureColor
                        && !ignoreCurrentFieldContent
                    )
                )
                {
                    output = curField;
                }
                else
                {
                    output = newField;
                }
                break;
            case Move.DiagonalUpRight:
                newCoordinates = [curField.X + 1, curField.Y - 1];
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                if (
                    newCoordinates[0] > 7
                    || newCoordinates[1] < 0
                    || (curField.Content is not null && !ignoreCurrentFieldContent)
                    || (
                        newField.Content is not null
                        && newField.Content.Color == figureColor
                        && !ignoreCurrentFieldContent
                    )
                )
                {
                    output = curField;
                }
                else
                {
                    output = newField;
                }
                break;
            case Move.DiagonalDownLeft:
                newCoordinates = [curField.X - 1, curField.Y + 1];
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                if (
                    newCoordinates[0] < 0
                    || newCoordinates[1] > 7
                    || (curField.Content is not null && !ignoreCurrentFieldContent)
                    || (
                        newField.Content is not null
                        && newField.Content.Color == figureColor
                        && !ignoreCurrentFieldContent
                    )
                )
                {
                    output = curField;
                }
                else
                {
                    output = newField;
                }
                break;
            case Move.DiagonalDownRight:
                newCoordinates = [curField.X + 1, curField.Y + 1];
                newField = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                if (
                    newCoordinates[0] > 7
                    || newCoordinates[1] > 7
                    || (curField.Content is not null && !ignoreCurrentFieldContent)
                    || (
                        newField.Content is not null
                        && newField.Content.Color == figureColor
                        && !ignoreCurrentFieldContent
                    )
                )
                {
                    output = curField;
                }
                else
                {
                    output = newField;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(move), move, null);
        }

        return output;
    }

    // helper functions ______

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
    private static bool MovesPawnInRightDirection(
        Color pawnColor,
        Move move,
        PlayingDirection direction
    )
    {
        // a pawn can't move side ways
        if (move is Move.Left or Move.Right)
        {
            return false;
        }
        var output = false;

        switch (pawnColor)
        {
            case Color.White:
                if (
                    (
                        move is Move.Up or Move.DiagonalUpLeft or Move.DiagonalUpRight
                        && direction == PlayingDirection.WhiteBottom
                    )
                    || (
                        move is Move.Down or Move.DiagonalDownLeft or Move.DiagonalDownRight
                        && direction == PlayingDirection.WhiteTop
                    )
                )
                {
                    output = true;
                }
                break;
            case Color.Black:
                if (
                    (
                        move is Move.Up or Move.DiagonalUpLeft or Move.DiagonalUpRight
                        && direction == PlayingDirection.WhiteTop
                    )
                    || (
                        move is Move.Down or Move.DiagonalDownLeft or Move.DiagonalDownRight
                        && direction == PlayingDirection.WhiteBottom
                    )
                )
                {
                    output = true;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(pawnColor), pawnColor, null);
        }

        return output;
    }

    /// <summary>
    /// Validates if a piece can make a move and returns the result.
    /// </summary>
    /// <param name="game">The current Game instance</param>
    /// <param name="pieceCoordinates">The coordinates of the field where the figure is placed.</param>
    /// <returns>The test result if a piece can move or not.</returns>
    /// <exception cref="Exception">When mandatory data is not provided or any error occured</exception>
    public static bool TestIfPieceCanMakeAMove(GameModel game, List<int> pieceCoordinates)
    {
        var fieldOfPiece = FieldHandler.GetSpecificFieldByCoordinates(game, pieceCoordinates);

        if (fieldOfPiece.Content is null)
            throw new BadHttpRequestException("Considered field with figure on it, is empty!");

        // determine move pattern
        var movePattern = MovingRules.DetermineMovePatternsByFigureType(fieldOfPiece.Content.Type);

        // check if the piece is present
        if (movePattern.AreMovesInfinite)
        {
            foreach (var pattern in movePattern.Patterns)
            {
                foreach (var move in pattern)
                {
                    var nextField = FieldHandler.CopyField(fieldOfPiece);

                    // go one step before entering the loop -------
                    nextField = GoStepStraight(
                        move,
                        game,
                        nextField,
                        fieldOfPiece.Content.Color,
                        true
                    );

                    // also check if the first step was on the destination field
                    if (nextField != fieldOfPiece)
                    {
                        return true;
                    }
                }
            }
        }
        else
        {
            var piece = fieldOfPiece.Content!;
            foreach (var pattern in movePattern.Patterns)
            {
                switch (piece.Type)
                {
                    case FigureType.Pawn:
                        // ReSharper disable once PossibleMultipleEnumeration
                        if (pattern.Count() == 2)
                        {
                            if (
                                MoveHistoryHandler.HasPieceAlreadyMoved(
                                    game.MoveHistory,
                                    piece.FigureId
                                )
                            )
                            {
                                break;
                            }

                            var nextField = FieldHandler.CopyField(fieldOfPiece);
                            // ReSharper disable once PossibleMultipleEnumeration
                            foreach (var move in pattern)
                            {
                                nextField = GoStepPawn(move, game, nextField, piece.Color);
                                if (nextField != fieldOfPiece)
                                {
                                    return true;
                                }
                            }
                        }
                        else
                        {
                            // ReSharper disable once PossibleMultipleEnumeration
                            if (
                                pattern
                                    .Select(move =>
                                        GoStepPawn(
                                            move,
                                            game,
                                            FieldHandler.CopyField(fieldOfPiece),
                                            piece.Color
                                        )
                                    )
                                    .Any(nextField => nextField != fieldOfPiece)
                            )
                            {
                                return true;
                            }
                        }

                        break;
                    case FigureType.Knight:
                    {
                        var nextField = FieldHandler.CopyField(fieldOfPiece);
                        var stepCounter = 0;

                        foreach (var move in pattern)
                        {
                            var previous = nextField;
                            nextField = GoStepKnight(move, game, nextField, piece.Color);
                            stepCounter++;
                            if (
                                nextField != fieldOfPiece
                                && previous != nextField
                                && stepCounter == 2
                            )
                            {
                                return true;
                            }
                        }

                        break;
                    }
                    case FigureType.King:
                    {
                        if (
                            pattern
                                .Select(move =>
                                    GoStepKing(
                                        move,
                                        game,
                                        FieldHandler.CopyField(fieldOfPiece),
                                        piece.Color
                                    )
                                )
                                .Any(nextField => nextField != fieldOfPiece)
                        )
                        {
                            return true;
                        }

                        break;
                    }
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Gathers all fields where a piece can move to in the next move.
    /// </summary>
    /// <param name="game">The current Game instance</param>
    /// <param name="pieceCoordinates">The coordinates of the piece.</param>
    /// <returns>List of field coordinates the piece can move to.</returns>
    public static List<List<int>> FieldsWherePieceCanMoveTo(
        GameModel game,
        List<int> pieceCoordinates
    )
    {
        var fieldsWherePieceCanMove = new List<List<int>>();

        // get the field of the piece
        var fieldOfPiece = FieldHandler.GetSpecificFieldByCoordinates(game, pieceCoordinates);

        if (fieldOfPiece.Content is null)
            throw new Exception("The content of the field can't be null!");

        switch (fieldOfPiece.Content.Type)
        {
            case FigureType.Pawn:
                // for pawns, I need to know in which direction they are turned to
                switch (game.Direction)
                {
                    case PlayingDirection.WhiteTop:
                        if (fieldOfPiece.Content.Color == Color.White)
                        {
                            // check for boundaries
                            // assign new possible fields that could be attacked by black piece
                            List<int> firstField = [fieldOfPiece.X - 1, fieldOfPiece.Y - 1];
                            List<int> secondField = [fieldOfPiece.X + 1, fieldOfPiece.Y - 1];

                            if (RulesExecutor.AreCoordinatesOnBoard(firstField))
                            {
                                RulesExecutor.AddCoordinatesToList(
                                    fieldsWherePieceCanMove,
                                    firstField
                                );
                            }

                            if (RulesExecutor.AreCoordinatesOnBoard(secondField))
                            {
                                RulesExecutor.AddCoordinatesToList(
                                    fieldsWherePieceCanMove,
                                    secondField
                                );
                            }
                        }
                        else
                        {
                            var firstField = new List<int>
                            {
                                fieldOfPiece.X - 1,
                                fieldOfPiece.Y + 1,
                            };
                            var secondField = new List<int>
                            {
                                fieldOfPiece.X + 1,
                                fieldOfPiece.Y + 1,
                            };

                            if (RulesExecutor.AreCoordinatesOnBoard(firstField))
                            {
                                RulesExecutor.AddCoordinatesToList(
                                    fieldsWherePieceCanMove,
                                    firstField
                                );
                            }

                            if (RulesExecutor.AreCoordinatesOnBoard(secondField))
                            {
                                RulesExecutor.AddCoordinatesToList(
                                    fieldsWherePieceCanMove,
                                    secondField
                                );
                            }
                        }
                        break;
                    case PlayingDirection.WhiteBottom:
                        if (fieldOfPiece.Content.Color == Color.White)
                        {
                            var firstField = new List<int>
                            {
                                fieldOfPiece.X - 1,
                                fieldOfPiece.Y + 1,
                            };
                            var secondField = new List<int>
                            {
                                fieldOfPiece.X + 1,
                                fieldOfPiece.Y + 1,
                            };

                            if (RulesExecutor.AreCoordinatesOnBoard(firstField))
                            {
                                RulesExecutor.AddCoordinatesToList(
                                    fieldsWherePieceCanMove,
                                    firstField
                                );
                            }

                            if (RulesExecutor.AreCoordinatesOnBoard(secondField))
                            {
                                RulesExecutor.AddCoordinatesToList(
                                    fieldsWherePieceCanMove,
                                    secondField
                                );
                            }
                        }
                        else
                        {
                            var firstField = new List<int>
                            {
                                fieldOfPiece.X - 1,
                                fieldOfPiece.Y - 1,
                            };
                            var secondField = new List<int>
                            {
                                fieldOfPiece.X + 1,
                                fieldOfPiece.Y - 1,
                            };

                            if (RulesExecutor.AreCoordinatesOnBoard(firstField))
                            {
                                RulesExecutor.AddCoordinatesToList(
                                    fieldsWherePieceCanMove,
                                    firstField
                                );
                            }

                            if (RulesExecutor.AreCoordinatesOnBoard(secondField))
                            {
                                RulesExecutor.AddCoordinatesToList(
                                    fieldsWherePieceCanMove,
                                    secondField
                                );
                            }
                        }
                        break;
                }
                break;
            case FigureType.Bishop:
                var bishopMovePatterns = new BishopMovePattern().Patterns.ToList();
                // run along the move patterns
                foreach (var pattern in bishopMovePatterns)
                {
                    var canStillContinue = true;
                    var nextField = FieldHandler.CopyField(fieldOfPiece);

                    var currentPattern = pattern.ToList();

                    // go one default step before starting the repetitive step execution
                    nextField = StepExecutor.GoStepStraight(
                        currentPattern[0],
                        game,
                        nextField,
                        fieldOfPiece.Content.Color == Color.White ? Color.Black : Color.White,
                        true
                    );
                    // also add the additional field to the output list
                    if (RulesExecutor.AreCoordinatesOnBoard([nextField.X, nextField.Y]))
                    {
                        var coordinatesToBeAdded = new List<int> { nextField.X, nextField.Y };
                        RulesExecutor.AddCoordinatesToList(
                            fieldsWherePieceCanMove,
                            coordinatesToBeAdded
                        );
                    }

                    // repeat to go as long as possible along one pattern
                    // just for straight move pattern
                    while (canStillContinue)
                    {
                        var previousField = nextField;

                        // go along the pattern
                        nextField = currentPattern.Aggregate(
                            nextField,
                            (current, move) =>
                                StepExecutor.GoStepStraight(
                                    move,
                                    game,
                                    current,
                                    fieldOfPiece.Content.Color == Color.White
                                        ? Color.Black
                                        : Color.White
                                )
                        );

                        // check if the field where the figure has moved has changed
                        if (previousField.X == nextField.X && previousField.Y == nextField.Y)
                        {
                            canStillContinue = false;
                        }
                        else
                        {
                            // add the field to the list
                            if (!RulesExecutor.AreCoordinatesOnBoard([nextField.X, nextField.Y]))
                                continue;
                            List<int> coordinatesToBeAdded = [nextField.X, nextField.Y];
                            RulesExecutor.AddCoordinatesToList(
                                fieldsWherePieceCanMove,
                                coordinatesToBeAdded
                            );
                        }
                    }
                }
                break;
            case FigureType.Knight:
                // do the same logic for the knight as for the bishop
                // all eight field can be added
                foreach (var pattern in new KnightMovePattern().Patterns)
                {
                    // assign field where knight stands before iterating the patterns
                    var nextField = FieldHandler.CopyField(fieldOfPiece);
                    var moveCount = 0;
                    // when it reaches 2 -> knight could land on that spot
                    foreach (var move in pattern)
                    {
                        var previousField = nextField;
                        // first go the step
                        nextField = StepExecutor.GoStepKnight(
                            move,
                            game,
                            previousField,
                            fieldOfPiece.Content.Color,
                            true
                        );
                        // increase counter by 1
                        moveCount++;
                        // when 2 steps have been taken -> check if previous field is equal to field after
                        // the moving operation -> continue
                        if (moveCount != 2)
                            continue;
                        if (previousField == nextField)
                            continue;
                        var coordinatesToBeAdded = new List<int> { nextField.X, nextField.Y };
                        RulesExecutor.AddCoordinatesToList(
                            fieldsWherePieceCanMove,
                            coordinatesToBeAdded
                        );
                    }
                }
                break;
            case FigureType.Rook:
                var rookMovePatterns = new RookMovePattern().Patterns.ToList();
                // rook has 4 moving opportunities
                foreach (var pattern in rookMovePatterns)
                {
                    var canStillContinueRun = true;
                    var nextField = FieldHandler.CopyField(fieldOfPiece);

                    var currentPattern = pattern.ToList();

                    // go one default step before starting the repetitive step execution
                    nextField = StepExecutor.GoStepStraight(
                        currentPattern[0],
                        game,
                        nextField,
                        fieldOfPiece.Content.Color == Color.White ? Color.Black : Color.White,
                        true
                    );
                    // also add the additional field to the output list
                    if (RulesExecutor.AreCoordinatesOnBoard([nextField.X, nextField.Y]))
                    {
                        RulesExecutor.AddCoordinatesToList(
                            fieldsWherePieceCanMove,
                            [nextField.X, nextField.Y]
                        );
                    }

                    // repeat to go as long as possible along one pattern
                    // just for straight move pattern
                    while (canStillContinueRun)
                    {
                        var previousField = nextField;

                        nextField = currentPattern.Aggregate(
                            nextField,
                            (current, move) =>
                                StepExecutor.GoStepStraight(
                                    move,
                                    game,
                                    current,
                                    fieldOfPiece.Content.Color == Color.White
                                        ? Color.Black
                                        : Color.White
                                )
                        );

                        if (previousField.X == nextField.X && previousField.Y == nextField.Y)
                        {
                            canStillContinueRun = false;
                        }
                        else
                        {
                            // add the field to the list
                            if (RulesExecutor.AreCoordinatesOnBoard([nextField.X, nextField.Y]))
                            {
                                var coordinatesToBeAdded = new List<int>
                                {
                                    nextField.X,
                                    nextField.Y,
                                };
                                RulesExecutor.AddCoordinatesToList(
                                    fieldsWherePieceCanMove,
                                    coordinatesToBeAdded
                                );
                            }
                        }
                    }
                }
                break;
            case FigureType.Queen:
                var queenMovePatterns = new QueenMovePatterns().Patterns.ToList();
                foreach (var pattern in queenMovePatterns)
                {
                    var canStillContinueRun = true;
                    var nextField = FieldHandler.CopyField(fieldOfPiece);

                    var currentPattern = pattern.ToList();

                    // go one default step before starting the repetitive step execution
                    nextField = StepExecutor.GoStepStraight(
                        currentPattern[0],
                        game,
                        nextField,
                        fieldOfPiece.Content.Color == Color.White ? Color.Black : Color.White,
                        true
                    );
                    // also add the additional field to the output list
                    if (RulesExecutor.AreCoordinatesOnBoard([nextField.X, nextField.Y]))
                    {
                        var coordinatesToBeAdded = new List<int> { nextField.X, nextField.Y };
                        RulesExecutor.AddCoordinatesToList(
                            fieldsWherePieceCanMove,
                            coordinatesToBeAdded
                        );
                    }

                    // repeat to go as long as possible along one pattern
                    // just for straight move pattern
                    while (canStillContinueRun)
                    {
                        var previousField = nextField;

                        nextField = currentPattern.Aggregate(
                            nextField,
                            (current, move) =>
                                StepExecutor.GoStepStraight(
                                    move,
                                    game,
                                    current,
                                    fieldOfPiece.Content.Color == Color.White
                                        ? Color.Black
                                        : Color.White
                                )
                        );

                        if (nextField.X == previousField.X && nextField.Y == previousField.Y)
                        {
                            canStillContinueRun = false;
                        }
                        else
                        {
                            // add the field to the list
                            var coordinatesToBeAdded = new List<int> { nextField.X, nextField.Y };
                            if (!RulesExecutor.AreCoordinatesOnBoard(coordinatesToBeAdded))
                                continue;
                            RulesExecutor.AddCoordinatesToList(
                                fieldsWherePieceCanMove,
                                coordinatesToBeAdded
                            );
                        }
                    }
                }
                break;
            case FigureType.King:
                foreach (var pattern in new KingMovePatterns().Patterns)
                {
                    // initialize the variable representing one of the 8 moves
                    var nextField = pattern.Aggregate(
                        fieldOfPiece,
                        (current, move) =>
                            RulesExecutor.KingJustTriesToGoToField(move, current, game)
                    );

                    // main check if the field where the figure has moved has changed
                    if (fieldOfPiece.X == nextField.X && fieldOfPiece.Y == nextField.Y)
                        continue;
                    var coordinatesToBeAdded = new List<int> { nextField.X, nextField.Y };
                    RulesExecutor.AddCoordinatesToList(
                        fieldsWherePieceCanMove,
                        coordinatesToBeAdded
                    );
                }
                break;
        }

        return fieldsWherePieceCanMove;
    }
}
