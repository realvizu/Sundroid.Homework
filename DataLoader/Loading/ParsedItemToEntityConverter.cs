using Sundroid.Homework.DataLoader.Parsing;
using Sundroid.Homework.Persistence.Entities;

namespace Sundroid.Homework.DataLoader.Loading;

/// <summary>
/// Converts parser result objects to DB entities.
/// </summary>
public static class ParsedItemToEntityConverter
{
    public static DataLogger ToEntity(this ParsedDataLoggerLine parsedData)
    {
        return new DataLogger
        {
            SerialNumber = parsedData.SerialNumber
        };
    }

    public static Inverter ToEntity(this ParsedInverterLine parsedData, int dataLoggerId)
    {
        return new Inverter
        {
            DataLoggerId = dataLoggerId,
            Name = parsedData.Name,
            SerialNumber = parsedData.SerialNumber
        };
    }

    public static LogItem ToEntity(this ParsedLogItemLine parsedData, int inverterId)
    {
        return new LogItem
        {
            InverterId = inverterId,
            Time = parsedData.Time,
            Values = parsedData.Values,
            CycleTime = parsedData.CycleTime
        };
    }
}