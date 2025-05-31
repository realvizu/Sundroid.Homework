using Microsoft.Extensions.Configuration;
using Sundroid.Homework.DataLoader.Loading;
using Sundroid.Homework.Persistence;

namespace Sundroid.Homework.DataLoader;

/// <summary>
/// Implements the data loader console application.
/// Specify the loaded file name as a command line parameter.
/// Specify the DB connection string in appsettings.json.
/// </summary>
internal class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            if (args.Length != 1)
                throw new Exception("Please specify exactly 1 parameter: the filename to load.");

            var filename = args[0];
            var connectionString = GetConnectionString();
            var dbContextFactory = new DataCollectorDbContextFactory(connectionString);
            var repository = new DataCollectorRepository(dbContextFactory, TimeProvider.System);

            Console.WriteLine($"Loading {filename} to DB.");
            Console.WriteLine($"ConnectionString: {connectionString}");

            var loader = new FileToDbLoader(repository);
            await loader.LoadFileToDbAsync(filename);

            Console.WriteLine("Finished successfully.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Loading failed. Exception details:{Environment.NewLine}{e}");
        }
    }

    private static string GetConnectionString()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        return configuration.GetConnectionString("DataCollector")
               ?? throw new Exception("ConnectionStrings/DataCollector not found in config.");
    }
}