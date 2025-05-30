namespace Sundroid.Homework.Persistence.Entities;

/// <summary>
/// Entity class for an inverter.
/// </summary>
public sealed class Inverter
{
    /// <summary>
    /// Primary key. Value assigned by DB.
    /// </summary>
    public int Id { get; set; }

    public required string Name { get; set; }
    public required string SerialNumber { get; set; }

    /// <summary>
    /// Foreign key to the logger that this inverter belongs to.
    /// </summary>
    public int DataLoggerId { get; set; }
}