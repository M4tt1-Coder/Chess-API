namespace Chess_API.utils.UtilTypes;

/// <summary>
/// Contains a list of covered fields by the pieces of one player.
///
/// Needed to determine from which piece a king is in check.
/// Also contains the coordinates of the piece that covers the fields.
/// </summary>
/// <param name="CoveredFields"> All fields that are being attacked by one piece</param>
/// <param name="PieceCoordinates">Position of the piece on the board</param>
public record CoveredFieldOfPieceObjects(List<List<int>> CoveredFields, List<int> PieceCoordinates);
    
/// <summary>
/// Represents a list of attacked fields by the pieces of both players.
/// </summary>
/// <param name="CoveredFieldOfPieceObjects">List of all attacked fields associated with a piece</param>
public record AttackedFieldsList(List<CoveredFieldOfPieceObjects> CoveredFieldOfPieceObjects);


