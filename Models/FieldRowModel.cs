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
    /// Defines what line it is starting from the white color side!
    /// </summary>
    public int Number { get; set; }

    /// <summary>
    /// Stores all fields in the row.
    ///
    /// It has until 8 members, located starting with the index 0 to 7.
    /// </summary>
    public List<FieldModel> Row { get; set; }

    public FieldRowModel(List<FieldModel> row, int number)
    {
        Row = row;
        Number = number;
    }
    
    
}