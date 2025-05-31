using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ProjectPipeline.Shared.Helpers;

/// <summary>
/// Helper class for common validation operations
/// </summary>
public static class ValidationHelper
{
    /// <summary>
    /// Validates an email address
    /// </summary>
    /// <param name="email">Email to validate</param>
    /// <returns>True if valid email</returns>
    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            var emailAttribute = new EmailAddressAttribute();
            return emailAttribute.IsValid(email);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Validates a phone number
    /// </summary>
    /// <param name="phoneNumber">Phone number to validate</param>
    /// <returns>True if valid phone number</returns>
    public static bool IsValidPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return false;

        // Basic phone number validation (supports various formats)
        var phoneRegex = new Regex(@"^[\+]?[1-9][\d]{0,15}$");
        return phoneRegex.IsMatch(phoneNumber.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", ""));
    }

    /// <summary>
    /// Validates password strength
    /// </summary>
    /// <param name="password">Password to validate</param>
    /// <returns>Validation result with errors</returns>
    public static ValidationResult ValidatePassword(string password)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(password))
        {
            errors.Add("Password is required");
            return new ValidationResult(false, errors);
        }

        if (password.Length < 6)
            errors.Add("Password must be at least 6 characters long");

        if (!password.Any(char.IsUpper))
            errors.Add("Password must contain at least one uppercase letter");

        if (!password.Any(char.IsLower))
            errors.Add("Password must contain at least one lowercase letter");

        if (!password.Any(char.IsDigit))
            errors.Add("Password must contain at least one digit");

        return new ValidationResult(errors.Count == 0, errors);
    }

    /// <summary>
    /// Validates a business unit code
    /// </summary>
    /// <param name="code">Code to validate</param>
    /// <returns>True if valid code</returns>
    public static bool IsValidBusinessUnitCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return false;

        // Code should be 2-10 characters, alphanumeric only
        var codeRegex = new Regex(@"^[A-Za-z0-9]{2,10}$");
        return codeRegex.IsMatch(code);
    }

    /// <summary>
    /// Sanitizes input string
    /// </summary>
    /// <param name="input">Input to sanitize</param>
    /// <returns>Sanitized string</returns>
    public static string SanitizeInput(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        // Remove potentially dangerous characters
        return input.Trim()
                   .Replace("<", "&lt;")
                   .Replace(">", "&gt;")
                   .Replace("\"", "&quot;")
                   .Replace("'", "&#x27;")
                   .Replace("/", "&#x2F;");
    }
}

/// <summary>
/// Validation result class
/// </summary>
public class ValidationResult
{
    public bool IsValid { get; }
    public IEnumerable<string> Errors { get; }

    public ValidationResult(bool isValid, IEnumerable<string> errors)
    {
        IsValid = isValid;
        Errors = errors ?? Enumerable.Empty<string>();
    }
}
