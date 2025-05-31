namespace ProjectPipeline.Shared.Extensions;

/// <summary>
/// Extension methods for DateTime operations
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// Gets the start of the day (00:00:00)
    /// </summary>
    /// <param name="date">Date</param>
    /// <returns>Start of day</returns>
    public static DateTime StartOfDay(this DateTime date)
    {
        return date.Date;
    }

    /// <summary>
    /// Gets the end of the day (23:59:59.999)
    /// </summary>
    /// <param name="date">Date</param>
    /// <returns>End of day</returns>
    public static DateTime EndOfDay(this DateTime date)
    {
        return date.Date.AddDays(1).AddTicks(-1);
    }

    /// <summary>
    /// Gets the start of the week (Monday)
    /// </summary>
    /// <param name="date">Date</param>
    /// <returns>Start of week</returns>
    public static DateTime StartOfWeek(this DateTime date)
    {
        var diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
        return date.AddDays(-1 * diff).Date;
    }

    /// <summary>
    /// Gets the start of the month
    /// </summary>
    /// <param name="date">Date</param>
    /// <returns>Start of month</returns>
    public static DateTime StartOfMonth(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, 1);
    }

    /// <summary>
    /// Gets the end of the month
    /// </summary>
    /// <param name="date">Date</param>
    /// <returns>End of month</returns>
    public static DateTime EndOfMonth(this DateTime date)
    {
        return date.StartOfMonth().AddMonths(1).AddDays(-1);
    }

    /// <summary>
    /// Calculates age from birth date
    /// </summary>
    /// <param name="birthDate">Birth date</param>
    /// <returns>Age in years</returns>
    public static int CalculateAge(this DateTime birthDate)
    {
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;

        if (birthDate.Date > today.AddYears(-age))
            age--;

        return age;
    }

    /// <summary>
    /// Checks if date is weekend
    /// </summary>
    /// <param name="date">Date to check</param>
    /// <returns>True if weekend</returns>
    public static bool IsWeekend(this DateTime date)
    {
        return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
    }

    /// <summary>
    /// Gets friendly time ago string
    /// </summary>
    /// <param name="date">Date</param>
    /// <returns>Friendly string like "2 hours ago"</returns>
    public static string ToTimeAgoString(this DateTime date)
    {
        var timeSpan = DateTime.UtcNow - date;

        if (timeSpan.TotalDays > 365)
            return $"{(int)(timeSpan.TotalDays / 365)} year{((int)(timeSpan.TotalDays / 365) == 1 ? "" : "s")} ago";

        if (timeSpan.TotalDays > 30)
            return $"{(int)(timeSpan.TotalDays / 30)} month{((int)(timeSpan.TotalDays / 30) == 1 ? "" : "s")} ago";

        if (timeSpan.TotalDays > 1)
            return $"{(int)timeSpan.TotalDays} day{((int)timeSpan.TotalDays == 1 ? "" : "s")} ago";

        if (timeSpan.TotalHours > 1)
            return $"{(int)timeSpan.TotalHours} hour{((int)timeSpan.TotalHours == 1 ? "" : "s")} ago";

        if (timeSpan.TotalMinutes > 1)
            return $"{(int)timeSpan.TotalMinutes} minute{((int)timeSpan.TotalMinutes == 1 ? "" : "s")} ago";

        return "Just now";
    }
}
