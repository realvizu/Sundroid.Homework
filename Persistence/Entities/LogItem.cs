namespace Sundroid.Homework.Persistence.Entities;

/// <summary>
/// Entity class for a log item that belongs to an inverter.
/// </summary>
/// <remarks>
/// See the model mapping and the DB constraint specifications in <see cref="DataCollectorDbContext"/>.
/// </remarks>
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
    /// The UTC timestamp of the value set.
    /// </summary>
    /// <remarks>
    /// InverterId + Time must be unique.
    /// </remarks>
    public DateTimeOffset Time { get; set; }

    /// <summary>
    /// Contains the logged values imported from the input files.
    /// </summary>
    public required ValueSet Values { get; set; }

    public int CycleTime { get; set; }

    /// <summary>
    /// UTC timestamp of the last update of this record.
    /// </summary>
    public DateTimeOffset UpdatedAtUtc { get; set; }
}