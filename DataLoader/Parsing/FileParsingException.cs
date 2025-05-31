namespace Sundroid.Homework.DataLoader.Parsing;

/// <summary>
/// Thrown for file parsing errors.
/// </summary>
public sealed class FileParsingException(string message) : Exception(message);