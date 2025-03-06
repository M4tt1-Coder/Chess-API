using System.Collections.ObjectModel;
using Chess_API.Enums;
using Chess_API.Interfaces;
using Chess_API.Models;
using Chess_API.MovePatterns;

namespace Chess_API.utils;

/// <summary>
/// Global rule endpoint.
///
/// Covers all necessary rule sets for every case and figure.
///
/// Provides a public function to start checking that all rules have been followed.
/// 
/// Checks for all movements cases of all pieces.
///
/// Gives a boolean whether the piece can move to a new field or not.
///
/// Potential checks, throwing other pieces, ....
/// </summary>
public class RulesExecutor
{
    /// <summary>
    /// MoveExaminer logger singleton service.
    /// </summary>
    private readonly ILogger<RulesExecutor> _logger;

    public RulesExecutor(ILogger<RulesExecutor> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Global rule endpoint.
    /// 
    /// Validates a move of a figure.
    ///
    /// Calls more underlying method structures.
    /// </summary>
    /// <param name="game">Current game</param>
    /// <param name="curField">The selected field of player.</param>
    /// <param name="newField">The new field where the user wants to put the figure.</param>
    /// <returns>A boolean to validate that all rules apply to the move.</returns>
    public void ValidateMove(GameModel game, FieldModel curField, FieldModel newField)
    {
        // TODO - Check if a player has won with a checkmate 
        if (curField.Content is null)
        {
            _logger.LogError("The field where the figure is standing is empty!");
            return;
        }
        
        // type of corresponding figure
        // first check is about the piece type
        var type = curField.Content.Type;
        
        // depending on the type check for movements

        switch (type)
        {
            // TODO - Add the pattern execution for all pieces
            case FigureType.Pawn:
                break;
            case FigureType.Bishop:
                break;
            case FigureType.Knight:
                break;
            case FigureType.Rook:
                break;
            case FigureType.Queen:
                break;
            case FigureType.King:
                break;
            default:
                _logger.LogError("A wrong enum value has been entered, while checking for the figure type!");
                break;
        }
    }

    // TODO - 1.) Finish check checker
    /// <summary>
    /// 
    /// </summary>
    /// <param name="game"></param>
    /// <param name="kingColor"></param>
    /// <param name="figureNow"></param>
    /// <param name="figureAfter"></param>
    /// <returns></returns>
    private bool CheckChecker(GameModel game, Colors kingColor, IList<int>? figureNow, IList<int>? figureAfter)
    {
        var output = true;
        IList<int> kingCoordinates = new List<int>();

        // determine kings location
        foreach (var row in game.Field)
        {
            foreach (var field in row.Row)
            {
                if (field.Content is null) continue;
                if (field.Content.Type == FigureType.King && field.Content.Color == kingColor)
                {
                    kingCoordinates = field.Coordinates;
                }
            }
        }
        // if its just a general check or when a piece is potentially moving
        
        // when the player has made move which is valid (didn't cause a check on its own king)
        // but gave a check to the opponents king
        if (figureNow is null || figureAfter is null)
        {
            // when in one move pattern of some opponent piece the same coordinates as the kings appear there is a check
            output = IsKingInCheck(game, kingCoordinates, kingColor);
        }
        else
        {
            // go through all opposite figures and check for possible checks
            // when a figure can move to the king's field -> check        
            var figure = FieldHandler.GetSpecificFieldByCoordinates(game, figureNow);
            // copy instance of the game to check if the move is valid
            var gameCopy = GameHandler.CopyGame(game);
            // move the figure to the new field
            // TODO - Check if the figure can move to the new field & if the king is in check
        }
        
        return output;
    }

    /// <summary>
    /// Gets all fields where the king is attacked by the opposite color.
    ///
    /// When the king is in check, he can't move to the field where he is attacked.
    ///
    /// If the coordinates of the king's field are in the list of attacked fields, the king is in check.
    /// </summary>
    /// <param name="game">The current game instance</param>
    /// <param name="kingCoordinates">Coordinates of the king's field</param>
    /// <param name="kingColor">The color of the king which is checked on being in check</param>
    /// <returns></returns>
    private bool IsKingInCheck(GameModel game, IList<int> kingCoordinates, Colors kingColor)
    {
        var output = false;
        
        // go through board -> check if opposite color -> execute patterns
        // get all fields which are attacked by the opponents pieces
        var attackedFields = FieldsWhereKingIsAttacked(game, kingColor);
        
        // compare king's coordinates with all coordinates where he is in check
        foreach (var checkCoordinates in attackedFields)
        {
            // when the field, where the king is situated, is being attacked
            if (Equals(checkCoordinates, kingCoordinates))
            {
                output = true;
            }
        }

        return output;
    }

    /// <summary>
    /// Figures of the opposite color doesn't matter.
    ///
    /// Iterates through all fields on the board -> when a piece of the opposite color is on the current field
    /// -> all field that are attacked by it will be added to the coordinates list
    ///
    /// Fails when the 'field' attribute is null.
    ///
    /// Helps determining, where a king of one color is in a check or where he can move.
    /// </summary>
    /// <param name="game">Current game instance</param>
    /// <param name="kingColor">Either white /  black : Color of the king</param>
    /// <returns>A list of coordinates (list of integers) of all field where the king is attacked</returns>
    private IEnumerable<IList<int>> FieldsWhereKingIsAttacked(GameModel game, Colors kingColor)
    {
        var output = new Collection<IList<int>>();
        
        // similar to move patterns but not moving but attacking
        // go along all possible move patterns
        // store fields which are attacked by figures of 

        foreach (var row in game.Field)
        {
            foreach (var field in row.Row)
            {
                // Simply continue the iteration when: there is now figure || the figure is of the same color as the king
                if (field.Content is null || field.Content.Color == kingColor) continue;
                switch (field.Content.Type)
                {
                    case FigureType.Pawn:
                        // for pawns I need to know in which direction they are turned to 
                        switch(game.Direction)
                        {
                            case PlayingDirection.WhiteTop:
                                if (kingColor == Colors.White)
                                {
                                    // check for boundaries 
                                    // assign new possible fields that could be attacked by black piece
                                    var firstField = new []{ field.Coordinates[0] - 1, field.Coordinates[1] - 1 };
                                    var secondField = new[] { field.Coordinates[0] + 1, field.Coordinates[1] - 1 };

                                    if (AreCoordinatesOnBoard(firstField))
                                    {
                                        AddCoordinatesToList(output, firstField);
                                    }

                                    if (AreCoordinatesOnBoard(secondField))
                                    {
                                        AddCoordinatesToList(output, secondField);
                                    }
                                }
                                else
                                {
                                    var firstField = new[] { field.Coordinates[0] - 1, field.Coordinates[1] + 1 };
                                    var secondField = new[] { field.Coordinates[0] + 1, field.Coordinates[1] + 1 };

                                    if (AreCoordinatesOnBoard(firstField))
                                    {
                                        AddCoordinatesToList(output, firstField);
                                    }

                                    if (AreCoordinatesOnBoard(secondField))
                                    {
                                        AddCoordinatesToList(output, secondField);
                                    }
                                }
                                break;
                            case PlayingDirection.WhiteBottom:
                                if (kingColor == Colors.White)
                                {
                                    var firstField = new[] { field.Coordinates[0] - 1, field.Coordinates[1] + 1 };
                                    var secondField = new[] { field.Coordinates[0] + 1, field.Coordinates[1] + 1 };

                                    if (AreCoordinatesOnBoard(firstField))
                                    {
                                        AddCoordinatesToList(output, firstField);
                                    }

                                    if (AreCoordinatesOnBoard(secondField))
                                    {
                                        AddCoordinatesToList(output, secondField);
                                    }
                                }
                                else
                                {
                                    var firstField = new[] { field.Coordinates[0] - 1, field.Coordinates[1] - 1 };
                                    var secondField = new[] { field.Coordinates[0] + 1, field.Coordinates[1] - 1 };

                                    if (AreCoordinatesOnBoard(firstField))
                                    {
                                        AddCoordinatesToList(output, firstField);
                                    }

                                    if (AreCoordinatesOnBoard(secondField))
                                    {
                                        AddCoordinatesToList(output, secondField);
                                    }
                                }
                                break;
                            default:
                                _logger.LogError("Couldn't resolve a good enum value!");
                                break;
                        }
                        break;
                    case FigureType.Bishop:
                        // run along the move patterns
                        foreach (var pattern in new BishopMovePattern().Patterns)
                        {
                            var canStillContinue = true;
                            var nextField = field;

                            // repeat to go as long as possible along one pattern
                            // just for straight move pattern
                            while (canStillContinue)
                            {
                                var previousField = nextField;

                                // add the field to the list
                                if (AreCoordinatesOnBoard(previousField.Coordinates))
                                {
                                    AddCoordinatesToList(output, previousField.Coordinates);
                                }
                                
                                foreach (var move in (List<Moves>)pattern)
                                {
                                    nextField = StepExecutor.GoStepStraight(move, game, field);    
                                }

                                if (Equals(previousField.Coordinates, nextField.Coordinates))
                                {
                                    canStillContinue = false;
                                }
                            }
                        }
                        break;
                    case FigureType.Knight:
                        // do the same logic for the knight as for the bishop
                        // all eight field can be added
                        foreach (var pattern in new KnightMovePattern().Patterns)
                        {   
                            // assign field where knight stands before iterating the patterns
                            var nextField = field;
                            var moveCount = 0;
                            // when it reaches 2 -> knight could land on that spot
                            foreach (var move in pattern)
                            {
                                var previousField = nextField;
                                // first go the step
                                nextField  = StepExecutor.GoStepKnight(move, game, previousField, field.Content.Color);
                                // increase counter by 1
                                moveCount++;
                                // when 2 steps have been taken -> check if previous field is equal to field after
                                // the moving operation -> continue
                                if (moveCount != 2) continue;
                                if (previousField == nextField)
                                {
                                    goto SkipKnightPattern;
                                }

                                AddCoordinatesToList(output, nextField.Coordinates);
                            }

                            SkipKnightPattern: ;
                        }
                        break;
                    case FigureType.Rook:
                        // rook has 4 moving opportunities
                        foreach (var pattern in new RookMovePattern().Patterns)
                        {
                            var canStillContinueRun = true;
                            var nextField = field;

                            // repeat to go as long as possible along one pattern
                            // just for straight move pattern
                            while (canStillContinueRun)
                            {
                                var previousField = nextField;

                                // add the field to the list
                                if (AreCoordinatesOnBoard(previousField.Coordinates))
                                {
                                    AddCoordinatesToList(output, previousField.Coordinates);
                                }

                                foreach (var move in (List<Moves>)pattern)
                                {
                                    nextField = StepExecutor.GoStepStraight(move, game, field);
                                }

                                if (Equals(previousField.Coordinates, nextField.Coordinates))
                                {
                                    canStillContinueRun = false;
                                }
                            }
                        }
                        break;
                    case FigureType.Queen:
                        foreach (var pattern in new QueenMovePatterns().Patterns)
                        {
                            var canStillContinueRun = true;
                            var nextField = field;

                            // repeat to go as long as possible along one pattern
                            // just for straight move pattern
                            while (canStillContinueRun)
                            {
                                var previousField = nextField;

                                // add the field to the list
                                if (AreCoordinatesOnBoard(previousField.Coordinates))
                                {
                                    AddCoordinatesToList(output, previousField.Coordinates);
                                }

                                foreach (var move in (List<Moves>)pattern)
                                {
                                    nextField = StepExecutor.GoStepStraight(move, game, field);
                                }

                                if (Equals(previousField.Coordinates, nextField.Coordinates))
                                {
                                    canStillContinueRun = false;
                                }
                            }
                        }
                        break;
                    case FigureType.King:
                        foreach (var pattern in new KingMovePatterns().Patterns)
                        {
                            // just do the one step & look if is possible for the king to move
                            // need a separate function which checks if the king can go to a specific field
                            // without including check-danger checks
                            // king has just one move in every pattern
                            
                            // initialize the variable representing one of the 8 moves 
                            var nextField = pattern.Aggregate(field, (current, move) => KingJustTriesToGoToField(move, current, game));
                                
                            // main check if the field where the figure has moved has changed
                            if (!Equals(nextField.Coordinates, field.Coordinates)) AddCoordinatesToList(output, nextField.Coordinates);
                        }
                        break;
                    default:
                        _logger.LogError("Enum value again out of scope!");
                        break;
                }
            }
        }

        return output;
    }
    
    // ----- Helper functions -----

    /// <summary>
    /// In total there are 2 step execution methods for the king.
    ///
    /// I am excluding check-checks here.
    ///
    /// Pays attention that all rules are fulfilled before the king is allowed to move
    /// to the new field on the board. 
    /// </summary>
    /// <param name="kingMove">One of the 8 move types</param>
    /// <param name="currentField">Starting field where we are going from</param>
    /// <param name="game">Current game object</param>
    /// <returns>A field the king can move to</returns>
    private FieldModel KingJustTriesToGoToField(Moves kingMove, FieldModel currentField, GameModel game)
    {
        // vars instantiation
        var output = new FieldModel();
        List<int> newCoordinates;
        // cover all moves
        switch (kingMove)
        {
            case Moves.Up:
                // get the new coordinates
                newCoordinates = new List<int>() { currentField.Coordinates[0], currentField.Coordinates[1] - 1 };
                // validate new coordinates
                if (newCoordinates[1] < 0) return currentField;
                // get new field with new coordinates from game board
                // assign to output var
                output = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                break;
            case Moves.Down:
                newCoordinates = new List<int>() { currentField.Coordinates[0], currentField.Coordinates[1] + 1};
                if (newCoordinates[1] > 7) return currentField;
                output = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                break;
            case Moves.Left:
                newCoordinates = new List<int>() { currentField.Coordinates[0] - 1, currentField.Coordinates[1]};
                if (newCoordinates[0] < 0) return currentField;
                output = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                break;
            case Moves.Right:
                newCoordinates = new List<int>() { currentField.Coordinates[0], currentField.Coordinates[1] + 1 };
                if (newCoordinates[0] > 7) return currentField;
                output = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                break;
            case Moves.DiagonalUpLeft:
                newCoordinates = new List<int>() { currentField.Coordinates[0] - 1, currentField.Coordinates[1] - 1 };
                if (newCoordinates[1] < 0 && newCoordinates[0] < 0) return currentField;
                output = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                break;
            case Moves.DiagonalUpRight:
                newCoordinates = new List<int>() { currentField.Coordinates[0] + 1, currentField.Coordinates[1] - 1 };
                if (newCoordinates[1] < 0 && newCoordinates[0] > 7) return currentField;
                output = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                break;
            case Moves.DiagonalDownLeft:
                newCoordinates = new List<int>() { currentField.Coordinates[0] - 1, currentField.Coordinates[1] + 1 };
                if (newCoordinates[1] > 7 && newCoordinates[0] < 0) return currentField;
                output = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                break;
            case Moves.DiagonalDownRight:
                newCoordinates = new List<int>() { currentField.Coordinates[0] + 1, currentField.Coordinates[1] + 1 };
                if (newCoordinates[1] > 7 && newCoordinates[0] > 7) return currentField;
                output = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                break;
            default:
                _logger.LogError("Enum value again out of scope!");
                break;
        }

        return output;
    }
    
    /// <summary>
    /// Check if some coordinates are already in a list or not.
    ///
    /// Adds or don't add them to the list dependently.
    /// </summary>
    /// <param name="list">The list of all coordinates.</param>
    /// <param name="coordinates">The to adding coordinates.</param>
    private static void AddCoordinatesToList(Collection<IList<int>> list, IList<int> coordinates)
    {
        if (Enumerable.Contains(list, coordinates))
        {
            return;
        }
        
        list.Add(coordinates);
    }
    
    /// <summary>
    /// Just checks if a given coordinate is not in the scope of the board data structure.
    /// </summary>
    /// <param name="coordinates">The coordinates to be checked.</param>
    /// <returns>Results of check if the coordinates are on the board.</returns>
    private static bool AreCoordinatesOnBoard(IList<int> coordinates)
    {
        foreach (var ordinate in coordinates)
        {
            if (ordinate is < 0 or > 7)
            {
                return false;
            }
        }

        return true;
    }
    
    
}