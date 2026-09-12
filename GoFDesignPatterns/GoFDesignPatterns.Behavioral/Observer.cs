namespace GoFDesignPatterns.Behavioral;

public record StockPriceUpdate(string Symbol, decimal Price, decimal PreviousPrice, DateTime Timestamp)
{
    public decimal Change => Price - PreviousPrice;
    public decimal PercentageChange => PreviousPrice != 0 ? (Change / PreviousPrice) * 100m : 0m;
}

/// <summary>
/// Subject (Observable): Emits stock price updates to subscribed observers.
/// Implements standard .NET System.IObservable&lt;StockPriceUpdate&gt;.
/// </summary>
public class StockMarketTicker : IObservable<StockPriceUpdate>
{
    private readonly List<IObserver<StockPriceUpdate>> _observers = [];
    private readonly Dictionary<string, decimal> _currentPrices = [];

    public IDisposable Subscribe(IObserver<StockPriceUpdate> observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }
        return new Unsubscriber(_observers, observer);
    }

    public void UpdateStockPrice(string symbol, decimal newPrice)
    {
        var previousPrice = _currentPrices.GetValueOrDefault(symbol, newPrice);
        _currentPrices[symbol] = newPrice;

        var update = new StockPriceUpdate(symbol, newPrice, previousPrice, DateTime.UtcNow);

        foreach (var observer in _observers)
        {
            observer.OnNext(update);
        }
    }

    private class Unsubscriber(List<IObserver<StockPriceUpdate>> observers, IObserver<StockPriceUpdate> observer)
        : IDisposable
    {
        public void Dispose() => observers.Remove(observer);
    }
}

// Observer 1: Algorithmic Trader
public class AlgorithmicTraderObserver(string traderName, decimal buyThresholdDropPercentage, List<string> logs)
    : IObserver<StockPriceUpdate>
{
    public void OnNext(StockPriceUpdate value)
    {
        if (value.PercentageChange <= -buyThresholdDropPercentage)
        {
            logs.Add($"[TRADER: {traderName}] BUY SIGNAL triggered for {value.Symbol}! Price dropped by {Math.Abs(value.PercentageChange):F2}% to {value.Price:C}");
        }
    }

    public void OnError(Exception error) => logs.Add($"[TRADER: {traderName}] Market feed error: {error.Message}");
    public void OnCompleted() => logs.Add($"[TRADER: {traderName}] Market session closed.");
}

// Observer 2: Compliance & Risk Management
public class RiskManagementObserver(List<string> logs) : IObserver<StockPriceUpdate>
{
    public void OnNext(StockPriceUpdate value)
    {
        if (Math.Abs(value.PercentageChange) >= 10m)
        {
            logs.Add($"[RISK AUDIT] HIGH VOLATILITY ALERT: {value.Symbol} moved {value.PercentageChange:+0.00;-0.00}% in single tick! Flagged for review.");
        }
    }

    public void OnError(Exception error) { }
    public void OnCompleted() { }
}
