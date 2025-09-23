using System.ComponentModel.DataAnnotations;
using Chess_API.Enums;

namespace Chess_API.Models;

/// <summary>
/// Biggest instance in the type tree.
///
/// Represents the whole game.
/// </summary>
public class GameModel
{
    /// <summary>
    /// Gives information about which color is on the top or on the bottom side
    /// of the board.
    /// </summary>
    public PlayingDirection Direction { get; set; }
    
    /// <summary>
    /// Represents the identifier of the current game instance in the database.
    /// </summary>
    [Key]
    public int GameId { get; set; }
    
    /// <summary>
    /// Stands for the player who is everytime a person not dependent on the game mode.
    /// </summary>
    public PlayerModel PlayerOne { get; set; } = null!;

    /// <summary>
    /// Can be the Ai instance with the default name "Paul",
    /// or another normal player.
    /// </summary>
    public PlayerModel PlayerTwo { get; set; } = null!;

    /// <summary>
    /// The actual game field the player interacts with.
    /// </summary>
    public List<FieldRowModel> Board { get; set; } = null!;

    /// <summary>
    /// List that contains all moves that have been made in the current game.
    /// </summary>
    public List<MoveModel> MoveHistory { get; set; } = null!;
    
    /// <summary>
    /// Counts how many rounds have been played in the specific playing mode.
    /// </summary>
    public int Round { get; set; }

    /// <summary>
    /// Its content is the chosen game mode of the player one.
    /// </summary>
    public PlayingMode Mode { get; set; }

    /// <summary>
    /// Sets if some player won, nobody won or the game haven't been decided already.
    /// </summary>
    public Winner Winner { get; set; }

    /// <summary>
    /// Is null when the players want to customize their time OR when they want no time limit.
    /// </summary>
    public PlayTimeMode PlayTimeMode { get; set; }

    /// <summary>
    /// Specifies which player's turn it currently is in the game.
    /// </summary>
    public PlayerTurn PlayerTurn { get; set; }
    
    public GameModel(PlayerModel playerOne, PlayerModel playerTwo, List<FieldRowModel> board, int round,
        PlayingMode mode, Winner winner, PlayingDirection playingDirection)
    {
        PlayerOne = playerOne;
        PlayerTwo = playerTwo;
        Board = board;
        Round = round;
        Mode = mode;
        Winner = winner;
        Direction = playingDirection;
        PlayerTurn = PlayerTurn.White;
        MoveHistory = new List<MoveModel>() {};
    }

    public GameModel()
    {
    }

    /// <summary>
    /// Creates a deep copy of the current state of the chessboard represented by a list of <see cref="FieldRowModel"/>.
    /// </summary>
    /// <returns>
    /// A new list of <see cref="FieldRowModel"/> objects that represents the exact state of the original board,
    /// ensuring any modifications to the copied board do not affect the original board.
    /// </returns>
    public List<FieldRowModel> CopyBoard()
    {
        var copiedBoard = new List<FieldRowModel>();

        foreach (var row in Board)
        {
            var copiedRow = new FieldRowModel
            {
                Row = new List<FieldModel>()
            };

            foreach (var field in row.Row)
            {
                copiedRow.Row.Add(new FieldModel(field.Color, field.Content, field.X, field.Y)
                {
                    MovableField = field.MovableField,
                    SelectedField = field.SelectedField
                });
            }

            copiedBoard.Add(copiedRow);
        }

        return copiedBoard;
    }
}