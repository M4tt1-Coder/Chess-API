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
    /// First field in the row.
    /// </summary>
    public FieldModel One { get; set; }

    /// <summary>
    /// Second field in the row.
    /// </summary>
    public FieldModel Two { get; set; }

    /// <summary>
    /// Third field in the row.
    /// </summary>
    public FieldModel Three { get; set; }

    /// <summary>
    /// Fourth field in the row.
    /// </summary>
    public FieldModel Four { get; set; }

    /// <summary>
    ///  Fifth field in the row.
    /// </summary>
    public FieldModel Five { get; set; }

    /// <summary>
    /// Sixth field in the row.
    /// </summary>
    public FieldModel Six { get; set; }

    /// <summary>
    /// Seventh field in the row.
    /// </summary>
    public FieldModel Seven { get; set; }

    /// <summary>
    /// 8. field in the row.
    /// </summary>
    public FieldModel Eight { get; set; }

    public FieldRowModel(FieldModel one, FieldModel two, FieldModel three, FieldModel four, FieldModel five,
        FieldModel six, FieldModel seven, FieldModel eight)
    {
        One = one;
        Two = two;
        Three = three;
        Four = four;
        Five = five;
        Six = six;
        Seven = seven;
        Eight = eight;
    }

    public FieldRowModel()
    {
    }
}