using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Sundroid.Homework.IntegrationTests.Infrastructure;
using Sundroid.Homework.Persistence;
using Sundroid.Homework.Persistence.Entities;
using Xunit;

namespace Sundroid.Homework.IntegrationTests.Persistence;

public sealed class DataCollectorRepositoryTests(IntegrationTestFixture testFixture)
    : IntegrationTestBase(testFixture)
{
    private static readonly DateTimeOffset TestTime = new(2024, 5, 30, 13, 14, 15, TimeSpan.Zero);

    [Fact]
    public async Task UpsertDataLoggerAsync_EntityNotFound_ShouldAddEntity()
    {
        var repository = CreateRepository();

        var dataLogger = new DataLogger { SerialNumber = "123" };
        var dataLoggerId = await repository.UpsertDataLoggerAsync(dataLogger);
        dataLoggerId.Should().BeGreaterThan(0);

        var savedDataLogger = await repository.GetDataLoggerByIdAsync(dataLoggerId);
        savedDataLogger.Should().BeEquivalentTo(dataLogger);
    }

    [Fact]
    public async Task UpsertDataLoggerAsync_EntityAlreadyExists_ShouldDoNothing()
    {
        var repository = CreateRepository();

        var dataLogger1 = new DataLogger { SerialNumber = "123" };
        var dataLoggerId1 = await repository.UpsertDataLoggerAsync(dataLogger1);

        var dataLogger2 = new DataLogger { SerialNumber = "123" };
        var dataLoggerId2 = await repository.UpsertDataLoggerAsync(dataLogger2);
        dataLoggerId2.Should().Be(dataLoggerId1);

        var savedDataLogger = await repository.GetDataLoggerByIdAsync(dataLoggerId1);
        savedDataLogger.Should().BeEquivalentTo(dataLogger1);
    }

    [Fact]
    public async Task UpsertInverterAsync_EntityNotFound_ShouldAddEntity()
    {
        var repository = CreateRepository();

        var dataLoggerId = await CreateDataLoggerInDbAsync(repository);

        var inverter = new Inverter { DataLoggerId = dataLoggerId, SerialNumber = "456", Name = "INV1 " };
        var inverterId = await repository.UpsertInverterAsync(inverter);
        inverterId.Should().BeGreaterThan(0);

        var savedInverter = await repository.GetInverterByIdAsync(inverterId);
        savedInverter.Should().BeEquivalentTo(inverter);
    }

    [Fact]
    public async Task UpsertInverterAsync_EntityAlreadyExists_ShouldUpdateIt()
    {
        var repository = CreateRepository();

        var dataLoggerId = await CreateDataLoggerInDbAsync(repository);

        var inverter1 = new Inverter { DataLoggerId = dataLoggerId, SerialNumber = "456", Name = "INV1 " };
        var inverterId1 = await repository.UpsertInverterAsync(inverter1);

        var inverter2 = new Inverter { DataLoggerId = dataLoggerId, SerialNumber = "456", Name = "INV1_MOD " };
        var inverterId2 = await repository.UpsertInverterAsync(inverter2);
        inverterId2.Should().Be(inverterId1);

        var savedInverter = await repository.GetInverterByIdAsync(inverterId1);
        savedInverter.Should().BeEquivalentTo(inverter2);
    }

    [Fact]
    public async Task UpsertInverterAsync_DataLoggerIdForeignKeyViolation_ShouldThrowException()
    {
        var repository = CreateRepository();

        var inverter = new Inverter { SerialNumber = "456", Name = "INV1 " };
        Func<Task> action = async () => await repository.UpsertInverterAsync(inverter);
        await action.Should().ThrowAsync<DbUpdateException>()
            .Where(i => i.InnerException != null && i.InnerException.Message.Contains("FK_Inverters_DataLoggers_DataLoggerId"));
    }

    [Fact]
    public async Task BulkUpsertDataEntriesAsync_EntitiesNotFound_ShouldAddAllEntities()
    {
        var repository = CreateRepository();

        var dataLoggerId = await CreateDataLoggerInDbAsync(repository);
        var inverterId = await CreateInverterInDbAsync(repository, dataLoggerId);

        const int entryCount = 10;
        var dataEntries = CreateDataEntries(inverterId, entryCount);
        await repository.BulkUpsertLogItemsAsync(dataEntries);

        var savedEntries = await repository.GetLogItemsByInverterIdAsync(inverterId);
        savedEntries.Should().HaveCount(entryCount);
        savedEntries.Select(i => i.Time).Should().BeEquivalentTo(dataEntries.Select(i => i.Time));
        savedEntries.Select(i => i.Values.EDay).Should().BeEquivalentTo(dataEntries.Select(i => i.Values.EDay));
        savedEntries.Select(i => i.UpdatedAtUtc).Should().AllBeEquivalentTo(FakeTimeProvider.GetUtcNow());
    }

    [Fact]
    public async Task BulkUpsertDataEntriesAsync_EntitiesAlreadyExist_ShouldUpdateAllEntities()
    {
        var repository = CreateRepository();

        var dataLoggerId = await CreateDataLoggerInDbAsync(repository);
        var inverterId = await CreateInverterInDbAsync(repository, dataLoggerId);

        const int entryCount = 10;
        var dataEntries1 = CreateDataEntries(inverterId, entryCount);
        await repository.BulkUpsertLogItemsAsync(dataEntries1);

        FakeTimeProvider.Advance(TimeSpan.FromHours(1));

        var dataEntries2 = CreateDataEntries(inverterId, entryCount, eDayBaseValue: 15.1234m);
        await repository.BulkUpsertLogItemsAsync(dataEntries2);

        var savedEntries = await repository.GetLogItemsByInverterIdAsync(inverterId);
        savedEntries.Should().HaveCount(entryCount);
        savedEntries.Select(i => i.Values.EDay).Should().BeEquivalentTo(dataEntries2.Select(i => i.Values.EDay));
        savedEntries.Select(i => i.UpdatedAtUtc).Should().AllBeEquivalentTo(FakeTimeProvider.GetUtcNow());
        savedEntries.Select(i => i.UpdatedAtUtc).Should().NotContain(dataEntries1[0].UpdatedAtUtc);
    }

    private static async Task<int> CreateDataLoggerInDbAsync(DataCollectorRepository repository)
    {
        var dataLogger = new DataLogger { SerialNumber = "123" };
        return await repository.UpsertDataLoggerAsync(dataLogger);
    }

    private static async Task<int> CreateInverterInDbAsync(DataCollectorRepository repository, int dataLoggerId)
    {
        var inverter = new Inverter { DataLoggerId = dataLoggerId, SerialNumber = "456", Name = "INV1 " };
        return await repository.UpsertInverterAsync(inverter);
    }

    private static List<LogItem> CreateDataEntries(int inverterId, int count, decimal eDayBaseValue = 0)
    {
        return Enumerable.Range(0, count)
            .Select(i => new LogItem
            {
                InverterId = inverterId,
                Time = TestTime.AddSeconds(i * 5),
                Values = new ValueSet
                {
                    EDay = eDayBaseValue + i
                }
            }).ToList();
    }
}