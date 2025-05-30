namespace Sundroid.Homework.DataLoader.Parsing;

public sealed class ParsedDataLoggerLine()
    : ParsedLineBase(ParsedLineType.DataLogger)
{
    public required string SerialNumber { get; set; }
}