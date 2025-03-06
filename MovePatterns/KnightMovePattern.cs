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

    public IEnumerable<IEnumerable<Moves>> Patterns => new Collection<Collection<Moves>>()
    {
        // 1
        new()
        {
            Moves.Up,
            Moves.DiagonalUpLeft
        },
        // 2
        new()
        {
            Moves.Up,
            Moves.DiagonalUpRight
        },
        // 3
        new()
        {
            Moves.Right,
            Moves.DiagonalUpRight
        },
        // 4
        new()
        {
            Moves.Right,
            Moves.DiagonalDownRight
        },
        // 5
        new()
        {
            Moves.Down,
            Moves.DiagonalDownRight
        },
        // 6
        new()
        {
            Moves.Down,
            Moves.DiagonalDownLeft
        },
        // 7
        new()
        {
            Moves.Left,
            Moves.DiagonalDownLeft
        },
        // 8
        new()
        {
            Moves.Left,
            Moves.DiagonalUpLeft
        }
    };
}