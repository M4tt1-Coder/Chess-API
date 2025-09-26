using System.Collections.ObjectModel;
using Chess_API.Enums;
using Chess_API.Interfaces;

namespace Chess_API.MovePatterns;

/// <summary>
/// Pawns are special figures, they can throw other pieces under a specific condition with a special move.
///
/// Nevertheless it mostly moves up / down & can normally throw with one diagonal move.
///
/// They can change into other pieces so movement checks have to depend on the figure type conditionally!
///
/// <code>
/// // for type checking
/// switch (Figure.Type)
/// {
///     case Types.Pawn:
///         // some stuff
///         break;
///     ...
/// }
/// </code>
/// </summary>
public class PawnMovePatterns : IMovePattern
{
    public bool AreMovesInfinite => false;

    public IEnumerable<IEnumerable<Move>> Patterns =>
        new Collection<Collection<Move>>()
        {
            // 1
            new() { Move.Up },
            // 2
            new() { Move.Up, Move.Up },
            // 3
            new() { Move.DiagonalUpLeft },
            // 4
            new() { Move.DiagonalUpRight },
            // 5
            new() { Move.Down },
            // 6
            new() { Move.Down, Move.Down },
            // 7
            new() { Move.DiagonalDownLeft },
            // 8
            new() { Move.DiagonalDownRight },
        };
}
