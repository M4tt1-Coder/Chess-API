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
    public bool AreMovesInfinite => true;

    public IEnumerable<IEnumerable<Move>> Patterns =>
        new Collection<Collection<Move>>()
        {
            // 1
            new() { Move.DiagonalUpLeft },
            // 2
            new() { Move.DiagonalUpRight },
            // 3
            new() { Move.DiagonalDownRight },
            // 4
            new() { Move.DiagonalDownLeft },
        };
}
