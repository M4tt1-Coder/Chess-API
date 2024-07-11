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
            Moves.Up,
            Moves.Up
        },
        // 3 
        new Collection<Moves>()
        {
            Moves.Up,
            Moves.Left
        },
        // 4
        new Collection<Moves>()
        {
            Moves.Up,
            Moves.Right
        }
    };
}