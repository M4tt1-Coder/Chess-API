using System.Collections.ObjectModel;
using Chess_API.Enums;
using Chess_API.Interfaces;

namespace Chess_API.MovePatterns;

/// <summary>
/// The bishop moves differently from the knight.
///
/// It is linear. Furthermore, its move pattern should be repeated until it is not
/// applicable anymore.
///
/// There are four possibilities.
/// </summary>
public class BishopMovePattern : IMovePattern
{
    public IEnumerable<IEnumerable<Moves>> Patterns => new Collection<Collection<Moves>>()
    {
        // 1
        new Collection<Moves>()
        {
            Moves.Up,
            Moves.Left
        },
        // 2
        new Collection<Moves>()
        {
            Moves.Up,
            Moves.Right
        },
        // 3
        new Collection<Moves>()
        {
            Moves.Down,
            Moves.Right,
        },
        // 4
        new Collection<Moves>()
        {
            Moves.Down,
            Moves.Left
        }
    };
}