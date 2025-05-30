using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Sundroid.Homework.Persistence.Entities;

namespace Sundroid.Homework.Persistence;

/// <summary>
/// Implements DB operations for data collector entities.
/// </summary>
public sealed class DataCollectorRepository(
    IDbContextFactory<DataCollectorDbContext> dbContextFactory,
    TimeProvider timeProvider)
{
    private static readonly BulkConfig LogItemBulkUpsertConfig = new()
    {
        UpdateByProperties = [nameof(LogItem.InverterId), nameof(LogItem.Time)],
    };

    /// <summary>
    /// Inserts a data logger entity if not yet exists by serial number.
    /// Returns its ID.
    /// </summary>
    /// <remarks>
    /// At the moment there's nothing to update in the entity when it already exists,
    /// but we still keep the Upsert operation for consistency and for easier extensibility later.
    /// </remarks>
    public async Task<int> UpsertDataLoggerAsync(DataLogger dataLogger)
    {
        await using (var dbContext = await dbContextFactory.CreateDbContextAsync())
        {
            var existingDataLogger = await dbContext.DataLoggers.FirstOrDefaultAsync(i => i.SerialNumber == dataLogger.SerialNumber);
            if (existingDataLogger is null)
            {
                await dbContext.AddAsync(dataLogger);
                await dbContext.SaveChangesAsync();
                return dataLogger.Id;
            }

            return existingDataLogger.Id;
        }
    }

    public async Task<DataLogger?> GetDataLoggerByIdAsync(int id)
    {
        await using (var dbContext = await dbContextFactory.CreateDbContextAsync())
        {
            return await dbContext.DataLoggers.FirstOrDefaultAsync(i => i.Id == id);
        }
    }

    public async Task<DataLogger?> GetDataLoggerBySerialNumberAsync(string serialNumber)
    {
        await using (var dbContext = await dbContextFactory.CreateDbContextAsync())
        {
            return await dbContext.DataLoggers.FirstOrDefaultAsync(i => i.SerialNumber == serialNumber);
        }
    }

    /// <summary>
    /// Inserts an inverter entity if not yet exists by serial number, or updates it if already exists.
    /// Returns its ID.
    /// </summary>
    public async Task<int> UpsertInverterAsync(Inverter inverter)
    {
        await using (var dbContext = await dbContextFactory.CreateDbContextAsync())
        {
            var existingInverter = await dbContext.Inverters.FirstOrDefaultAsync(i =>
                i.SerialNumber == inverter.SerialNumber &&
                i.DataLoggerId == inverter.DataLoggerId);

            if (existingInverter is null)
            {
                await dbContext.AddAsync(inverter);
                await dbContext.SaveChangesAsync();
                return inverter.Id;
            }

            inverter.Id = existingInverter.Id;
            dbContext.Entry(existingInverter).CurrentValues.SetValues(inverter);
            await dbContext.SaveChangesAsync();

            return existingInverter.Id;
        }
    }

    public async Task<Inverter?> GetInverterByIdAsync(int id)
    {
        await using (var dbContext = await dbContextFactory.CreateDbContextAsync())
        {
            return await dbContext.Inverters.FirstOrDefaultAsync(i => i.Id == id);
        }
    }

    public async Task<Inverter?> GetInverterBySerialNumberAsync(string serialNumber)
    {
        await using (var dbContext = await dbContextFactory.CreateDbContextAsync())
        {
            return await dbContext.Inverters.FirstOrDefaultAsync(i => i.SerialNumber == serialNumber);
        }
    }

    /// <summary>
    /// Inserts or updates the given data entry collection by inverter ID and Time.
    /// </summary>
    public async Task BulkUpsertLogItemsAsync(IReadOnlyCollection<LogItem> logItems)
    {
        SetUpdatedTimestamp(logItems);

        await using (var dbContext = await dbContextFactory.CreateDbContextAsync())
        {
            await dbContext.BulkInsertOrUpdateAsync(logItems, LogItemBulkUpsertConfig);
        }
    }

    public async Task<IReadOnlyCollection<LogItem>> GetLogItemsByInverterIdAsync(
        int inverterId,
        DateTimeOffset? fromTime = null,
        DateTimeOffset? toTime = null)
    {
        await using (var dbContext = await dbContextFactory.CreateDbContextAsync())
        {
            var query = dbContext.LogItems.Where(i => i.InverterId == inverterId);

            if (fromTime != null)
                query = query.Where(i => i.Time >= fromTime);

            if (toTime != null)
                query = query.Where(i => i.Time <= toTime);

            return await query.ToListAsync();
        }
    }

    private void SetUpdatedTimestamp(IReadOnlyCollection<LogItem> logItems)
    {
        foreach (var logItem in logItems)
            logItem.UpdatedAtUtc = timeProvider.GetUtcNow();
    }
}