namespace Chess_API.utils;

/// <summary>
/// Contains helper functions to convert different datatypes to other necessary types.
/// </summary>
public static class ConverterHelper
{
    /// <summary>
    /// Uses a string to create a list of integers.
    ///
    /// Separator is a comma.
    /// </summary>
    /// <param name="input">Integer values combined in a string!</param>
    /// <returns>List of integers</returns>
    public static IList<int> ConvertStringToIntsList(string input)
    {
        return input.Split(',').Select(int.Parse).ToList();
    }
    
    /// <summary>
    /// Creates a string out of a list of integers.
    /// </summary>
    /// <param name="input">List of integers that should be included in one string.</param>
    /// <returns>String with integer values</returns>
    public static string ConvertIntsListToString(IList<int> input)
    {
        Console.WriteLine("Input: " + input);
        return string.Join(",", input);
    }
}