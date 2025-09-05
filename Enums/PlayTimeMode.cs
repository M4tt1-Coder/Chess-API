using System.ComponentModel.DataAnnotations;

namespace Chess_API.Enums;

/// <summary>
/// Enumeration of all time modes.
///
/// 'None' is when the user doesn't want to have a time limit or wants to customize it.
///
/// Represents all specific game options.
///
/// The time in seconds are applied to the enums members.
/// </summary>
public enum PlayTimeMode
{
    [Display(Name = "No time limit")]
    NoTimeLimit = 0,
    [Display(Name = "None / Customized")]
    Custom = 1,
    [Display(Name = "3 min")]
    ThreeMinutes = 180,
    [Display(Name = "10 min")]
    TenMinutes = 600,
    [Display(Name = "30 min")]
    ThirtyMinutes = 1800,
}