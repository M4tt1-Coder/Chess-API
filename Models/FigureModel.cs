using Chess_API.Enums;

namespace Chess_API.Models;

/// <summary>
/// Figure class represents a piece on the board.
/// It is stored in a field.
///
/// It can 'move' to another field if the conditions for the movement are valid.
///
/// It holds a path to a png-image of the figure.
/// </summary>
public class FigureModel
{
    /// <summary>
    /// Defines type of a piece through an enum.
    /// </summary>
    public FigureType Type { get; set; }

    /// <summary>
    /// That is an external path to a local stored, static png-picture.
    /// </summary>
    public string PictureSource { get; set; }

    /// <summary>
    /// Shows if the user has clicked on a field with a piece of his color.
    /// Should enable a appearance of possible movements on the board.
    /// </summary>
    public bool Selected { get; set; }

    /// <summary>
    /// Property to declare the color of the figure.
    /// </summary>
    public Colors Color { get; set; }

    public FigureModel(FigureType type, bool selected, string pictureSource, Colors color)
    {
        Type = type;
        Selected = selected;
        PictureSource = pictureSource;
        Color = color;
    }

    public FigureModel()
    {
    }
}