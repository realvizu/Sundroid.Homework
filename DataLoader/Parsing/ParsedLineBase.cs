namespace Sundroid.Homework.DataLoader.Parsing;

/// <summary>
/// Abstract base class for the parser result objects.
/// </summary>
public abstract class ParsedLineBase(ParsedLineType type)
{
    public ParsedLineType Type { get; } = type;
}