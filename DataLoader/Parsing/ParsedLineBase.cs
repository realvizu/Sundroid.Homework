namespace Sundroid.Homework.DataLoader.Parsing;

public abstract class ParsedLineBase(ParsedLineType type)
{
    public ParsedLineType Type { get; } = type;
}