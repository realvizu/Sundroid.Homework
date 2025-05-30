namespace Sundroid.Homework.DataLoader.Parsing;

public static class FileParser
{
    private const string DataLoggerLineStartToken = "#SmartLogger";
    private const string InverterLineStartToken = "#INV";
    private const string LogItemHeaderStartToken = "#Time";

    /// <summary>
    /// Returns parsed lines one-by-one from the given filename.
    /// Throws <see cref="FileParsingException"/> for any parsing errors.
    /// </summary>
    public static async IAsyncEnumerable<ParsedLineBase> GetParsedLinesAsync(string filename)
    {
        await foreach (var line in File.ReadLinesAsync(filename))
        {
            var parsedLine = ParseLine(line);
            if (parsedLine != null)
                yield return parsedLine;
        }
    }

    private static ParsedLineBase? ParseLine(string line)
    {
        if (line.StartsWith(DataLoggerLineStartToken))
            return FileLineParser.ParseDataLoggerLine(line);

        if (line.StartsWith(InverterLineStartToken))
            return FileLineParser.ParseInverterLine(line);

        if (line.StartsWith(LogItemHeaderStartToken))
        {
            // Skip the header
            return null;
        }
        
        // Assuming that this line is a log item
        return FileLineParser.ParseLogItemLine(line);
    }
}