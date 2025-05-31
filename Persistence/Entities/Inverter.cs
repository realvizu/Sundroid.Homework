namespace Sundroid.Homework.Persistence.Entities;

/// <summary>
/// Entity class for an inverter.
/// </summary>
/// <remarks>
/// See the model mapping and the DB constraint specifications in <see cref="DataCollectorDbContext"/>.
/// </remarks>
public sealed class Inverter
{
    /// <summary>
    /// Primary key. Value assigned by DB.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The name of the inverter as it appears in the input files.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Unique serial number of the inverter.
    /// </summary>
    public required string SerialNumber { get; set; }

    /// <summary>
    /// Foreign key to the logger that this inverter belongs to.
    /// </summary>
    public int DataLoggerId { get; set; }
}