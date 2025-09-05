using System.Collections.ObjectModel;
using Chess_API.Enums;
using Chess_API.Interfaces;

namespace Chess_API.MovePatterns;

/// <summary>
/// The queen has the biggest move-variety of all figures.
///
/// On all linear move sets should be checked if they can proceed.
///
/// Its the strongest piece of all.
///
/// It has similar move patterns as the king.
/// <code>
/// // ...
///
/// foreach (var pattern in new QueenMovePatterns().Patterns)
/// {
///     // ... your checks
/// }
/// </code>
/// </summary>
public class QueenMovePatterns : IMovePattern
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
        },
        // 5
        new()
        {
            Move.DiagonalUpRight
        },
        // 6 
        new()
        {
            Move.DiagonalDownRight
        },
        // 7 
        new()
        {
            Move.DiagonalDownLeft
        },
        // 8
        new()
        {
            Move.DiagonalUpLeft
        }
    };
}