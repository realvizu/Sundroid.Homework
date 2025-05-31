namespace Sundroid.Homework.Persistence.Entities;

/// <summary>
/// Entity class for a data logger.
/// </summary>
/// <remarks>
/// See the model mapping and the DB constraint specifications in <see cref="DataCollectorDbContext"/>.
/// </remarks>
public sealed class DataLogger
{
    /// <summary>
    /// Primary key. Value assigned by DB.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Unique serial number of the data logger.
    /// </summary>
    public required string SerialNumber { get; set; }
}