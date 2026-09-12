using System.Diagnostics;
using GoFDesignPatterns.Core;

namespace GoFDesignPatterns.Structural;

public record OrderSummary(Guid OrderId, string CustomerName, decimal TotalAmount, string Status);

public interface IOrderService
{
    Result<OrderSummary> GetOrderById(Guid orderId);
}

// Concrete Component
public class OrderService : IOrderService
{
    private readonly Dictionary<Guid, OrderSummary> _database = new()
    {
        [Guid.Parse("11111111-1111-1111-1111-111111111111")] =
            new(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Alice Johnson", 249.99m, "Completed"),
        [Guid.Parse("22222222-2222-2222-2222-222222222222")] =
            new(Guid.Parse("22222222-2222-2222-2222-222222222222"), "Bob Smith", 89.50m, "Processing")
    };

    public Result<OrderSummary> GetOrderById(Guid orderId)
    {
        // Simulate DB latency
        Thread.Sleep(15);

        return _database.TryGetValue(orderId, out var order)
            ? Result<OrderSummary>.Success(order)
            : Result<OrderSummary>.Failure("NotFound", $"Order {orderId} was not found in database.");
    }
}

// Base Decorator
public abstract class OrderServiceDecorator(IOrderService innerService) : IOrderService
{
    protected readonly IOrderService InnerService = innerService;

    public virtual Result<OrderSummary> GetOrderById(Guid orderId) => InnerService.GetOrderById(orderId);
}

// Concrete Decorator 1: Logging
public class LoggingOrderServiceDecorator(IOrderService innerService, List<string> auditLog)
    : OrderServiceDecorator(innerService)
{
    public override Result<OrderSummary> GetOrderById(Guid orderId)
    {
        auditLog.Add($"[LOG] Starting query for OrderId: {orderId}");
        var result = base.GetOrderById(orderId);
        auditLog.Add(result.IsSuccess
            ? $"[LOG] Successfully retrieved OrderId: {orderId} (Status: {result.Value.Status})"
            : $"[LOG] Failed to retrieve OrderId: {orderId} (Error: {result.Error.Message})");
        return result;
    }
}

// Concrete Decorator 2: In-Memory Caching
public class CachingOrderServiceDecorator(IOrderService innerService, List<string> auditLog)
    : OrderServiceDecorator(innerService)
{
    private readonly Dictionary<Guid, OrderSummary> _cache = [];

    public override Result<OrderSummary> GetOrderById(Guid orderId)
    {
        if (_cache.TryGetValue(orderId, out var cached))
        {
            auditLog.Add($"[CACHE HIT] Retrieved OrderId: {orderId} from in-memory cache.");
            return Result<OrderSummary>.Success(cached);
        }

        auditLog.Add($"[CACHE MISS] OrderId: {orderId} not found in cache. Querying inner service...");
        var result = base.GetOrderById(orderId);

        if (result.IsSuccess)
        {
            _cache[orderId] = result.Value;
            auditLog.Add($"[CACHE STORE] Stored OrderId: {orderId} in cache.");
        }

        return result;
    }
}

// Concrete Decorator 3: Performance Profiling
public class PerformanceOrderServiceDecorator(IOrderService innerService, List<string> auditLog)
    : OrderServiceDecorator(innerService)
{
    public override Result<OrderSummary> GetOrderById(Guid orderId)
    {
        var sw = Stopwatch.StartNew();
        var result = base.GetOrderById(orderId);
        sw.Stop();
        auditLog.Add($"[METRICS] Query for OrderId: {orderId} took {sw.ElapsedMilliseconds}ms");
        return result;
    }
}
