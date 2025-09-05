using System.Collections.ObjectModel;
using Chess_API.Enums;
using Chess_API.Interfaces;

namespace Chess_API.MovePatterns;

/// <summary>
/// The king has the biggest value of all pieces, it can't be thrown but it's move sets just can be repeat once in a players turn.
///
/// Similar to the queen but weakened in movement.
///
/// <code>
/// // same implementation like all other pieces.
/// </code>
/// </summary>
public class KingMovePatterns : IMovePattern
{
    public bool AreMovesInfinite => false;

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