namespace Sundroid.Homework.DataLoader.Parsing;

public sealed class ParsedInverterLine()
    : ParsedLineBase(ParsedLineType.Inverter)
{
    public required string Name { get; init; }
    public required string SerialNumber { get; init; }
}