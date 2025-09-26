using System.ComponentModel.DataAnnotations;

namespace Chess_API.Models;

/// <summary>
/// There are 8 rows in a chess board, so eight rows in the game model.
///
/// The system builds on a 2D-coordinate-system.
///
/// It represents one of these rows.
/// </summary>
public class FieldRowModel
{
    /// <summary>
    /// The identifier for a row.
    ///
    /// Represents the number in the playing field in which the row sits
    ///
    /// Can't be null.
    /// </summary>
    [Key]
    public int RowId { get; set; }

    /// <summary>
    /// Declares which number the row is from the top to the bottom in the playing field.
    /// </summary>
    [Required]
    public int RowNumber { get; set; }

    /// <summary>
    /// Stores all fields in the row.
    ///
    /// It has 8 members, located starting with the index 0 to 7.
    /// </summary>
    [Required]
    public IList<FieldModel> Row { get; set; } = null!;

    public FieldRowModel(List<FieldModel> row, int number)
    {
        RowNumber = number;
        Row = row;
    }

    public FieldRowModel() { }
}
