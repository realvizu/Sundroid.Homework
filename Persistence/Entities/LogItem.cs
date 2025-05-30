namespace Sundroid.Homework.Persistence.Entities;

/// <summary>
/// Entity class for a log item that belongs to an inverter.
/// </summary>
public sealed class LogItem
{
    /// <summary>
    /// Primary key. Value assigned by DB.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Foreign key to the inverter that this log item belongs to.
    /// </summary>
    public int InverterId { get; set; }

    /// <summary>
    /// (InverterId + Time) should be unique.
    /// </summary>
    public DateTimeOffset Time { get; set; }

    public required ValueSet Values { get; set; }

    public int CycleTime { get; set; }

    public DateTimeOffset UpdatedAtUtc { get; set; }
}