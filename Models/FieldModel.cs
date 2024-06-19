using System.ComponentModel.DataAnnotations;
using Chess_API.Enums;

namespace Chess_API.Models;

// TODO - Add props that hold the position of a field specifically

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
    public Colors Color { get; init; }

    /// <summary>
    /// The property can have two states:
    /// 1.) hold nothing
    /// 2.) include a piece of every type.
    /// </summary> 
    public FigureModel? Content { get; set; }

    /// <summary>
    /// In a piece movement, this prop defines if the player can move the piece to the specific field.
    /// </summary>
    public bool MoveSelected { get; set; }
    
    public FieldModel(Colors color, FigureModel? content)
    {
        Color = color;
        Content = content;
        MoveSelected = false;
    }

    public FieldModel()
    {
    }
}