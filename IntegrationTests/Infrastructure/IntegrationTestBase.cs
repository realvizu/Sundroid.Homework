using Microsoft.Extensions.Time.Testing;
using Sundroid.Homework.Persistence;
using Xunit;

namespace Sundroid.Homework.IntegrationTests.Infrastructure;

public abstract class IntegrationTestBase(IntegrationTestFixture testFixture)
    : IClassFixture<IntegrationTestFixture>, IAsyncLifetime
{
    protected FakeTimeProvider FakeTimeProvider { get; } = new();
    protected IntegrationTestFixture TestFixture { get; } = testFixture;

    protected DataCollectorRepository CreateRepository()
    {
        var dbContextFactory = new DataCollectorDbContextFactory(TestFixture.DbInDocker.ConnectionString);
        return new DataCollectorRepository(dbContextFactory, FakeTimeProvider);
    }

    public virtual async Task InitializeAsync()
    {
        await TestFixture.ResetDatabaseAsync();
    }

    public virtual Task DisposeAsync() => Task.CompletedTask;
}