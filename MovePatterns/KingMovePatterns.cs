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