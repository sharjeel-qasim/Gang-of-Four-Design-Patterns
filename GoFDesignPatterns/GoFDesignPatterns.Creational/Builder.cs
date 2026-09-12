using GoFDesignPatterns.Core;

namespace GoFDesignPatterns.Creational;

public record Invoice(
    string InvoiceNumber,
    Customer Customer,
    IReadOnlyList<OrderItem> Items,
    decimal Subtotal,
    decimal TaxAmount,
    decimal DiscountAmount,
    decimal TotalAmount,
    string Currency,
    DateTime DueDate,
    string? Notes
);

// Step Builder Interfaces enforcing sequential construction at compile-time
public interface IInvoiceCustomerStep
{
    IInvoiceItemStep ForCustomer(Customer customer);
}

public interface IInvoiceItemStep
{
    IInvoiceItemStep AddItem(string sku, string name, int quantity, decimal unitPrice);
    IInvoiceOptionsStep WithTaxRate(decimal taxRatePercentage);
}

public interface IInvoiceOptionsStep
{
    IInvoiceOptionsStep WithDiscount(decimal discountAmount);
    IInvoiceOptionsStep WithDueDate(DateTime dueDate);
    IInvoiceOptionsStep WithNotes(string notes);
    Invoice Build();
}

/// <summary>
/// Fluent Step-Builder enforcing valid domain creation order at compile-time.
/// </summary>
public class InvoiceBuilder : IInvoiceCustomerStep, IInvoiceItemStep, IInvoiceOptionsStep
{
    private Customer? _customer;
    private readonly List<OrderItem> _items = [];
    private decimal _taxRatePercentage = 0.10m; // Default 10%
    private decimal _discountAmount = 0m;
    private DateTime _dueDate = DateTime.UtcNow.AddDays(30);
    private string? _notes;
    private readonly string _currency = "USD";

    private InvoiceBuilder() { }

    public static IInvoiceCustomerStep Create() => new InvoiceBuilder();

    public IInvoiceItemStep ForCustomer(Customer customer)
    {
        _customer = customer ?? throw new ArgumentNullException(nameof(customer));
        return this;
    }

    public IInvoiceItemStep AddItem(string sku, string name, int quantity, decimal unitPrice)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive.", nameof(quantity));
        if (unitPrice < 0) throw new ArgumentException("Unit price cannot be negative.", nameof(unitPrice));

        _items.Add(new OrderItem(sku, name, quantity, unitPrice));
        return this;
    }

    public IInvoiceOptionsStep WithTaxRate(decimal taxRatePercentage)
    {
        if (_items.Count == 0)
            throw new InvalidOperationException("Must add at least one item before setting tax.");

        _taxRatePercentage = taxRatePercentage;
        return this;
    }

    public IInvoiceOptionsStep WithDiscount(decimal discountAmount)
    {
        _discountAmount = discountAmount;
        return this;
    }

    public IInvoiceOptionsStep WithDueDate(DateTime dueDate)
    {
        _dueDate = dueDate;
        return this;
    }

    public IInvoiceOptionsStep WithNotes(string notes)
    {
        _notes = notes;
        return this;
    }

    public Invoice Build()
    {
        if (_customer == null) throw new InvalidOperationException("Customer is required.");
        if (_items.Count == 0) throw new InvalidOperationException("At least one line item is required.");

        var subtotal = _items.Sum(i => i.TotalPrice);
        var discountedSubtotal = Math.Max(0, subtotal - _discountAmount);
        var tax = discountedSubtotal * _taxRatePercentage;
        var total = discountedSubtotal + tax;
        var invoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpperInvariant()}";

        return new Invoice(
            InvoiceNumber: invoiceNumber,
            Customer: _customer,
            Items: _items.AsReadOnly(),
            Subtotal: subtotal,
            TaxAmount: tax,
            DiscountAmount: _discountAmount,
            TotalAmount: total,
            Currency: _currency,
            DueDate: _dueDate,
            Notes: _notes
        );
    }
}
