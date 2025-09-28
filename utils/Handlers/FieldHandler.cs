using Chess_API.Enums;
using Chess_API.Models;
using Chess_API.utils.Services;

namespace Chess_API.utils.Handlers;

/// <summary>
/// Its function is to provide a chess-field
/// with normal start layout.
/// </summary>
public static class FieldHandler
{
    //constants

    /// <summary>
    /// Indicates the number of rows in a chess game.
    /// </summary>
    private const int RowNumber = 8;

    /// <summary>
    /// Sets the number of fields in a row.
    /// </summary>
    private const int FieldNumber = 8;

    /// <summary>
    /// Determines if the user already selected a field or not.
    /// </summary>
    /// <param name="X">Nullable field X ordinate</param>
    /// <param name="Y">Nullable field Y ordinate</param>
    /// <param name="IsThereSelectedField">True when there is a selected field</param>
    public record FieldSelectedCheckResult(int? X, int? Y, bool IsThereSelectedField);

    /// <summary>
    /// Creates a default field layout.
    ///
    /// Field row starting with index 0 begins at the white side.
    /// </summary>
    /// <returns>Default list of eight field-rows</returns>
    public static List<FieldRowModel> Default()
    {
        var output = new List<FieldRowModel>();

        for (var i = 0; i < RowNumber; i++)
        {
            var row = new FieldRowModel(new List<FieldModel>(), i + 1);

            for (var j = 0; j < FieldNumber; j++)
            {
                switch (i % 2)
                {
                    case 0:
                        switch (j % 2)
                        {
                            case 0:
                                row.Row.Add(
                                    new FieldModel(Color.White, GetFigureModel(j, i), j, i)
                                );
                                break;
                            case 1:
                                row.Row.Add(
                                    new FieldModel(Color.Black, GetFigureModel(j, i), j, i)
                                );
                                break;
                        }

                        break;
                    case 1:
                        switch (j % 2)
                        {
                            case 0:
                                row.Row.Add(
                                    new FieldModel(Color.Black, GetFigureModel(j, i), j, i)
                                );
                                break;
                            case 1:
                                row.Row.Add(
                                    new FieldModel(Color.White, GetFigureModel(j, i), j, i)
                                );
                                break;
                        }

                        break;
                }
            }

            output.Add(row);
        }

        return output;
    }

    /// <summary>
    /// Goes through all positions of the figures in starting chess game layout.
    ///
    /// Depending on the row (starting at the white side with index 0) returns a specific colored figure.
    ///
    /// Set all pawns on the black and white side.
    /// </summary>
    /// <param name="fieldIndex">On which field the algorithm is current.</param>
    /// <param name="rowIndex">Which row the figure should be set.</param>
    /// <returns>A specific figure positioned on the board.</returns>
    private static FigureModel? GetFigureModel(int fieldIndex, int rowIndex)
    {
        return rowIndex switch
        {
            7 => DetermineFigure(fieldIndex, true),
            6 => new FigureModel(FigureType.Pawn, PictureSources.White_Pawn(), Color.White),
            1 => new FigureModel(FigureType.Pawn, PictureSources.Black_Pawn(), Color.Black),
            0 => DetermineFigure(fieldIndex, false),
            _ => null,
        };
    }

    /// <summary>
    /// Decides which color the figure has, depending on the row index.
    /// -> passed through boolean argument
    ///
    /// Sets all other figure types (bishop, knight, ...).
    /// </summary>
    /// <param name="fieldIndex">Field position in a row.</param>
    /// <param name="white">Indicates if a figure is white or not.</param>
    /// <returns>A figure model in the back rank lines of white or black.</returns>
    private static FigureModel DetermineFigure(int fieldIndex, bool white)
    {
        if (white)
        {
            switch (fieldIndex)
            {
                case 0:
                case 7:
                    return new FigureModel(
                        FigureType.Rook,
                        PictureSources.White_Rook(),
                        Color.White
                    );
                case 1:
                case 6:
                    return new FigureModel(
                        FigureType.Knight,
                        PictureSources.White_Knight(),
                        Color.White
                    );
                case 2:
                case 5:
                    return new FigureModel(
                        FigureType.Bishop,
                        PictureSources.White_Bishop(),
                        Color.White
                    );
                case 3:
                    return new FigureModel(
                        FigureType.King,
                        PictureSources.White_King(),
                        Color.White
                    );
                case 4:
                    return new FigureModel(
                        FigureType.Queen,
                        PictureSources.White_Queen(),
                        Color.White
                    );
            }
        }
        else
        {
            switch (fieldIndex)
            {
                case 0:
                case 7:
                    return new FigureModel(
                        FigureType.Rook,
                        PictureSources.Black_Rook(),
                        Color.Black
                    );
                case 1:
                case 6:
                    return new FigureModel(
                        FigureType.Knight,
                        PictureSources.Black_Knight(),
                        Color.Black
                    );
                case 2:
                case 5:
                    return new FigureModel(
                        FigureType.Bishop,
                        PictureSources.Black_Bishop(),
                        Color.Black
                    );
                case 3:
                    return new FigureModel(
                        FigureType.King,
                        PictureSources.Black_King(),
                        Color.Black
                    );
                case 4:
                    return new FigureModel(
                        FigureType.Queen,
                        PictureSources.Black_Queen(),
                        Color.Black
                    );
            }
        }

        return new FigureModel();
    }

    /// <summary>
    /// Searches in the whole board after a field with specific coordinates.
    ///
    /// Notifies you if such a field couldn't found.
    /// </summary>
    /// <param name="game">Current game</param>
    /// <param name="coordinates">Some coordinates on the board.</param>
    /// <returns>
    /// A field that has been found by coordinates.
    /// </returns>
    public static FieldModel GetSpecificFieldByCoordinates(GameModel game, List<int> coordinates)
    {
        var output = new FieldModel();

        // compare coordinates with every field in the game
        foreach (var row in game.Board)
        {
            foreach (var field in row.Row)
            {
                if (field.X == coordinates[0] && field.Y == coordinates[1])
                {
                    output = field;
                }
            }
        }

        return output;
    }

    /// <summary>
    /// Simply creates a copy of a field.
    ///
    /// A new instance of a field is created.
    /// </summary>
    /// <param name="field">The field that should be copied</param>
    /// <returns>A copy of the a field.</returns>
    public static FieldModel CopyField(FieldModel field)
    {
        return new FieldModel(field.Color, field.Content, field.X, field.Y)
        {
            MovableField = field.MovableField,
            SelectedField = field.SelectedField,
        };
    }

    /// <summary>
    /// Unselects the selected field in the game.
    /// </summary>
    /// <param name="game">Current game instance</param>
    /// <returns>Updated game object</returns>
    public static GameModel UnselectAllFields(GameModel game)
    {
        foreach (var row in game.Board)
        {
            foreach (var field in row.Row)
            {
                if (field.Content is not null)
                {
                    field.SelectedField = false;
                }
            }
        }

        return game;
    }

    /// <summary>
    /// Checks if a field has been selected by the user.
    ///
    /// Fails when more than 2 have been selected somehow.
    /// </summary>
    /// <param name="game">Current game instance</param>
    /// <returns>Result with possible field coordinates.</returns>
    /// <exception cref="Exception">If too many fields where selected</exception>
    public static FieldSelectedCheckResult IsAFieldSelected(GameModel game)
    {
        var output = new FieldSelectedCheckResult(null, null, false);
        var numFieldsSelected = 0;
        foreach (var row in game.Board)
        {
            foreach (var field in row.Row)
            {
                if (field.Content is not null && field.SelectedField)
                {
                    output = new FieldSelectedCheckResult(field.X, field.Y, true);
                    numFieldsSelected++;
                }
            }
        }

        if (numFieldsSelected >= 2)
        {
            throw new Exception(
                "Too many fields have been selected! Number of Fields: " + numFieldsSelected
            );
        }

        return output;
    }

    /// <summary>
    /// Checks the field coordinates and sets the field selected.
    /// </summary>
    /// <param name="game">Current game instance</param>
    /// <param name="coordinates">Coordinates of the field to be checked</param>
    /// <returns>Updated game instance</returns>
    public static GameModel SetFieldSelected(GameModel game, List<int> coordinates)
    {
        foreach (var row in game.Board)
        {
            foreach (var field in row.Row)
            {
                if (field.X != coordinates[0] || field.Y != coordinates[1] || field.Content is null)
                    continue;
                if (
                    (field.Content.Color == Color.White && game.PlayerTurn == PlayerTurn.White)
                    || (field.Content.Color == Color.Black && game.PlayerTurn == PlayerTurn.Black)
                )
                {
                    field.SelectedField = !field.SelectedField;
                }
            }
        }

        return game;
    }
}
