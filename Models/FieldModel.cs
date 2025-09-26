using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Chess_API.Enums;

namespace Chess_API.Models;

/// <summary>
/// There are 64 fields on the chess broad.
///
/// They are stored in a data structure with eight rows with eight fields each.
///
/// It can hold a figure as content or be empty.
///
/// When a player wants to move a piece, he can select a possible field.
/// -> The field will be highlighted to make it easier to show a potential move.
///
/// Every field has a specific color to maintain.
/// </summary>
public class FieldModel
{
    /// <summary>
    /// The identifier for every field.
    ///
    /// Can't be null.
    /// </summary>
    [Key]
    public int FieldId { get; init; }

    /// <summary>
    /// As in the figure class, these stands for color of the field.
    /// </summary>
    [Required]
    public Color Color { get; init; }

    /// <summary>
    /// The property can have two states:
    /// 1.) Hold nothing
    /// 2.) Include a piece of every type.
    /// </summary>
    public FigureModel? Content { get; set; }

    /// <summary>
    /// The X ordinate of the field.
    /// </summary>
    [Required]
    public int X { get; set; }

    /// <summary>
    /// The Y ordinate of the field.
    /// </summary>
    [Required]
    public int Y { get; set; }

    /// <summary>
    /// Is TRUE when the user can move a piece to this field.
    /// </summary>
    public bool MovableField { get; set; }

    /// <summary>
    /// Is TRUE when the user selected this field.
    /// </summary>
    public bool SelectedField { get; set; }

    public FieldModel(Color color, FigureModel? content, int x, int y)
    {
        Color = color;
        Content = content;
        MovableField = false;
        SelectedField = false;
        X = x;
        Y = y;
    }

    public FieldModel() { }
}
