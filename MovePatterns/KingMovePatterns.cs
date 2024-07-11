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