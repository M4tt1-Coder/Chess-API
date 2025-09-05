using System.ComponentModel.DataAnnotations;

namespace Chess_API.Enums;

/// <summary>
/// A representation purpose for all game modes that can be played.
///
/// -> User VS User locally is equal to 0.
/// -> User VS AI = 1
/// -> User VS User online = 2
/// -> NotSet / Default = 3
/// </summary>
public enum PlayingMode
{
    [Display(Name = "User vs User")]
    UserVsUserLocal,
    [Display(Name = "User vs AI")]
    UserVsAi,
    [Display(Name = "User vs User (online)")]
    UserVsUserOnline,
    [Display(Name = "Not Set")]
    Default
}