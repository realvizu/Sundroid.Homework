using Microsoft.EntityFrameworkCore;

namespace Sundroid.Homework.Persistence;

/// <summary>
/// Creates DataCollectorDbContext instances.
/// </summary>
public sealed class DataCollectorDbContextFactory(string connectionString) : IDbContextFactory<DataCollectorDbContext>
{
    public DataCollectorDbContext CreateDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataCollectorDbContext>().UseSqlServer(connectionString);
        return new DataCollectorDbContext(optionsBuilder.Options);
    }
}