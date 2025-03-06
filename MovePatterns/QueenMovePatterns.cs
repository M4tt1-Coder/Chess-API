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

    public IEnumerable<IEnumerable<Moves>> Patterns => new Collection<Collection<Moves>>()
    {
        // 1
        new()
        {
            Moves.Up
        },
        // 2
        new()
        {
            Moves.Right
        },
        // 3 
        new()
        {
            Moves.Down
        },
        // 4
        new()
        {
            Moves.Left
        },
        // 5
        new()
        {
            Moves.DiagonalUpRight
        },
        // 6 
        new()
        {
            Moves.DiagonalDownRight
        },
        // 7 
        new()
        {
            Moves.DiagonalDownLeft
        },
        // 8
        new()
        {
            Moves.DiagonalUpLeft
        }
    };
}