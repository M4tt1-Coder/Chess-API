namespace Chess_API.Enums;

/// <summary>
/// This is a custom string-enumeration.
/// 
/// Should represent a time every player gets in a the upcoming game.
///
/// It is set in the options-page by the player himself or herself.
///
/// It's string in the game instance can be null if the player wants to customize their time.
/// </summary>
public static class PlayTimeMode
{
    /// <summary>
    /// The players each have 3 minutes for their game.
    /// </summary>
    public static readonly string ThreeMinutes = "03:00";

    /// <summary>
    /// The players each have 10 minutes for their game.
    /// </summary>
    public static readonly string TenMinutes = "10:00";

    /// <summary>
    /// The players each have 30 minutes for their game.
    /// </summary>
    public static readonly string ThirtyMinutes = "30:00";
}