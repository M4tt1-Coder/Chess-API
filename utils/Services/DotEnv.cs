namespace Chess_API.utils.Services;

/// <summary>
/// Loads all static environment variables from .env into the .NET environment.
/// 
/// Source: https://dusted.codes/dotenv-in-dotnet
/// </summary>
public static class DotEnv
{
    /// <summary>
    /// Sets environment variables for the whole app.
    ///
    /// Reads the content of the .env-file.
    ///
    /// Retrieves the key-value-pair and sets it in the .NET environment.
    /// </summary>
    /// <param name="filePath">Where the .env file is situated.</param>
    public static void Load(string filePath)
    {
        if (!File.Exists(filePath))
            return;

        foreach (var line in File.ReadAllLines(filePath))
        {
            var parts = line.Split(
                '=',
                StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
                continue;

            if (parts[1][1] is '"' || parts[1][parts[1].Length - 1] is '"')
            {
                parts[1] = parts[1].Substring(1, parts[1].Length - 2);
            }

            Environment.SetEnvironmentVariable(parts[0], parts[1]);
        }
    }
}