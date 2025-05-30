using Microsoft.EntityFrameworkCore;
using Sundroid.Homework.Persistence;

namespace Sundroid.Homework.IntegrationTests.Infrastructure;

public sealed class DataCollectorDbInDocker : DbInDocker
{
    public IDbContextFactory<DataCollectorDbContext> DbContextFactory { get; private set; } = null!;

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        DbContextFactory = new DataCollectorDbContextFactory(ConnectionString);

        await using (var dbContext = await DbContextFactory.CreateDbContextAsync())
        {
            await dbContext.Database.MigrateAsync();
        }
    }
}