using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace Sundroid.Homework.DataLoader.Parsing;

public static class FileLineParser
{
    private const string SerialNumberPrefix = "ESN:";
    private const string InverterNamePrefix = "#INV";

    public static ParsedDataLoggerLine ParseDataLoggerLine(string line)
    {
        var serialNumber = ExtractSerialNumber(line);
        return new ParsedDataLoggerLine { SerialNumber = serialNumber };
    }

    public static ParsedInverterLine ParseInverterLine(string line)
    {
        var inverterName = ExtractInverterName(line);
        var serialNumber = ExtractSerialNumber(line);
        return new ParsedInverterLine { Name = inverterName, SerialNumber = serialNumber };
    }

    public static ParsedLogItemLine ParseLogItemLine(string line)
    {
        var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
            ShouldQuote = i => false,
            Delimiter = ";",
            NewLine = "\n",
        };

        using var reader = new StringReader(line);
        using var csvReader = new CsvReader(reader, csvConfiguration);
        csvReader.Context.RegisterClassMap(typeof(ParsedLogItemLineClassMap));

        var records = csvReader.GetRecords<ParsedLogItemLine>().ToList();
        return records[0];
    }

    public static string ExtractInverterName(string line)
    {
        var start = line.IndexOf(InverterNamePrefix, StringComparison.Ordinal);
        var end = line.IndexOf(' ', StringComparison.Ordinal);

        if (start < 0 || end < 0)
            throw new FileParsingException($"Could not parse InverterName from line: '{line}'");

        return line.Substring(start + 1, end - start - 1).Trim();
    }

    public static string ExtractSerialNumber(string line)
    {
        var start = line.IndexOf(SerialNumberPrefix, StringComparison.Ordinal);

        if (start < 0)
            throw new FileParsingException($"Could not parse SerialNumber from line: '{line}'");

        return line.Substring(start + SerialNumberPrefix.Length).Trim();
    }
}