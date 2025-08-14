using System.Collections.ObjectModel;
using Chess_API.Enums;
using Chess_API.Interfaces;

namespace Chess_API.MovePatterns;

/// <summary>
/// The knight has eight move patterns.
///
/// ! All move patterns are special, they aren't linear. ! 
/// </summary>
public class KnightMovePattern : IMovePattern
{
    public bool AreMovesInfinite => false;

    public IEnumerable<IEnumerable<Move>> Patterns => new Collection<Collection<Move>>()
    {
        // 1
        new()
        {
            Move.Up,
            Move.DiagonalUpLeft
        },
        // 2
        new()
        {
            Move.Up,
            Move.DiagonalUpRight
        },
        // 3
        new()
        {
            Move.Right,
            Move.DiagonalUpRight
        },
        // 4
        new()
        {
            Move.Right,
            Move.DiagonalDownRight
        },
        // 5
        new()
        {
            Move.Down,
            Move.DiagonalDownRight
        },
        // 6
        new()
        {
            Move.Down,
            Move.DiagonalDownLeft
        },
        // 7
        new()
        {
            Move.Left,
            Move.DiagonalDownLeft
        },
        // 8
        new()
        {
            Move.Left,
            Move.DiagonalUpLeft
        }
    };
}