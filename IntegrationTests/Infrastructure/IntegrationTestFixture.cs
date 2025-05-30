using Respawn;
using Xunit;

namespace Sundroid.Homework.IntegrationTests.Infrastructure;

/// <summary>
/// Creates a DataCollector DB in docker and initializes Respawner,
/// so that the DB can be cleaned before after test method by calling ResetDatabaseAsync.
/// </summary>
/// <remarks>
/// This test fixture is instantiated by Xunit, one instance per test class.
/// It helps to speed up tests by creating only 1 docker container per test class (not for each method).
/// </remarks>
public sealed class IntegrationTestFixture : IAsyncLifetime
{
    private Respawner _respawner = null!;

    public DataCollectorDbInDocker DbInDocker { get; set; } = null!;

    public async Task InitializeAsync()
    {
        await ParallelTestHelper.ConcurrentDockerContainersSemaphore.WaitAsync();

        DbInDocker = new();
        await DbInDocker.InitializeAsync();

        _respawner = await Respawner.CreateAsync(DbInDocker.ConnectionString, new RespawnerOptions
        {
            DbAdapter = DbAdapter.SqlServer,
        });
    }

    public async Task DisposeAsync()
    {
        await DbInDocker.DisposeAsync();

        ParallelTestHelper.ConcurrentDockerContainersSemaphore.Release();
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(DbInDocker.ConnectionString);
    }
}