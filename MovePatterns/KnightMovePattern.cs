using System.Collections.ObjectModel;
using Chess_API.Enums;
using Chess_API.Interfaces;

namespace Chess_API.MovePatterns;

// TODO - Finish knight patterns

/// <summary>
/// The knight has eight move patterns.
///
/// ! All move patterns are special, they aren't linear. ! 
/// </summary>
public class KnightMovePattern : IMovePattern
{
    public IEnumerable<IEnumerable<Moves>> Patterns => new Collection<Collection<Moves>>()
    {
        new Collection<Moves>()
        {
            Moves.Down,
            Moves.Right,
        }
    };
}