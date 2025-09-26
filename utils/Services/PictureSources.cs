namespace Chess_API.utils.Services;

/// <summary>
/// Holds all picture sources to pass them through methods to the frontend pages.
/// </summary>
public static class PictureSources
{
    //white figures ------------

    /// <summary>
    /// Retrieves the directory path to the white pawn image.
    /// </summary>
    /// <returns>The string path to the white pawn.</returns>
    public static string White_Pawn()
    {
        return Environment.GetEnvironmentVariable("PAWN_WHITE_PATH")!;
    }

    /// <summary>
    /// Gets the path to the white rook image.
    /// </summary>
    /// <returns>A string path to the white rook.</returns>
    public static string White_Rook()
    {
        return Environment.GetEnvironmentVariable("ROOK_WHITE_PATH")!;
    }

    /// <summary>
    /// Gets the path to the white knight image.
    /// </summary>
    /// <returns>A string path to the white knight.</returns>
    public static string White_Knight()
    {
        return Environment.GetEnvironmentVariable("KNIGHT_WHITE_PATH")!;
    }

    /// <summary>
    /// Sends the path to the white bishop from the environment.
    /// </summary>
    /// <returns>A string path to the white bishop.</returns>
    public static string White_Bishop()
    {
        return Environment.GetEnvironmentVariable("BISHOP_WHITE_PATH")!;
    }

    /// <summary>
    /// Gets the path to the white queen.
    /// </summary>
    /// <returns>A string path to the white queen.</returns>
    public static string White_Queen()
    {
        return Environment.GetEnvironmentVariable("QUEEN_WHITE_PATH")!;
    }

    /// <summary>
    /// Gets the path to the white king.
    /// </summary>
    /// <returns>A string path to the white king.</returns>
    public static string White_King()
    {
        return Environment.GetEnvironmentVariable("KING_WHITE_PATH")!;
    }

    //black figures ------------

    /// <summary>
    /// Gets the path to the black pawn.
    /// </summary>
    /// <returns>A string path to the black pawn.</returns>
    public static string Black_Pawn()
    {
        return Environment.GetEnvironmentVariable("PAWN_BLACK_PATH")!;
    }

    /// <summary>
    /// Gets the path to the black rook.
    /// </summary>
    /// <returns>A string path to the black rook.</returns>
    public static string Black_Rook()
    {
        return Environment.GetEnvironmentVariable("ROOK_BLACK_PATH")!;
    }

    /// <summary>
    /// Gets the path to the black knight.
    /// </summary>
    /// <returns>A string path to the black knight.</returns>
    public static string Black_Knight()
    {
        return Environment.GetEnvironmentVariable("KNIGHT_BLACK_PATH")!;
    }

    /// <summary>
    /// Gets the path to the black bishop.
    /// </summary>
    /// <returns>A string path to the black bishop.</returns>
    public static string Black_Bishop()
    {
        return Environment.GetEnvironmentVariable("BISHOP_BLACK_PATH")!;
    }

    /// <summary>
    /// Gets the path to the black queen.
    /// </summary>
    /// <returns>A string path to the black queen.</returns>
    public static string Black_Queen()
    {
        return Environment.GetEnvironmentVariable("QUEEN_BLACK_PATH")!;
    }

    /// <summary>
    /// Gets the path to the black king.
    /// </summary>
    /// <returns>A string path to the black king.</returns>
    public static string Black_King()
    {
        return Environment.GetEnvironmentVariable("KING_BLACK_PATH")!;
    }
}
