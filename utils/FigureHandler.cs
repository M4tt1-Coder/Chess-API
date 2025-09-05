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
    
    // public static List<List<int>> AllFieldsAttackedByFigure(GameModel game, List<int> figureCoordinates)
    // {
    //     var output = new List<List<int>>();
    //     var figureField = FieldHandler.GetSpecificFieldByCoordinates(game, figureCoordinates);
    //     
    //     if (figureField.Content is null)
    //     {
    //         Console.WriteLine("FigureHandler.AllFieldsAttackedByFigure: No figure on the given coordinates!");
    //         return output;
    //     }
    //     
    //     
    //     
    // }
}