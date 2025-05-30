using Sundroid.Homework.Persistence.Entities;

namespace Sundroid.Homework.DataLoader.Parsing;

public sealed class ParsedLogItemLine()
    : ParsedLineBase(ParsedLineType.LogItem)
{
    public DateTimeOffset Time { get; set; }
    public ValueSet Values { get; set; } = new();
    public int CycleTime { get; set; }
}