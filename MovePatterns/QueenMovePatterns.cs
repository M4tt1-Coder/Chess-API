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
/// foreach (var pattern in QueenMovePatterns.Patterns)
/// {
///     // ... your checks
/// }
/// </code>
/// </summary>
public class QueenMovePatterns : IMovePattern
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
        },
        // 5
        new Collection<Moves>()
        {
            Moves.Up,
            Moves.Right
        },
        // 6 
        new Collection<Moves>()
        {
            Moves.Down,
            Moves.Right
        },
        // 7 
        new Collection<Moves>()
        {
            Moves.Down,
            Moves.Left
        },
        // 8
        new Collection<Moves>()
        {
            Moves.Up,
            Moves.Left
        }
    };
}