using Chess_API.Models;

namespace Chess_API.utils.Handlers;

/// <summary>
/// Regulates every action that is related to the move history.
///
/// Provides all functions to add, remove and get moves from the history.
/// </summary>
public static class MoveHistoryHandler
{
 /// <summary>
 /// Checks if a move is valid and adds it to the move history.
 /// </summary>
 /// <param name="game">Current game instance</param>
 /// <param name="move">A move to be added</param>
 /// <exception cref="Exception">The move's number doesn't fit the length of the history</exception>
 public static void AddMove(GameModel game, MoveModel move)
 {
  if (game.MoveHistory.Count >= move.MoveNum || Math.Abs(game.MoveHistory.Count - move.MoveNum) >= 2)
  {
   throw new Exception("Move History has been changed outside of the MoveHistoryManager! Please start the game again!");
  }
  game.MoveHistory.Add(move);
 }

 /// <summary>
 /// Simply removes a Move from the games history.
 /// </summary>
 /// <param name="game">Current game instance</param>
 /// <param name="move">Move to be removed</param>
 public static void _RemoveMove(GameModel game, MoveModel move)
 {
  game.MoveHistory.Remove(move);
 }
 
 /// <summary>
 /// Gets a move from the history by its number.
 /// </summary>
 /// <param name="game">Current game instance</param>
 /// <param name="moveNum">Number of the move</param>
 /// <returns>A specific move by its number</returns>
 public static MoveModel GetMove(GameModel game, int moveNum)
 {
  return game.MoveHistory.First(m => m.MoveNum == moveNum);
 }

 /// <summary>
 /// Checks if a piece already has moved from its starting position.
 /// </summary>
 /// <param name="moveHistory">Move history of the game</param>
 /// <param name="pawnId">Id of the piece</param>
 /// <returns></returns>
 public static bool HasPieceAlreadyMoved(IList<MoveModel> moveHistory, int pawnId)
 {
  var output = false;
  var pieceMove = moveHistory.FirstOrDefault(move => move.FigureId == pawnId);
  if (pieceMove is not null)
  {
   output = true;
  }
  return output;
 }
}