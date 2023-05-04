namespace WorkLogger.Common.DateExtensions;

public static class DateOnlyExtensions
{
    public static DateTimeOffset ToDateTimeOffset(this DateOnly dateOnly)
    {
        return new DateTimeOffset(dateOnly.ToDateTime(TimeOnly.MinValue), TimeSpan.Zero);
    }
}
