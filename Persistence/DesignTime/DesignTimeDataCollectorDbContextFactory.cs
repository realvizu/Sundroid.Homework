using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Sundroid.Homework.Persistence.DesignTime;

/// <summary>
/// Creates a DbContext that can be used at design time, e.g. for generating migrations.
/// </summary>
/// <remarks>
/// See: https://learn.microsoft.com/en-us/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli#from-a-design-time-factory
/// </remarks>
public class DesignTimeDataCollectorDbContextFactory : IDesignTimeDbContextFactory<DataCollectorDbContext>
{
    // This connection is only for local (development) environment.
    private const string DesignTimeConnectionString = "Server=127.0.0.1;Database=datacollector;User ID=sa;Password=Password.123;TrustServerCertificate=True;";

    public DataCollectorDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataCollectorDbContext>();
        optionsBuilder.UseSqlServer(DesignTimeConnectionString);

        return new DataCollectorDbContext(optionsBuilder.Options);
    }
}