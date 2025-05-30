using Microsoft.EntityFrameworkCore;

namespace Sundroid.Homework.Persistence;

public sealed class DataCollectorDbContextFactory(string connectionString) : IDbContextFactory<DataCollectorDbContext>
{
    public DataCollectorDbContext CreateDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataCollectorDbContext>().UseSqlServer(connectionString);
        return new DataCollectorDbContext(optionsBuilder.Options);
    }
}