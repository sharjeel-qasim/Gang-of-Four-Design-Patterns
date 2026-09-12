using System.Text.Json;
using GoFDesignPatterns.Core;

namespace GoFDesignPatterns.ModernEnterprise;

public record OutboxMessage
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string EventType { get; init; }
    public required string Payload { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? ProcessedAt { get; set; }
    public bool IsProcessed { get; set; }
}

public class SimulatedDatabaseContext
{
    public List<CustomerOrder> Orders { get; } = [];
    public List<OutboxMessage> OutboxMessages { get; } = [];
}

/// <summary>
/// Senior Distributed Systems Pattern: Transactional Outbox.
/// Solves Dual-Write problem when persisting database state AND publishing a message to a broker.
/// Instead of calling the message bus inside the transaction, an OutboxMessage is saved atomically
/// to the same database. A reliable background worker reads the outbox and dispatches messages with retry.
/// </summary>
public class OrderServiceWithOutbox(SimulatedDatabaseContext db, List<string> logs)
{
    public Result<string> PlaceOrder(string orderId, string customerTier, decimal totalAmount)
    {
        logs.Add($"[Outbox Transaction] Beginning atomic database transaction for Order '{orderId}'...");

        // 1. Business entity persistence
        var order = new CustomerOrder(orderId, customerTier, totalAmount, false);
        db.Orders.Add(order);
        logs.Add($"[Outbox Transaction] Saved order entity '{orderId}' to database table.");

        // 2. Outbox event persistence in SAME transaction
        var orderCreatedEvent = new
        {
            OrderId = orderId,
            CustomerTier = customerTier,
            Amount = totalAmount,
            Timestamp = DateTime.UtcNow
        };

        var outboxMessage = new OutboxMessage
        {
            EventType = "OrderCreatedEvent",
            Payload = JsonSerializer.Serialize(orderCreatedEvent)
        };

        db.OutboxMessages.Add(outboxMessage);
        logs.Add($"[Outbox Transaction] Added OutboxMessage {outboxMessage.Id} ({outboxMessage.EventType}) to Outbox table.");
        logs.Add("[Outbox Transaction] Transaction COMMITTED successfully. Database and Outbox are consistent.");

        return Result<string>.Success($"Order {orderId} placed with transactional outbox message {outboxMessage.Id}");
    }
}

public class OutboxMessagePublisher(SimulatedDatabaseContext db, List<string> logs)
{
    public int DispatchPendingMessages()
    {
        logs.Add("[Outbox Worker] Background worker polling for unhandled Outbox messages...");
        var pending = db.OutboxMessages.Where(m => !m.IsProcessed).ToList();

        foreach (var message in pending)
        {
            logs.Add($"[Outbox Worker] Publishing '{message.EventType}' ({message.Id}) to RabbitMQ/Kafka message bus...");
            // Simulate broker delivery
            message.IsProcessed = true;
            message.ProcessedAt = DateTime.UtcNow;
            logs.Add($"[Outbox Worker] Message {message.Id} marked as Processed.");
        }

        logs.Add($"[Outbox Worker] Successfully dispatched {pending.Count} pending message(s).");
        return pending.Count;
    }
}
