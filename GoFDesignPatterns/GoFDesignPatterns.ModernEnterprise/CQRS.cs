using GoFDesignPatterns.Core;

namespace GoFDesignPatterns.ModernEnterprise;

// Command (Write Side)
public record CreateProductCommand(string Sku, string Name, decimal Price, int InitialStock);

public class ProductWriteModel
{
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockLevel { get; set; }
    public DateTime LastModifiedAt { get; set; }
}

// Query (Read Side - Flat Projection optimized for fast reads)
public record ProductReadModel(string Sku, string Name, string FormattedPrice, bool IsAvailable);

// In-Memory Storage segregating Command Store and Read Projection Store
public class ProductStore
{
    public Dictionary<string, ProductWriteModel> WriteDatabase { get; } = [];
    public Dictionary<string, ProductReadModel> ReadDatabase { get; } = [];
}

// Command Handler
public class CreateProductCommandHandler(ProductStore store, List<string> logs)
{
    public Result<string> Handle(CreateProductCommand command)
    {
        logs.Add($"[CQRS Command] Executing CreateProductCommand for SKU '{command.Sku}'");

        if (string.IsNullOrWhiteSpace(command.Sku))
            return Result<string>.Failure("InvalidSku", "SKU cannot be empty.");
        if (command.Price <= 0)
            return Result<string>.Failure("InvalidPrice", "Price must be positive.");

        // 1. Mutate Write Store
        var product = new ProductWriteModel
        {
            Sku = command.Sku,
            Name = command.Name,
            Price = command.Price,
            StockLevel = command.InitialStock,
            LastModifiedAt = DateTime.UtcNow
        };
        store.WriteDatabase[command.Sku] = product;
        logs.Add($"[CQRS Command] Persisted to write database (Stock: {product.StockLevel})");

        // 2. Project / Synchronize Read Store (Materialized View)
        var projection = new ProductReadModel(
            command.Sku,
            command.Name,
            $"{command.Price:C}",
            command.InitialStock > 0
        );
        store.ReadDatabase[command.Sku] = projection;
        logs.Add($"[CQRS Projection] Read-optimized projection updated for SKU '{command.Sku}'");

        return Result<string>.Success($"Product '{command.Sku}' created successfully.");
    }
}

// Query Handler
public class GetProductBySkuQueryHandler(ProductStore store, List<string> logs)
{
    public Result<ProductReadModel> Handle(string sku)
    {
        logs.Add($"[CQRS Query] Fetching read projection for SKU '{sku}'");

        return store.ReadDatabase.TryGetValue(sku, out var projection)
            ? Result<ProductReadModel>.Success(projection)
            : Result<ProductReadModel>.Failure("NotFound", $"Product with SKU '{sku}' not found in read store.");
    }
}
