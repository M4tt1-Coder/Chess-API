using Chess_API.Enums;
using Chess_API.Models;

namespace Chess_API.utils;

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
    /// Creates a default field layout.
    ///
    /// Field row starting with index 0 begins at the white side.
    /// </summary>
    /// <returns>Default list of eight field-rows</returns>
    public static List<FieldRowModel> Default()
    {
        List<FieldRowModel> output = new List<FieldRowModel>();

        for (int i = 0; i < RowNumber; i++)
        {
            var row = new FieldRowModel(new List<FieldModel>(), i + 1);

            for (int j = 0; j < FieldNumber; j++)
            {
                if (i % 2 == 0)
                {
                    if (j % 2 == 0)
                    {
                        row.Row.Add(new FieldModel(Colors.White, GetFigureModel(j, i), new []{ j, i }));
                    }
                    else if (j % 2 == 1)
                    {
                        row.Row.Add(new FieldModel(Colors.Black, GetFigureModel(j, i), new[] { j, i }));
                    }
                }
                else if (i % 2 == 1)
                {
                    if (j % 2 == 0)
                    {
                        row.Row.Add(new FieldModel(Colors.Black, GetFigureModel(j, i), new[] { j, i }));
                    }
                    else if (j % 2 == 1)
                    {
                        row.Row.Add(new FieldModel(Colors.White, GetFigureModel(j, i), new[] { j, i }));
                    }
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
            0 => DetermineFigure(fieldIndex, true),
            1 => new FigureModel(FigureType.Pawn, false, PictureSources.White_Pawn(), Colors.White),
            6 => new FigureModel(FigureType.Pawn, false, PictureSources.Black_Pawn(), Colors.Black),
            7 => DetermineFigure(fieldIndex, false),
            _ => null
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
                    return new FigureModel(FigureType.Rook, false, PictureSources.White_Rook(), Colors.White);
                case 1:
                case 6:
                    return new FigureModel(FigureType.Knight, false, PictureSources.White_Knight(), Colors.White);
                case 2:
                case 5:
                    return new FigureModel(FigureType.Bishop, false, PictureSources.White_Bishop(), Colors.White);
                case 3:
                    return new FigureModel(FigureType.King, false, PictureSources.White_King(), Colors.White);
                case 4:
                    return new FigureModel(FigureType.Queen, false, PictureSources.White_Queen(), Colors.White);
            }
        }
        else
        {
            switch (fieldIndex)
            {
                case 0:
                case 7:
                    return new FigureModel(FigureType.Rook, false, PictureSources.Black_Rook(), Colors.Black);
                case 1:
                case 6:
                    return new FigureModel(FigureType.Knight, false, PictureSources.Black_Knight(), Colors.Black);
                case 2:
                case 5:
                    return new FigureModel(FigureType.Bishop, false, PictureSources.Black_Bishop(), Colors.Black);
                case 3:
                    return new FigureModel(FigureType.King, false, PictureSources.Black_King(), Colors.Black);
                case 4:
                    return new FigureModel(FigureType.Queen, false, PictureSources.Black_Queen(), Colors.Black);
            }
        }

        return new FigureModel();
    }
}