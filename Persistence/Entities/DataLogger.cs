namespace Sundroid.Homework.Persistence.Entities;

/// <summary>
/// Entity class for a data logger.
/// </summary>
public sealed class DataLogger
{
    /// <summary>
    /// Primary key. Value assigned by DB.
    /// </summary>
    public int Id { get; set; }

    public required string SerialNumber { get; set; }
}