namespace Sundroid.Homework.DataLoader.Parsing;

/// <summary>
/// Implements the logic of parsing an input file.
/// Returns an async enumerable if parser result objects.
/// </summary>
public static class FileParser
{
    private const string DataLoggerLineStartToken = "#SmartLogger";
    private const string InverterLineStartToken = "#INV";
    private const string LogItemHeaderStartToken = "#Time";

    /// <summary>
    /// Returns an async collection of parser result objects, one for each line of the input file.
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