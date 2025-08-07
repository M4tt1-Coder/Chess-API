namespace Chess_API.utils.UtilTypes;

/// <summary>
/// Result of the checking process if the king is in check.
/// </summary>
/// <param name="IsInCheck">True, if the king was really in a check.</param>
/// <param name="AttackingPieceCoordinates">Not null, when the king is in check. Coordinates of the piece that is checking the king</param>
public record KingInCheckResult(bool IsInCheck, List<int>? AttackingPieceCoordinates);