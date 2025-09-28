using Chess_API.Models;
using Chess_API.utils.Executors;

namespace Chess_API.utils.Handlers;

/// <summary>
/// Provides functionalities to manipulate the game board.
/// </summary>
public static class BoardHandler
{
    /// <summary>
    /// Marks all fields a piece can move to, which can be seen on the board.
    /// </summary>
    /// <param name="game">The current Game instance.</param>
    /// <returns>The updated game with the marked fields.</returns>
    public static GameModel MarkAllFieldsWherePieceCanMoveTo(GameModel game)
    {
        var selectedFieldWithPiece = FieldHandler.IsAFieldSelected(game);
        if (!selectedFieldWithPiece.IsThereSelectedField) return game;
        
        // check if the coordinates where provided
        if (selectedFieldWithPiece.X is null || selectedFieldWithPiece.Y is null) return game;
        
        // get all field where a piece can move to
        var fieldsPieceCanMoveTo = StepExecutor.FieldsWherePieceCanMoveTo(game,
            [(int)selectedFieldWithPiece.X, (int)selectedFieldWithPiece.Y]);

        // mark all fields
        foreach (var row in game.Board)
        {
            foreach (var field in row.Row)
            {
                foreach (var foundField in fieldsPieceCanMoveTo) {
                    if (foundField[0] == field.X && foundField[1] == field.Y)
                    {
                        field.MovableField = true;
                    }
                }
            }
        }
        
        // return the game instance
        return game;
    }

    /// <summary>
    /// Unselects all fields that where marked in the last round.
    /// </summary>
    /// <param name="game">The current Game instance</param>
    /// <returns>Updated Game object</returns>
    public static GameModel UnmarkAllFieldsWherePieceCouldMoveTo(GameModel game)
    {
        foreach (var row in game.Board)
        {
            foreach (var field in row.Row)
            {
                if (field.MovableField)
                {
                    field.MovableField = false;
                }
            }
        }

        return game;
    }
}