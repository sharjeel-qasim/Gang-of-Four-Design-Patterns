namespace GoFDesignPatterns.Core;

public record Money(decimal Amount, string Currency = "USD")
{
    public static Money Zero(string currency = "USD") => new(0m, currency);
    public static Money From(decimal amount, string currency = "USD") => new(amount, currency);

    public static Money operator +(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException($"Cannot add {a.Currency} and {b.Currency}");
        return new Money(a.Amount + b.Amount, a.Currency);
    }

    public static Money operator *(Money a, decimal multiplier) =>
        new(a.Amount * multiplier, a.Currency);
}

public record Customer(Guid Id, string FullName, string Email, string Tier = "Standard");

public record OrderItem(string Sku, string ProductName, int Quantity, decimal UnitPrice)
{
    public decimal TotalPrice => Quantity * UnitPrice;
}

public enum OrderStatus
{
    Created,
    PaymentPending,
    Paid,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}
