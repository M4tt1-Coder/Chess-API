using Chess_API.Enums;

namespace Chess_API.Interfaces;

/// <summary>
/// The framework for every figure move pattern.
///
/// It should include all possible general moves of a piece.
/// </summary>
public interface IMovePattern
{
    /// <summary>
    /// Defines if the moves are infinite and repetitive.
    /// </summary>
    public bool AreMovesInfinite { get; }

    /// <summary>
    /// Public accessible list of all patterns.
    /// </summary>
    public IEnumerable<IEnumerable<Move>> Patterns { get; }
}
