using GoFDesignPatterns.Core;

namespace GoFDesignPatterns.Structural;

// Complex Subsystem 1: Inventory
public class InventoryService
{
    public bool CheckAndReserveStock(string sku, int quantity, List<string> logs)
    {
        logs.Add($"[Inventory] Verified and reserved {quantity} unit(s) of SKU '{sku}'.");
        return true;
    }

    public void ReleaseStock(string sku, int quantity, List<string> logs)
    {
        logs.Add($"[Inventory] Released {quantity} unit(s) of SKU '{sku}' back to stock.");
    }
}

// Complex Subsystem 2: Payment Processing
public class PaymentProcessingService
{
    public Result<string> ChargeCreditCard(string customerId, decimal amount, List<string> logs)
    {
        if (amount <= 0)
        {
            logs.Add($"[Payment] Charge failed: Invalid amount {amount:C}");
            return Result<string>.Failure("InvalidAmount", "Payment amount must be greater than zero.");
        }

        var authCode = $"AUTH-{Guid.NewGuid().ToString()[..8].ToUpper()}";
        logs.Add($"[Payment] Successfully charged {amount:C} to customer '{customerId}'. AuthCode: {authCode}");
        return Result<string>.Success(authCode);
    }
}

// Complex Subsystem 3: Shipping & Logistics
public class ShippingService
{
    public string CreateShippingLabel(string customerId, string shippingAddress, List<string> logs)
    {
        var tracking = $"TRK-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}";
        logs.Add($"[Shipping] Generated FedEx label to '{shippingAddress}'. Tracking: {tracking}");
        return tracking;
    }
}

// Complex Subsystem 4: Notification
public class CustomerNotificationService
{
    public void SendOrderConfirmation(string customerEmail, string orderId, string trackingNumber, List<string> logs)
    {
        logs.Add($"[Notification] Sent order confirmation email to '{customerEmail}' for Order {orderId}.");
    }
}

public record CheckoutRequest(
    string CustomerId,
    string CustomerEmail,
    string ShippingAddress,
    string Sku,
    int Quantity,
    decimal TotalAmount
);

public record CheckoutReceipt(
    string OrderId,
    string PaymentAuthCode,
    string TrackingNumber,
    decimal TotalAmount,
    DateTime CreatedAt
);

/// <summary>
/// Facade: Simplifies the complex checkout workflow behind a single high-level method.
/// </summary>
public class OrderCheckoutFacade(
    InventoryService inventory,
    PaymentProcessingService payment,
    ShippingService shipping,
    CustomerNotificationService notification)
{
    public (Result<CheckoutReceipt> Result, List<string> Logs) PlaceOrder(CheckoutRequest request)
    {
        var logs = new List<string> { "Starting checkout orchestration via OrderCheckoutFacade..." };

        // 1. Reserve Inventory
        if (!inventory.CheckAndReserveStock(request.Sku, request.Quantity, logs))
        {
            return (Result<CheckoutReceipt>.Failure("OutOfStock", $"Insufficient stock for {request.Sku}"), logs);
        }

        // 2. Process Payment
        var paymentResult = payment.ChargeCreditCard(request.CustomerId, request.TotalAmount, logs);
        if (paymentResult.IsFailure)
        {
            // Rollback inventory reservation
            inventory.ReleaseStock(request.Sku, request.Quantity, logs);
            return (Result<CheckoutReceipt>.Failure(paymentResult.Error), logs);
        }

        // 3. Create Shipping Manifest
        var trackingNumber = shipping.CreateShippingLabel(request.CustomerId, request.ShippingAddress, logs);

        var orderId = $"ORD-{Guid.NewGuid().ToString()[..8].ToUpper()}";

        // 4. Send Confirmation Notification
        notification.SendOrderConfirmation(request.CustomerEmail, orderId, trackingNumber, logs);

        logs.Add($"Order {orderId} processed successfully!");

        var receipt = new CheckoutReceipt(
            OrderId: orderId,
            PaymentAuthCode: paymentResult.Value,
            TrackingNumber: trackingNumber,
            TotalAmount: request.TotalAmount,
            CreatedAt: DateTime.UtcNow
        );

        return (Result<CheckoutReceipt>.Success(receipt), logs);
    }
}
