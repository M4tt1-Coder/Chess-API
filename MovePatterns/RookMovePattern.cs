using System.Collections.ObjectModel;
using Chess_API.Enums;
using Chess_API.Interfaces;

namespace Chess_API.MovePatterns;

/// <summary>
/// The rook has simple straight forward moves.
///
/// It goes just in one direction.
///
/// Steps again have to be repeated until the rook couldn't move anymore.
///
/// <example>
/// Loop through all possible patterns.
/// </example>
/// <code>
/// use path…MovePatterns;
///
/// foreach (var p in RookMovePattern.Patterns)
/// {
///     // do something
/// }
/// </code>
/// </summary>
public class RookMovePattern : IMovePattern
{
    public IEnumerable<IEnumerable<Moves>> Patterns => new Collection<Collection<Moves>>()
    {
        // 1
        new Collection<Moves>()
        {
            Moves.Up
        },
        // 2
        new Collection<Moves>()
        {
            Moves.Right
        },
        // 3
        new Collection<Moves>()
        {
            Moves.Down
        },
        // 4
        new Collection<Moves>()
        {
            Moves.Left
        }
    };
}