using GoFDesignPatterns.Core;

namespace GoFDesignPatterns.Behavioral;

/// <summary>
/// Memento: Immutable snapshot of the Originator's state.
/// Contains no public setters, preserving encapsulation.
/// </summary>
public record CartMemento(
    DateTime Timestamp,
    IReadOnlyList<OrderItem> Items,
    decimal TotalAmount,
    string Description
);

/// <summary>
/// Originator: The shopping cart whose state needs saving and restoring.
/// </summary>
public class ShoppingCartOriginator
{
    private readonly List<OrderItem> _items = [];

    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();
    public decimal TotalAmount => _items.Sum(i => i.TotalPrice);

    public void AddItem(string sku, string name, int quantity, decimal unitPrice)
    {
        _items.Add(new OrderItem(sku, name, quantity, unitPrice));
    }

    public void Clear() => _items.Clear();

    // Create Memento (Snapshot)
    public CartMemento CreateSnapshot(string description)
    {
        return new CartMemento(
            DateTime.UtcNow,
            _items.Select(i => i with { }).ToList().AsReadOnly(),
            TotalAmount,
            description
        );
    }

    // Restore Memento
    public void Restore(CartMemento memento)
    {
        _items.Clear();
        _items.AddRange(memento.Items.Select(i => i with { }));
    }
}

/// <summary>
/// Caretaker: Manages the history of mementos without inspecting or altering their internal data.
/// </summary>
public class CartCaretaker
{
    private readonly Stack<CartMemento> _undoStack = [];
    private readonly List<string> _historyLog = [];

    public IReadOnlyList<string> HistoryLog => _historyLog.AsReadOnly();

    public void SaveCheckpoint(ShoppingCartOriginator cart, string checkpointName)
    {
        var memento = cart.CreateSnapshot(checkpointName);
        _undoStack.Push(memento);
        _historyLog.Add($"[CHECKPOINT] '{checkpointName}' saved ({memento.Items.Count} items, Total: {memento.TotalAmount:C})");
    }

    public Result<CartMemento> Undo(ShoppingCartOriginator cart)
    {
        if (_undoStack.Count == 0)
        {
            return Result<CartMemento>.Failure("NoCheckpoints", "No checkpoints available to restore.");
        }

        var memento = _undoStack.Pop();
        cart.Restore(memento);
        _historyLog.Add($"[RESTORED] Cart rolled back to '{memento.Description}' (Total: {memento.TotalAmount:C})");
        return Result<CartMemento>.Success(memento);
    }
}
