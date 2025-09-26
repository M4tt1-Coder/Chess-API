using Chess_API.Enums;
using Chess_API.Models;
using Chess_API.utils.Executors;

namespace Chess_API.utils.Handlers;

/// <summary>
/// The FigureHandler class is a utility class designed to handle operations and logic
/// related to chessboard figures. It provides methods for managing and manipulating
/// figures on a chessboard, including rules, positions, and movement validations.
/// </summary>
public static class FigureHandler
{
    /// <summary>
    /// Creates a copy of the provided chess figure.
    /// </summary>
    /// <param name="figure">The chess figure to be copied, represented by a <see cref="FigureModel"/> object.</param>
    /// <returns>A new <see cref="FigureModel"/> instance that is a duplicate of the input figure.</returns>
    public static FigureModel CopyFigure(FigureModel figure)
    {
        return new FigureModel(figure.Type, figure.PictureSource, figure.Color, figure.FigureId);
    }

    /// <summary>
    /// In the case that one or more pieces of the opposite color can move, it returns true.
    /// </summary>
    /// <param name="game">The current Game instance</param>
    /// <param name="pieceColorToConsider">The actual color of the figure</param>
    /// <returns>TRUE, when at least one piece can move.</returns>
    public static bool OneOrMorePiecesCanMove(GameModel game, Color pieceColorToConsider)
    {
        // iterate over the fields
        foreach (var singleField in game.Board.SelectMany(row => row.Row))
        {
            if (singleField.Content is null || singleField.Content.Color == pieceColorToConsider)
                continue;

            if (
                StepExecutor.TestIfPieceCanMakeAMove(
                    game,
                    new List<int> { singleField.X, singleField.Y }
                )
            )
            {
                return true;
            }
        }
        return false;
    }
}
