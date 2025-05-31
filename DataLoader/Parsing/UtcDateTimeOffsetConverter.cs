using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace Sundroid.Homework.DataLoader.Parsing;

/// <summary>
/// Converts the input file's timestamp string representation into a UTC DateTimeOffset.
/// Used by the CsvHelper tool. See: <see cref="ParsedLogItemLineClassMap"/>
/// </summary>
public sealed class UtcDateTimeOffsetConverter : ITypeConverter
{
    public object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        return text == null
            ? null
            : DateTimeOffset.ParseExact(text, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
    }

    public string? ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
    {
        // No need to implement it, because we are only reading CSV.
        throw new NotImplementedException();
    }
}