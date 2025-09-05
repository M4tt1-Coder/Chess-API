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
    public bool AreMovesInfinite => true;

    public IEnumerable<IEnumerable<Move>> Patterns => new Collection<Collection<Move>>()
    {
        // 1
        new()
        {
            Move.Up
        },
        // 2
        new()
        {
            Move.Right
        },
        // 3
        new()
        {
            Move.Down
        },
        // 4
        new()
        {
            Move.Left
        }
    };
}