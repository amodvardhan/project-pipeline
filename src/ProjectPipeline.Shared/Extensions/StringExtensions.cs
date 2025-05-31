using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace ProjectPipeline.Shared.Extensions;

/// <summary>
/// Extension methods for string operations
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Converts string to title case
    /// </summary>
    /// <param name="input">Input string</param>
    /// <returns>Title case string</returns>
    public static string ToTitleCase(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
    }

    /// <summary>
    /// Truncates string to specified length
    /// </summary>
    /// <param name="input">Input string</param>
    /// <param name="maxLength">Maximum length</param>
    /// <param name="suffix">Suffix to add if truncated</param>
    /// <returns>Truncated string</returns>
    public static string Truncate(this string input, int maxLength, string suffix = "...")
    {
        if (string.IsNullOrWhiteSpace(input) || input.Length <= maxLength)
            return input ?? string.Empty;

        return input.Substring(0, maxLength - suffix.Length) + suffix;
    }

    /// <summary>
    /// Removes special characters from string
    /// </summary>
    /// <param name="input">Input string</param>
    /// <returns>Clean string</returns>
    public static string RemoveSpecialCharacters(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        return Regex.Replace(input, @"[^a-zA-Z0-9\s]", "");
    }

    /// <summary>
    /// Converts string to slug format
    /// </summary>
    /// <param name="input">Input string</param>
    /// <returns>Slug string</returns>
    public static string ToSlug(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        // Convert to lowercase and replace spaces with hyphens
        var slug = input.ToLower()
                       .Replace(" ", "-")
                       .Replace("_", "-");

        // Remove special characters except hyphens
        slug = Regex.Replace(slug, @"[^a-z0-9\-]", "");

        // Remove multiple consecutive hyphens
        slug = Regex.Replace(slug, @"-+", "-");

        // Remove leading and trailing hyphens
        return slug.Trim('-');
    }

    /// <summary>
    /// Checks if string is null, empty, or whitespace
    /// </summary>
    /// <param name="input">Input string</param>
    /// <returns>True if null, empty, or whitespace</returns>
    public static bool IsNullOrWhiteSpace(this string input)
    {
        return string.IsNullOrWhiteSpace(input);
    }

    /// <summary>
    /// Masks sensitive information in string
    /// </summary>
    /// <param name="input">Input string</param>
    /// <param name="visibleChars">Number of visible characters at start and end</param>
    /// <param name="maskChar">Character to use for masking</param>
    /// <returns>Masked string</returns>
    public static string Mask(this string input, int visibleChars = 2, char maskChar = '*')
    {
        if (string.IsNullOrWhiteSpace(input) || input.Length <= visibleChars * 2)
            return new string(maskChar, input?.Length ?? 0);

        var start = input.Substring(0, visibleChars);
        var end = input.Substring(input.Length - visibleChars);
        var middle = new string(maskChar, input.Length - (visibleChars * 2));

        return start + middle + end;
    }
}
