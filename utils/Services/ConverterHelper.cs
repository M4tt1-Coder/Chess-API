namespace Chess_API.utils.Services;

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
    public static List<int> ConvertStringToIntsList(string input)
    {
        return input.Split(',').Select(int.Parse).ToList();
    }
    
    /// <summary>
    /// Creates a string out of a list of integers.
    /// </summary>
    /// <param name="input">List of integers that should be included in one string.</param>
    /// <returns>String with integer values</returns>
    public static string ConvertIntsListToString(List<int> input)
    {
        Console.WriteLine("Input: " + input);
        return string.Join(",", input);
    }

    /// <summary>
    /// Adds different lists of lists together.
    /// </summary>
    /// <param name="data">List that need to be summarized</param>
    /// <typeparam name="T">Independent type of the list elements</typeparam>
    /// <returns>Merged list of list</returns>
    public static List<List<T>> JoinListsOfListsTogether<T>(List<List<List<T>>> data)
    {
        var output = new List<List<T>>();

        foreach (var list in data)
        {
            output.AddRange(list);
        }

        return output;
    }
}