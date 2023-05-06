using WorkLogger.Common.DateExtensions;
using WorkLogger.Domain.Entities;

namespace WorkLogger.Common;

public static class HolidaysHelpers
{
    public static IEnumerable<Holiday> GetHolidays2023()
    {
        if (DateTime.Now.Year != 2023)
        {
            throw new InvalidOperationException(
                "This method is only valid for 2023, please Update method HolidaysHelpers.GetHolidays2023()");
        }

        var polishHolidays = new Dictionary<string, DateOnly>()
        {
            { "Nowy Rok" , new DateOnly(2023, 1, 1)},
            { "Święto Trzech Króli" , new DateOnly(2023, 1, 6)},
            { "pierwszy dzień Wielkiej Nocy" , new DateOnly(2023, 4, 9)},
            { "drugi dzień Wielkiej Nocy" , new DateOnly(2023, 4, 10)},
            { "Święto Państwowe" , new DateOnly(2023, 5, 1)},
            { "Święto Narodowe Trzeciego Maja" , new DateOnly(2023, 5, 3)},
            { "Zielone Świątki" , new DateOnly(2023, 5, 28)},
            { "Boże Ciało" , new DateOnly(2023, 6, 8)},
            { "Wniebowzięcie Najświętszej Maryi Panny" , new DateOnly(2023, 8, 15)},
            { "Wszystkich Świętych" , new DateOnly(2023, 11, 1)},
            { "Narodowe Święto Niepodległości" , new DateOnly(2023, 11, 11)},
            { "pierwszy dzień Bożego Narodzenia" , new DateOnly(2023, 12, 25)},
            { "drugi dzień Bożego Narodzenia" , new DateOnly(2023, 12, 26)},
        };

        return polishHolidays.Select(x => 
            new Holiday(x.Key, x.Value.ToDateTimeOffset()));
    }
}
