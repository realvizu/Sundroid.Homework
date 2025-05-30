using Sundroid.Homework.DataLoader.Parsing;
using Sundroid.Homework.Persistence;
using Sundroid.Homework.Persistence.Entities;

namespace Sundroid.Homework.DataLoader.Loading;

public class FileToDbLoader(DataCollectorRepository repository)
{
    public async Task LoadFileToDbAsync(string filename)
    {
        int currentDataLoggerId = 0;
        int currentInverterId = 0;
        var logItemBatch = new List<LogItem>();

        await foreach (var parsedItem in FileParser.GetParsedLinesAsync(filename))
        {
            switch (parsedItem)
            {
                case ParsedDataLoggerLine dataLoggerLine:
                    if (currentInverterId != 0 || logItemBatch.Any())
                        throw new FileParsingException("Out of order data logger line detected. Data logger must be the first valid line.");

                    var dataLoggerEntity = dataLoggerLine.ToEntity();
                    currentDataLoggerId = await repository.UpsertDataLoggerAsync(dataLoggerEntity);
                    break;

                case ParsedInverterLine inverterLine:
                    if (currentDataLoggerId == 0)
                        throw new FileParsingException("Trying to process an inverter row without data logger context.");

                    // When we reach an inverter line, we must save the previously collected log items for the previous inverter.
                    await SaveLogItemBatchAsync(logItemBatch);

                    var inverterEntity = inverterLine.ToEntity(currentDataLoggerId);
                    currentInverterId = await repository.UpsertInverterAsync(inverterEntity);
                    break;

                case ParsedLogItemLine logItemLine:
                    if (currentInverterId == 0)
                        throw new FileParsingException("Trying to process a data item row without inverter context.");

                    var logItemEntity = logItemLine.ToEntity(currentInverterId);
                    logItemBatch.Add(logItemEntity);
                    break;
            }
        }

        // We also have to save the last log item batch.
        await SaveLogItemBatchAsync(logItemBatch);
    }

    private async Task SaveLogItemBatchAsync(List<LogItem> logItemBatch)
    {
        if (logItemBatch.Any())
        {
            await repository.BulkUpsertLogItemsAsync(logItemBatch);
            logItemBatch.Clear();
        }
    }
}