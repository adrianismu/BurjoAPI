namespace Burjo.Core.Utilities;

public static class StringHelper
{
    /// <summary>
    /// Parse comma-separated values into a list of trimmed, lowercase strings
    /// </summary>
    /// <param name="input">Comma-separated string</param>
    /// <returns>List of processed strings</returns>
    public static List<string> ParseCommaSeparatedValues(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return new List<string>();

        return input
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(item => item.Trim().ToLowerInvariant())
            .ToList();
    }
}
