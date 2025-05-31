namespace Sundroid.Homework.DataLoader.Parsing;

/// <summary>
/// The result object for the input file lines that specifies an inverter.
/// </summary>
public sealed class ParsedInverterLine()
    : ParsedLineBase(ParsedLineType.Inverter)
{
    public required string Name { get; init; }
    public required string SerialNumber { get; init; }
}