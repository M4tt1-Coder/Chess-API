using Chess_API.Enums;
using Chess_API.Models;
using Chess_API.MovePatterns;
using Chess_API.Rules;
using Chess_API.utils.Handlers;
using Chess_API.utils.UtilTypes;

namespace Chess_API.utils.Executors;

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
public static class RulesExecutor
{
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
    /// <returns>The Updated game object</returns>
    public static GameModel ValidateMove(GameModel game, FieldModel curField, FieldModel newField)
    {
        if (curField.Content is null)
        {
            return game;
        }
        
        var currentFieldFigureId = curField.Content.FigureId;
        
        // can piece move to the field
        if (!MovingRules.CanPieceMoveToFieldWithCheck(game, new List<int>() { curField.X, curField.Y },
                new List<int>() { newField.X, newField.Y })) return game;
        
        // add the move to the history
        MoveHistoryHandler.AddMove(game, new MoveModel(game.MoveHistory.Count + 1, currentFieldFigureId,
            new List<int> { curField.X, curField.Y }, new List<int> { newField.X, newField.Y },
            PlayerHandler.GetPlayerIdOnTurn(game)));
        // modify who's turn it is
        PlayerHandler.ChangePlayerTurn(game);
        // move the figure to the new field
        game = MoveFigureToField(game, new List<int> { curField.X, curField.Y }, new List<int>() { newField.X, newField.Y });

        // return the game object
        return game;
    }

    /// <summary>
    /// Differentiates between the two cases when the player made a move and his / her own king is in check AND
    /// when the player made a move and the opponent's king is in check.
    /// </summary>
    /// <param name="game">Current game instance</param>
    /// <param name="kingColor">Color of the king</param>
    /// <param name="figureNow">Starting position of the piece</param>
    /// <param name="figureAfter">Ending position of the piece</param>
    /// <returns>True, when the king is in check</returns>
    public static bool CheckChecker(GameModel game, Color kingColor, List<int>? figureNow = null, List<int>? figureAfter = null)
    {
        bool output;
        var kingCoordinates = new List<int>();

        // determine kings location
        foreach (var row in game.Board)
        {
            foreach (var field in row.Row)
            {
                if (field.Content is null) continue;
                if (field.Content.Type == FigureType.King && field.Content.Color == kingColor)
                {
                    kingCoordinates = new List<int> { field.X, field.Y };
                }
            }
        }
        // if its just a general check or when a piece is potentially moving
        
        // when the player has made move which is valid (didn't cause a check on its own king)
        // but gave a check to the opponents king
        if (figureNow is null || figureAfter is null)
        {
            // when in one move pattern of some opponent piece the same coordinates as the kings appear there is a check
            output = IsKingInCheck(game, kingCoordinates, kingColor).IsInCheck;
        }
        else
        {
            // go through all opposite figures and check for possible checks
            // when a figure can move to the king's field -> check        
            // copy instance of the game to check if the move is valid
            var gameCopy = GameHandler.CopyGame(game);
            // check if you are moving your own king
            if (IsKingOnThisField(game, figureNow))
            {
                // when the king is moved -> update the coordinates
                kingCoordinates = figureAfter;
            }
            // move the figure to the new field
            gameCopy = MoveFigureToField(gameCopy, figureNow, figureAfter);
            // check if the king is in check
            output = IsKingInCheck(gameCopy, kingCoordinates, kingColor).IsInCheck; 
        }
        return output;
    }

    /// <summary>
    /// Figures of the opposite color doesn't matter.
    ///
    /// Iterates through all fields on the board -> when a piece of the opposite color is on the current field
    /// -> all fields that are attacked by it will be added to the coordinate list
    ///
    /// Fails when the 'field' attribute is null.
    ///
    /// Helps to determine where a king of one color is in a check or where he can move.
    /// </summary>
    /// <param name="game">Current game instance</param>
    /// <param name="kingColor">Either white / black : Color of the king</param>
    /// <returns>List record with all attacked fields</returns>
    private static AttackedFieldsList FieldsWhereKingIsAttacked(GameModel game, Color kingColor)
    {
        // field list which can be assigned to a piece
        var associatedWithPiece = new List<CoveredFieldOfPieceObjects>();
        
        // similar to move patterns but not moving but attacking
        // go along all possible move patterns
        // store fields which are attacked by figures of 
        foreach (var row in game.Board)
        {
            foreach (var field in row.Row)
            {
                // continue the iteration when: there is now figure || the figure is of the same color as the king
                if (field.Content is null || field.Content.Color == kingColor) continue;
                
                // Piece coordinates
                var pieceCoordinates = new List<int> { field.X, field.Y };                
                
                // list of coordinates of attacked fields
                var fieldsAttackedByPiece = new List<List<int>>();
                
                // add the field where the piece is situated
                if (AreCoordinatesOnBoard(new List<int>() { field.X, field.Y }))
                {
                    AddCoordinatesToList(fieldsAttackedByPiece, new List<int>() { field.X, field.Y });
                }
                
                switch (field.Content.Type)
                { 
                    case FigureType.Pawn:
                        // for pawns, I need to know in which direction they are turned to 
                        switch(game.Direction)
                        {
                            case PlayingDirection.WhiteTop:
                                if (kingColor == Color.White)
                                {
                                    // check for boundaries 
                                    // assign new possible fields that could be attacked by black piece
                                    var firstField = new List<int> { field.X - 1, field.Y - 1 };
                                    var secondField = new List<int>() { field.X + 1, field.Y - 1 };

                                    if (AreCoordinatesOnBoard(firstField))
                                    {
                                        AddCoordinatesToList(fieldsAttackedByPiece, firstField);
                                    }

                                    if (AreCoordinatesOnBoard(secondField))
                                    {
                                        AddCoordinatesToList(fieldsAttackedByPiece, secondField);
                                    }
                                }
                                else
                                {
                                    var firstField = new List<int> { field.X - 1, field.Y + 1 };
                                    var secondField = new List<int> { field.X + 1, field.Y + 1 };

                                    if (AreCoordinatesOnBoard(firstField))
                                    {
                                        AddCoordinatesToList(fieldsAttackedByPiece, firstField);
                                    }

                                    if (AreCoordinatesOnBoard(secondField))
                                    {
                                        AddCoordinatesToList(fieldsAttackedByPiece, secondField);
                                    }
                                }
                                break;
                            case PlayingDirection.WhiteBottom:
                                if (kingColor == Color.White)
                                {
                                    var firstField = new List<int> { field.X - 1, field.Y + 1 };
                                    var secondField = new List<int> { field.X + 1, field.Y + 1 };

                                    if (AreCoordinatesOnBoard(firstField))
                                    {
                                        AddCoordinatesToList(fieldsAttackedByPiece, firstField);
                                    }

                                    if (AreCoordinatesOnBoard(secondField))
                                    {
                                        AddCoordinatesToList(fieldsAttackedByPiece, secondField);
                                    }
                                }
                                else
                                {
                                    var firstField = new List<int> { field.X - 1, field.Y - 1 };
                                    var secondField = new List<int> { field.X + 1, field.Y - 1 };

                                    if (AreCoordinatesOnBoard(firstField))
                                    {
                                        AddCoordinatesToList(fieldsAttackedByPiece, firstField);
                                    }

                                    if (AreCoordinatesOnBoard(secondField))
                                    {
                                        AddCoordinatesToList(fieldsAttackedByPiece, secondField);
                                    }
                                }
                                break;
                        }
                        break;
                    case FigureType.Bishop:
                        var bishopMovePatterns = new BishopMovePattern().Patterns.ToList();
                        // run along the move patterns
                        foreach (var pattern in bishopMovePatterns)
                        {
                            var canStillContinue = true;
                            var nextField = FieldHandler.CopyField(field);

                            var currentPattern = pattern.ToList();

                            // go one default step before starting the repetitive step execution 
                            nextField = StepExecutor.GoStepStraight(currentPattern[0], game, nextField,
                                kingColor == Color.White ? Color.Black : Color.White, true);
                            // also add the additional field to the output list
                            if (AreCoordinatesOnBoard(new List<int>() { nextField.X, nextField.Y }))
                            {
                                var coordinatesToBeAdded = new List<int> { nextField.X, nextField.Y }; 
                                AddCoordinatesToList(fieldsAttackedByPiece, coordinatesToBeAdded);
                            }
                            
                            // repeat to go as long as possible along one pattern
                            // just for straight move pattern
                            while (canStillContinue)
                            {
                                var previousField = nextField;

                                // go along the pattern
                                nextField = currentPattern.Aggregate(nextField, (current, move) => 
                                    StepExecutor.GoStepStraight(move, game, current, 
                                        kingColor == Color.White ? Color.Black : Color.White));

                                // check if the field where the figure has moved has changed
                                if (previousField.X == nextField.X && previousField.Y == nextField.Y)
                                {
                                    canStillContinue = false;
                                }
                                else
                                {
                                    // add the field to the list
                                    if (!AreCoordinatesOnBoard(new List<int>() { nextField.X, nextField.Y })) continue;
                                    var coordinatesToBeAdded = new List<int> { nextField.X, nextField.Y }; 
                                    AddCoordinatesToList(fieldsAttackedByPiece, coordinatesToBeAdded);
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
                            var nextField = FieldHandler.CopyField(field);
                            var moveCount = 0;
                            // when it reaches 2 -> knight could land on that spot
                            foreach (var move in pattern)
                            {
                                var previousField = nextField;
                                // first go the step
                                nextField = StepExecutor.GoStepKnight(move, game, previousField, field.Content.Color, true);
                                // increase counter by 1
                                moveCount++;
                                // when 2 steps have been taken -> check if previous field is equal to field after
                                // the moving operation -> continue
                                if (moveCount != 2) continue;
                                if (previousField == nextField) continue;
                                var coordinatesToBeAdded = new List<int> { nextField.X, nextField.Y };
                                AddCoordinatesToList(fieldsAttackedByPiece, coordinatesToBeAdded);
                            }
                        }
                        break;
                    case FigureType.Rook:
                        var rookMovePatterns = new RookMovePattern().Patterns.ToList();
                        // rook has 4 moving opportunities
                        foreach (var pattern in rookMovePatterns)
                        {
                            var canStillContinueRun = true;
                            var nextField = FieldHandler.CopyField(field);

                            var currentPattern = pattern.ToList();
                            
                            // go one default step before starting the repetitive step execution 
                            nextField = StepExecutor.GoStepStraight(currentPattern[0], game, nextField,
                                kingColor == Color.White ? Color.Black : Color.White, true);
                            // also add the additional field to the output list
                            if (AreCoordinatesOnBoard(new List<int> { nextField.X, nextField.Y }))
                            {
                                AddCoordinatesToList(fieldsAttackedByPiece, new List<int> { nextField.X, nextField.Y });
                            }
                            
                            // repeat to go as long as possible along one pattern
                            // just for straight move pattern
                            while (canStillContinueRun)
                            {
                                var previousField = nextField;

                                nextField = currentPattern.Aggregate(nextField, (current, move) => 
                                    StepExecutor.GoStepStraight(move, game, current, 
                                        kingColor == Color.White ? Color.Black : Color.White));

                                if (previousField.X == nextField.X && previousField.Y == nextField.Y)
                                {
                                    canStillContinueRun = false;
                                }
                                else
                                {
                                    // add the field to the list
                                    if (AreCoordinatesOnBoard(new List<int>() { nextField.X, nextField.Y }))
                                    {
                                        var coordinatesToBeAdded = new List<int> { nextField.X, nextField.Y };
                                        AddCoordinatesToList(fieldsAttackedByPiece, coordinatesToBeAdded);
                                    }
                                }
                            }
                        }
                        break;
                    case FigureType.Queen:
                        var queenMovePatterns = new QueenMovePatterns().Patterns.ToList();
                        foreach (var pattern in queenMovePatterns)
                        {
                            var canStillContinueRun = true;
                            var nextField = FieldHandler.CopyField(field);
                            
                            var currentPattern = pattern.ToList();
                            
                            // go one default step before starting the repetitive step execution 
                            nextField = StepExecutor.GoStepStraight(currentPattern[0], game, nextField,
                                kingColor == Color.White ? Color.Black : Color.White, true);
                            // also add the additional field to the output list
                            if (AreCoordinatesOnBoard(new List<int>() { nextField.X, nextField.Y }))
                            {
                                var coordinatesToBeAdded = new List<int> { nextField.X, nextField.Y };
                                AddCoordinatesToList(fieldsAttackedByPiece, coordinatesToBeAdded);
                            }
                            
                            // repeat to go as long as possible along one pattern
                            // just for straight move pattern
                            while (canStillContinueRun)
                            {
                                var previousField = nextField;

                                nextField = currentPattern.Aggregate(nextField, (current, move) => StepExecutor.GoStepStraight(move, game, current, kingColor == Color.White ? Color.Black : Color.White));

                                if (nextField.X == previousField.X && nextField.Y == previousField.Y)
                                {
                                    canStillContinueRun = false;
                                }
                                else
                                {
                                    // add the field to the list
                                    var coordinatesToBeAdded = new List<int> { nextField.X, nextField.Y };
                                    if (!AreCoordinatesOnBoard(coordinatesToBeAdded)) continue;
                                    AddCoordinatesToList(fieldsAttackedByPiece, coordinatesToBeAdded);
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
                            if (field.X == nextField.X && field.Y == nextField.Y) continue;
                            var coordinatesToBeAdded = new List<int> { nextField.X, nextField.Y };
                            AddCoordinatesToList(fieldsAttackedByPiece, coordinatesToBeAdded);
                        }
                        break;
                }
                
                // add the list of attacked fields related to the piece to the list
                associatedWithPiece.Add(new CoveredFieldOfPieceObjects(fieldsAttackedByPiece, pieceCoordinates));
            }
        }

        return new AttackedFieldsList(associatedWithPiece);
    }
    
    /// <summary>
    /// Checks if one of the players lost and then applies modifications to the active game instance.
    /// </summary>
    /// <param name="game">Current game instance</param>
    /// <returns>Updated game object after executing all necessary checks.</returns>
    /// <exception cref="Exception">When functions return invalid data or fail the work properly.</exception>
    public static GameModel HasGameEnded(GameModel game)
    {
        // validate if the game ended in a draw this round
        if (PlayerHandler.CanNotMakeAMoveAnymore(game))
        {
            game = GameHandler.ApplyChangesAfterGameEnded(game, Winner.Draw);
            return game;
        }
        
        // check if an own piece of the kings color can throw the piece which checks the king
        // when the king is in check -> check if he can move to a field where he is not in check anymore
        // when the king is in check -> check if a piece can stop 
        
        // check if player one lost
        if (CheckChecker(game, game.PlayerOne.PieceColor))
        {
            var kingCoordinates = GetKingCoordinates(game, game.PlayerOne.PieceColor);
            var checkResult = IsKingInCheck(game, kingCoordinates,
                game.PlayerOne.PieceColor);
            
            // validate returned output
            if (!checkResult.IsInCheck || checkResult.AttackingPieceCoordinates is null)
            {
                throw new Exception("In the check validation process an invalid result was returned! " +
                                    "King is in check but an attacking piece couldn't be found OR two different results have been returned!");
            }

            var finalCheckResult = CanKingEscapeCheckByMoving(game, kingCoordinates, game.PlayerOne.PieceColor,
                FieldsWhereKingIsAttacked(game, game.PlayerOne.PieceColor).GetAllAttackedFields()) 
                                   || CanKingAvoidCheckByThrowingPiece(game, checkResult.AttackingPieceCoordinates!, game.PlayerOne.PieceColor) 
                                   || CanAPieceBlockCheck(game, checkResult.AttackingPieceCoordinates!, game.PlayerOne.PieceColor, kingCoordinates);

            if (!finalCheckResult)
            {
                // set winner
                // set win score of successful player
                game = GameHandler.ApplyChangesAfterGameEnded(game, Winner.PlayerTwo);
            }
        }
        
        // check if player two lost
        if (!CheckChecker(game, game.PlayerTwo.PieceColor)) return game;
        {
            var kingCoordinates = GetKingCoordinates(game, game.PlayerTwo.PieceColor);
            var checkResult = IsKingInCheck(game, kingCoordinates,
                game.PlayerTwo.PieceColor);

            // validate returned output
            if (!checkResult.IsInCheck || checkResult.AttackingPieceCoordinates is null)
            {
                throw new Exception("In the check validation process an invalid result was returned! " +
                                    "King is in check but an attacking piece couldn't be found OR two different results have been returned!");
            }

            // first try to move the king to a field where he is not in check anymore (*)
            // second try to throw the piece which checks the king (*)
            // third try to block the piece which checks the king (*)
            // when all three cases are false -> player one has lost

            var finalCheckResult = CanKingEscapeCheckByMoving(game, kingCoordinates, game.PlayerTwo.PieceColor,
                                       FieldsWhereKingIsAttacked(game, game.PlayerTwo.PieceColor)
                                           .GetAllAttackedFields())
                                   || CanKingAvoidCheckByThrowingPiece(game, checkResult.AttackingPieceCoordinates!,
                                       game.PlayerTwo.PieceColor)
                                   || CanAPieceBlockCheck(game, checkResult.AttackingPieceCoordinates!,
                                       game.PlayerTwo.PieceColor, kingCoordinates);

            if (!finalCheckResult)
            {
                // set winner
                // set win score of successful player
                game = GameHandler.ApplyChangesAfterGameEnded(game, Winner.PlayerOne);
            }
        }

        // return if no player has lost
        return game;
    }

    // ----- Helper functions -----
    
    /// <summary>
    /// Determines, whether a piece can block a check of the king or not.
    /// </summary>
    /// <param name="game">Current game instance</param>
    /// <param name="pieceCoordinates">Field, where the attacking piece is located</param>
    /// <param name="kingColor">Color of the own king</param>
    /// <param name="kingCoordinates">Coordinates of the field where the king is situated.</param>
    /// <returns>True, when one own piece of the king in check can block the attack</returns>
    private static bool CanAPieceBlockCheck(GameModel game, List<int> pieceCoordinates, Color kingColor, List<int> kingCoordinates)
    {
        var output = false;
        var attackedFields = FieldsWhereKingIsAttacked(game, kingColor == Color.White ? Color.Black : Color.White);
        var fieldsAttackedByPiece = new List<List<int>>();
        var fieldOfPiece = FieldHandler.GetSpecificFieldByCoordinates(game, pieceCoordinates);
        
        // continue when there is no piece or the piece is an attacking pawn / knight -> they can't be blocked
        if (fieldOfPiece.Content is null || fieldOfPiece.Content.Type == FigureType.Knight || 
            fieldOfPiece.Content.Type == FigureType.Pawn)
        {
            return false;
        }
        
        // determine fields that are attacked by the piece that checks the king
        foreach (var attackedFieldsByPiece in attackedFields.CoveredFieldOfPieceObjects
                     .Where(attackedFieldsByPiece => attackedFieldsByPiece.PieceCoordinates == pieceCoordinates))
        {
            fieldsAttackedByPiece = attackedFieldsByPiece.CoveredFields;
        }
        
        // go through all own pieces and check if they can move to one of the fields
        // that are attacked by the piece that checks the king
        
        foreach (var row in game.Board)
        {
            foreach (var field in row.Row)
            {
                // continue when there is no piece or the piece is of the opposite color
                if (field.Content is null || field.Content.Color != kingColor) continue;

                var allyPieceCoordinates = new List<int> { field.X, field.Y };
                
                // go through all fields that are attacked by the piece that checks the king
                foreach(var potentialField in fieldsAttackedByPiece.Where(attackedField => MovingRules.CanPieceMoveToFieldWithCheck(game,
                            allyPieceCoordinates, attackedField)))
                {
                    // move figure to field and see if the check was blocked
                    var gameCopy = GameHandler.CopyGame(game);

                    gameCopy = MoveFigureToField(gameCopy, allyPieceCoordinates, potentialField);
                    
                    // validate if the king is still in check
                    var checkResult = IsKingInCheck(gameCopy, kingCoordinates, kingColor);

                    if (!checkResult.IsInCheck)
                    {
                        output = true;
                    }
                }
            }
        }
        
        return output;
    } 
    
    /// <summary>
    /// Looks, if when a king is in Check, that one of his piece can throw the attacking piece.
    /// </summary>
    /// <param name="game">Current game instance</param>
    /// <param name="pieceCoordinates">Coordinates of the attacking piece</param>
    /// <param name="kingColor">Color of the king</param>
    /// <returns>True, when the attacking piece can be thrown</returns>
    private static bool CanKingAvoidCheckByThrowingPiece(GameModel game, List<int> pieceCoordinates, Color kingColor)
    {
        var output = false;
        // just get all fields that are attacked by the kings own pieces
        var allAttackedFields = FieldsWhereKingIsAttacked(game, kingColor == Color.White ? Color.Black : Color.White).GetAllAttackedFields();

        // compare with pieceCoordinates
        foreach (var _ in allAttackedFields.Where(field => field[0] == pieceCoordinates[0] && field[1] == pieceCoordinates[1] &&
                                                               field[2] == pieceCoordinates[2]))
        {
            output = true;
        }

        return output;
    }
    
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
    private static FieldModel KingJustTriesToGoToField(Move kingMove, FieldModel currentField, GameModel game)
    {
        // vars instantiation
        var output = new FieldModel();
        List<int> newCoordinates;
        // cover all moves
        switch (kingMove)
        {
            case Move.Up:
                // get the new coordinates
                newCoordinates = new List<int>() { currentField.X, currentField.Y - 1 };
                // validate new coordinates
                if (newCoordinates[1] < 0) return currentField;
                // get new field with new coordinates from game board
                // assign to output var
                output = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                break;
            case Move.Down:
                newCoordinates = new List<int>() { currentField.X, currentField.Y + 1};
                if (newCoordinates[1] > 7) return currentField;
                output = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                break;
            case Move.Left:
                newCoordinates = new List<int>() { currentField.X - 1, currentField.Y};
                if (newCoordinates[0] < 0) return currentField;
                output = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                break;
            case Move.Right:
                newCoordinates = new List<int>() { currentField.X, currentField.Y + 1 };
                if (newCoordinates[0] > 7) return currentField;
                output = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                break;
            case Move.DiagonalUpLeft:
                newCoordinates = new List<int>() { currentField.X - 1, currentField.Y - 1 };
                if (newCoordinates[1] < 0 && newCoordinates[0] < 0) return currentField;
                output = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                break;
            case Move.DiagonalUpRight:
                newCoordinates = new List<int>() { currentField.X + 1, currentField.Y - 1 };
                if (newCoordinates[1] < 0 && newCoordinates[0] > 7) return currentField;
                output = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                break;
            case Move.DiagonalDownLeft:
                newCoordinates = new List<int>() { currentField.X - 1, currentField.Y + 1 };
                if (newCoordinates[1] > 7 && newCoordinates[0] < 0) return currentField;
                output = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
                break;
            case Move.DiagonalDownRight:
                newCoordinates = new List<int>() { currentField.X + 1, currentField.Y + 1 };
                if (newCoordinates[1] > 7 && newCoordinates[0] > 7) return currentField;
                output = FieldHandler.GetSpecificFieldByCoordinates(game, newCoordinates);
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
    private static void AddCoordinatesToList(List<List<int>> list, List<int> coordinates)
    {
        if (IsFieldAlreadyInList(list, coordinates))
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
    private static bool AreCoordinatesOnBoard(List<int> coordinates)
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
    
    /// <summary>
    /// Moves a piece to a new field by changing the content of the fields.
    /// </summary>
    /// <param name="game">Current game instance</param>
    /// <param name="from">Starting coordinates of the piece</param>
    /// <param name="to">Destination of the figure</param>
    /// <returns>New game instance</returns>   
    private static GameModel MoveFigureToField(GameModel game, List<int> from, List<int> to)
    {
        if (FieldHandler.GetSpecificFieldByCoordinates(game, from).Content is null)
        {
            return game;
        }
        // get the piece object
        var piece = FieldHandler.GetSpecificFieldByCoordinates(game, from).Content!;
        // reassign the field where the piece is situated
        game.Board[from[1]].Row[from[0]].Content = null;
        // assign the piece to the new field
        game.Board[to[1]].Row[to[0]].Content = piece;
        
        // return the new game instance
        return game;   
    }

    /// <summary>
    /// Checks whether a given set of coordinates is already present in a collection of coordinate lists.
    /// </summary>
    /// <param name="list">A collection containing sets of coordinates.</param>
    /// <param name="coordinates">The specific coordinates to check for existence in the collection.</param>
    /// <returns>True if the coordinates are already in the list, otherwise false.</returns>
    private static bool IsFieldAlreadyInList(List<List<int>> list, List<int> coordinates)
    {
        return list.Any(c => c[0] == coordinates[0] && c[1] == coordinates[1]);
    }

    /// <summary>
    /// Determines if the King is located on the specified field coordinates within the game.
    /// </summary>
    /// <param name="game">The current state of the game.</param>
    /// <param name="figureCoordinates">The coordinates of the field to check.</param>
    /// <returns>True if the field contains a King; otherwise, false.</returns>
    private static bool IsKingOnThisField(GameModel game, List<int> figureCoordinates)
    {
        var field = FieldHandler.GetSpecificFieldByCoordinates(game, figureCoordinates);
        if (field.Content is null)
        {
            return false;
        }
        return field.Content.Type == FigureType.King;
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
    /// <returns>True and the coordinates of the piece which checks him, if the king is in check </returns>
    private static KingInCheckResult IsKingInCheck(GameModel game, List<int> kingCoordinates, Color kingColor)
    {
        var output = false;
        var canEscapeByMoving = false;
        List<int>? coordinates = null;
        // go through board -> check if opposite color -> execute patterns
        // get all fields which are attacked by the opponents pieces
        var attackedFields = FieldsWhereKingIsAttacked(game, kingColor);

        // compare king's coordinates with all coordinates where he is in check
        foreach (var fieldsOfOnePiece in attackedFields.CoveredFieldOfPieceObjects.
                     Where(fieldsOfOnePiece => fieldsOfOnePiece.CoveredFields.Any(singleCoordinates =>
                     singleCoordinates[0] == kingCoordinates[0] && singleCoordinates[1] == kingCoordinates[1])))
        {
            output = true;
            // assign the coordinates of the piece which checks the king
            coordinates = fieldsOfOnePiece.PieceCoordinates;

            // Immediately check if the king can escape by moving to field around him
            canEscapeByMoving = CanKingEscapeCheckByMoving(game, kingCoordinates, kingColor, attackedFields.GetAllAttackedFields());
            
            break;
        }

        
        
        return new KingInCheckResult(output, coordinates, canEscapeByMoving);
    }

    /// <summary>
    /// Makes sure that a king in check can try to move to another field to avoid the check.
    /// </summary>
    /// <param name="game">Current Game instance</param>
    /// <param name="kingCoordinates">Coordinates of the king</param>
    /// <param name="kingColor">Color of the king</param>
    /// <param name="attackedFields">List of Field coordinates where the king would be in check</param>
    /// <returns>True, if the king can escape to field</returns>
    private static bool CanKingEscapeCheckByMoving(GameModel game, List<int> kingCoordinates, Color kingColor, List<List<int>> attackedFields)
    {
        var fieldsCanMoveTo = GetListOfFieldsWhereKingCanMove(game, kingCoordinates, kingColor);

        return fieldsCanMoveTo.Any(field => !IsFieldAlreadyInList(attackedFields, field));
    }
    
    /// <summary>
    /// Tries to move a king to all fields on the board he can move to.
    ///
    /// Collects valid surrounding fields and returns them.
    /// </summary>
    /// <param name="game">Current game instance</param>
    /// <param name="kingCoordinates">Coordinates of the king</param>
    /// <param name="kingColor">Color of the king</param>
    /// <returns>List of Coordinates of Fields, where a king could potentially move to</returns>
    private static List<List<int>> GetListOfFieldsWhereKingCanMove(GameModel game, List<int> kingCoordinates, Color kingColor)
    {
        var output = new List<List<int>>();

        var fieldOfKing = FieldHandler.GetSpecificFieldByCoordinates(game, kingCoordinates);

        var kingMovePattern = new KingMovePatterns();

        foreach (var pattern in kingMovePattern.Patterns)
        {
            output.AddRange(from move in pattern select StepExecutor.GoStepKing(move, game, fieldOfKing, kingColor) 
                into potentialField where potentialField.X != fieldOfKing.X || potentialField.Y != fieldOfKing.Y
                select new List<int> { potentialField.X, potentialField.Y });
        }
        
        return output;
    }

    /// <summary>
    /// Gets either the coordinates of the white or black king on the board.
    /// </summary>
    /// <param name="game">Current game object with board information</param>
    /// <param name="kingColor">Color of the king which's coordinates should be returned.</param>
    /// <returns>Coordinates of the requested king</returns>
    private static List<int> GetKingCoordinates(GameModel game, Color kingColor)
    {
        var output = new List<int>();

        foreach (var row in game.Board)
        {
            foreach (var field in row.Row)
            {
                if (IsKingOnThisField(game, new List<int> {field.X, field.Y}) &&
                    field.Content!.Color == kingColor) output = new List<int> { field.X, field.Y };
            }
        }
        return output;
    }
}

