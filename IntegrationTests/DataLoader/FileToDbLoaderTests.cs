using System.Globalization;
using FluentAssertions;
using Sundroid.Homework.DataLoader.Loading;
using Sundroid.Homework.IntegrationTests.Infrastructure;
using Sundroid.Homework.Persistence;
using Sundroid.Homework.Persistence.Entities;
using Xunit;

namespace Sundroid.Homework.IntegrationTests.DataLoader;

/// <summary>
/// These tests are based on the testfile.csv that you can find in the project folder.
/// All asserts are based on the file contents.
/// </summary>
public sealed class FileToDbLoaderTests(IntegrationTestFixture testFixture)
    : IntegrationTestBase(testFixture)
{
    [Fact]
    public async Task LoadFileToDbAsync_ValidFile_ShouldAddAllEntities()
    {
        var repository = CreateRepository();
        var loader = CreateFileToDbLoader(repository);

        await loader.LoadFileToDbAsync("testfile.csv");

        await AssertTestFileContentAsync(repository);
    }

    [Fact]
    public async Task LoadFileToDbAsync_LoadingTheSameFileTwice_ShouldBeIdempotent()
    {
        var repository = CreateRepository();
        var loader = CreateFileToDbLoader(repository);

        await loader.LoadFileToDbAsync("testfile.csv");
        await loader.LoadFileToDbAsync("testfile.csv");

        await AssertTestFileContentAsync(repository);
    }

    [Fact]
    public async Task LoadFileToDbAsync_LoadingTheSameInverterAndTimeWithDifferentValues_LastLoadedValueShouldWin()
    {
        var repository = CreateRepository();
        var loader = CreateFileToDbLoader(repository);

        await loader.LoadFileToDbAsync("testfile.csv");
        await loader.LoadFileToDbAsync("testfile-update.csv");

        await AssertTestFileContentAsync(repository, expectedUpv1: 999.9999m);
    }

    private async Task AssertTestFileContentAsync(DataCollectorRepository repository, decimal? expectedUpv1 = null)
    {
        var dataLogger = await ValidateDataLoggerDataInDbAsync(repository, "102110045721");

        var inverter1 = await ValidateInverterDataInDbAsync(repository, dataLogger.Id, "INV1", "21010730236TLC900294");
        await ValidateInverterLogItemCountAsync(repository, inverter1.Id, 3);

        var inverter2 = await ValidateInverterDataInDbAsync(repository, dataLogger.Id, "INV2", "21010730236TL4902749");
        await ValidateInverterLogItemCountAsync(repository, inverter2.Id, 1);

        // Validating one of the log items in full details.
        await ValidateInverterLogItemAsync(repository, inverter2.Id, expectedUpv1 ?? 594.4m);
    }

    private static async Task<DataLogger> ValidateDataLoggerDataInDbAsync(
        DataCollectorRepository repository,
        string serialNumber)
    {
        var dataLogger = await repository.GetDataLoggerBySerialNumberAsync(serialNumber);
        dataLogger.Should().NotBeNull();
        return dataLogger;
    }

    private static async Task<Inverter> ValidateInverterDataInDbAsync(
        DataCollectorRepository repository,
        int dataLoggerId,
        string name,
        string serialNumber)
    {
        var inverter = await repository.GetInverterBySerialNumberAsync(serialNumber);
        inverter.Should().NotBeNull();
        inverter.Name.Should().Be(name);
        inverter.DataLoggerId.Should().Be(dataLoggerId);
        return inverter;
    }

    private static async Task ValidateInverterLogItemCountAsync(
        DataCollectorRepository repository,
        int inverterId,
        int expectedItemCount)
    {
        var inverterLogItems = await repository.GetLogItemsByInverterIdAsync(inverterId);
        inverterLogItems.Should().HaveCount(expectedItemCount);
    }

    private async Task ValidateInverterLogItemAsync(DataCollectorRepository repository, int inverterId, decimal upv1)
    {
        var inverterLogItems = await repository.GetLogItemsByInverterIdAsync(inverterId);
        inverterLogItems.Should().HaveCount(1);

        var logItem = inverterLogItems.First();
        logItem.Should().BeEquivalentTo(
            new LogItem
            {
                Id = logItem.Id,
                InverterId = inverterId,
                Time = ParseDateTimeOffsetUtc("2025-05-15 13:00:00"),
                Values = new ValueSet
                {
                    Upv1 = upv1,
                    Upv2 = 594.4m,
                    Upv3 = 588.9m,
                    Upv4 = 588.9m,
                    Upv5 = 595.0m,
                    Upv6 = 595.0m,
                    Upv7 = 591.2m,
                    Upv8 = 591.2m,
                    Ipv1 = 4.1m,
                    Ipv2 = 4.2m,
                    Ipv3 = 4.3m,
                    Ipv4 = 4.2m,
                    Ipv5 = 4.2m,
                    Ipv6 = 4.3m,
                    Ipv7 = 4.3m,
                    Ipv8 = 4.2m,
                    Uac1 = 229.7m,
                    Uac2 = 229.6m,
                    Uac3 = 228.7m,
                    Iac1 = 28.7m,
                    Iac2 = 28.6m,
                    Iac3 = 28.6m,
                    Status = 512,
                    Error = 0,
                    Temp = 48.7m,
                    Cos = 0.999m,
                    Fac = 50.02m,
                    Pac = 19.671m,
                    Qac = 0.000m,
                    Eac = 1.97m,
                    EDay = 143.20m,
                    ETotal = 259265.16m
                },
                CycleTime = 5,
                UpdatedAtUtc = FakeTimeProvider.GetUtcNow()
            }
        );
    }

    private static DateTimeOffset ParseDateTimeOffsetUtc(string text)
    {
        return DateTimeOffset.ParseExact(text, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
    }

    private static FileToDbLoader CreateFileToDbLoader(DataCollectorRepository repository)
    {
        return new FileToDbLoader(repository);
    }
}