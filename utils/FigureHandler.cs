using Chess_API.Models;

namespace Chess_API.utils;

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
        return new FigureModel(figure.Type, figure.Selected, figure.PictureSource, figure.Color, figure.FigureId);
    }
}