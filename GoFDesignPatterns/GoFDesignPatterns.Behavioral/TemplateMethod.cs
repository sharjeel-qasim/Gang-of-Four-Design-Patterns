using GoFDesignPatterns.Core;

namespace GoFDesignPatterns.Behavioral;

public record IngestionRecord(string Id, string Name, decimal Value, DateTime Timestamp);

/// <summary>
/// Abstract Class defining the Template Method skeleton for an ETL Data Ingestion pipeline.
/// Subclasses implement primitive steps without altering the high-level workflow algorithm.
/// </summary>
public abstract class DataIngestionPipeline
{
    public abstract string SourceFormat { get; }

    // The Template Method: Sealed or non-virtual to protect the invariant pipeline steps.
    public Result<List<IngestionRecord>> ExecutePipeline(string rawSourceData, List<string> logs)
    {
        logs.Add($"Starting ingestion pipeline for format: {SourceFormat}");

        // Step 1: Extract
        logs.Add($"[Step 1: Extract] Parsing raw {SourceFormat} input stream...");
        var extractedData = ExtractData(rawSourceData);

        // Step 2: Validate (Can have a default implementation or hook)
        logs.Add("[Step 2: Validate] Verifying schema and field integrity...");
        if (!ValidateSchema(extractedData, out var validationError))
        {
            logs.Add($"[Validation Failed] {validationError}");
            return Result<List<IngestionRecord>>.Failure("ValidationFailed", validationError);
        }

        // Step 3: Transform
        logs.Add("[Step 3: Transform] Applying business transformations and normalization...");
        var transformedRecords = TransformData(extractedData);

        // Optional Hook: Post-transformation audit
        OnRecordsTransformedHook(transformedRecords, logs);

        // Step 4: Load
        logs.Add($"[Step 4: Load] Persisting {transformedRecords.Count} records into database...");
        LoadRecords(transformedRecords);

        logs.Add($"Pipeline execution completed successfully for {SourceFormat}.");
        return Result<List<IngestionRecord>>.Success(transformedRecords);
    }

    protected abstract List<Dictionary<string, string>> ExtractData(string raw);
    protected virtual bool ValidateSchema(List<Dictionary<string, string>> records, out string error)
    {
        if (records.Count == 0)
        {
            error = "Input stream contains zero data rows.";
            return false;
        }

        error = string.Empty;
        return true;
    }

    protected abstract List<IngestionRecord> TransformData(List<Dictionary<string, string>> records);
    protected virtual void OnRecordsTransformedHook(List<IngestionRecord> records, List<string> logs) { }
    protected virtual void LoadRecords(List<IngestionRecord> records) { }
}

public class CsvDataIngestionPipeline : DataIngestionPipeline
{
    public override string SourceFormat => "CSV";

    protected override List<Dictionary<string, string>> ExtractData(string raw)
    {
        var lines = raw.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length <= 1) return [];

        var headers = lines[0].Split(',').Select(h => h.Trim()).ToArray();
        var rows = new List<Dictionary<string, string>>();

        for (int i = 1; i < lines.Length; i++)
        {
            var cols = lines[i].Split(',').Select(c => c.Trim()).ToArray();
            var dict = new Dictionary<string, string>();
            for (int h = 0; h < headers.Length && h < cols.Length; h++)
            {
                dict[headers[h]] = cols[h];
            }
            rows.Add(dict);
        }

        return rows;
    }

    protected override List<IngestionRecord> TransformData(List<Dictionary<string, string>> records)
    {
        return records.Select(r => new IngestionRecord(
            Id: r.GetValueOrDefault("Id", Guid.NewGuid().ToString()),
            Name: r.GetValueOrDefault("Name", "Unknown"),
            Value: decimal.TryParse(r.GetValueOrDefault("Value", "0"), out var v) ? v : 0m,
            Timestamp: DateTime.UtcNow
        )).ToList();
    }

    protected override void OnRecordsTransformedHook(List<IngestionRecord> records, List<string> logs)
    {
        logs.Add($"[CSV Hook] Anonymized sensitive fields for {records.Count} CSV records.");
    }
}

public class JsonDataIngestionPipeline : DataIngestionPipeline
{
    public override string SourceFormat => "JSON";

    protected override List<Dictionary<string, string>> ExtractData(string raw)
    {
        // Simple mock extraction for demonstration
        return [
            new() { ["Id"] = "J-101", ["Name"] = "Cloud Subscription", ["Value"] = "499.00" },
            new() { ["Id"] = "J-102", ["Name"] = "SSL Certificate", ["Value"] = "79.99" }
        ];
    }

    protected override List<IngestionRecord> TransformData(List<Dictionary<string, string>> records)
    {
        return records.Select(r => new IngestionRecord(
            Id: r["Id"],
            Name: r["Name"].ToUpperInvariant(),
            Value: decimal.Parse(r["Value"]),
            Timestamp: DateTime.UtcNow
        )).ToList();
    }
}
