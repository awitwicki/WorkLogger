namespace WorkLogger.Domain.Entities;

public class Holiday
{
    public string Name { get; set; } = string.Empty;
    public DateTimeOffset DateDay { get; set; }
}
