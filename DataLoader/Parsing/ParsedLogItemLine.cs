using Sundroid.Homework.Persistence.Entities;

namespace Sundroid.Homework.DataLoader.Parsing;

/// <summary>
/// The result object for the input file lines that specify a timestamped value set.
/// </summary>
public sealed class ParsedLogItemLine()
    : ParsedLineBase(ParsedLineType.LogItem)
{
    public DateTimeOffset Time { get; set; }
    public ValueSet Values { get; set; } = new();
    public int CycleTime { get; set; }
}