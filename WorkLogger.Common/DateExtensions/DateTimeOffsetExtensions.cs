namespace WorkLogger.Common.DateExtensions;

public static class DateTimeOffsetExtensions
{
    public static DateOnly ToDateOnly(this DateTimeOffset date)
    {
        return new DateOnly(date.Year, date.Month, date.Day);
    }
}
