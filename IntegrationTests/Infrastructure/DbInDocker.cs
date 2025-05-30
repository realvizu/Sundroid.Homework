using Microsoft.Data.SqlClient;
using Testcontainers.MsSql;
using Xunit;

namespace Sundroid.Homework.IntegrationTests.Infrastructure;

/// <summary>
/// Abstract base class for a DB in a docker container.
/// </summary>
/// <remarks>
/// A new MSSQL container is created for every test, with a clean DB. 
/// Therefore, there's no need to clean up data in DB tables.
/// </remarks>
public abstract class DbInDocker : IAsyncLifetime
{
    private const string DbName = "testdb";

    private readonly MsSqlContainer _msSqlContainer;

    public string ConnectionString { get; private set; } = null!;

    protected DbInDocker()
    {
        var msSqlBuilder = new MsSqlBuilder();
        _msSqlContainer = msSqlBuilder.Build();
    }

    public virtual async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();

        ConnectionString = await CreateTestDbAsync();
    }


    public virtual async Task DisposeAsync()
    {
        await _msSqlContainer.DisposeAsync();
    }

    /// <summary>
    /// Creates a test DB inside the MSSQL in docker,
    /// and returns a connection string pointing to that DB.
    /// </summary>
    private async Task<string> CreateTestDbAsync()
    {
        var masterConnectionString = _msSqlContainer.GetConnectionString();

        using (var masterConnection = new SqlConnection(masterConnectionString))
        {
            await masterConnection.OpenAsync();

            using (var createCmd = masterConnection.CreateCommand())
            {
                createCmd.CommandText = $"CREATE DATABASE [{DbName}]";
                await createCmd.ExecuteNonQueryAsync();
            }
        }

        return new SqlConnectionStringBuilder(masterConnectionString) { InitialCatalog = DbName }.ToString();
    }
}