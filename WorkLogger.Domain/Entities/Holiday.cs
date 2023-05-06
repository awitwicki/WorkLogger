namespace WorkLogger.Domain.Entities;

public class Holiday
{
    public string Name { get; set; } = string.Empty;
    public DateTimeOffset DateDay { get; set; }

    public Holiday()
    {
    }
    
    public Holiday(string name, DateTimeOffset dateDay)
    {
        Name = name;
        DateDay = dateDay;
    }
}
