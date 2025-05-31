namespace Sundroid.Homework.DataLoader.Parsing;

/// <summary>
/// The result object for the "SmartLogger" line of the input file.
/// </summary>
public sealed class ParsedDataLoggerLine()
    : ParsedLineBase(ParsedLineType.DataLogger)
{
    public required string SerialNumber { get; set; }
}