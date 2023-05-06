namespace WorkLogger.Common.DateExtensions;

public static class DateTimeOffsetExtensions
{
    public static DateOnly ToDateOnly(this DateTimeOffset date)
    {
        return new DateOnly(date.Year, date.Month, date.Day);
    }
    
    public static DateTimeOffset TruncateToMonth(this DateTimeOffset date)
    {
        return new DateTimeOffset(date.Year, date.Month, 1, 0, 0, 0, TimeSpan.Zero);
    }
}
