using GoFDesignPatterns.Core;

namespace GoFDesignPatterns.Behavioral;

public interface IDiscountStrategy
{
    string StrategyName { get; }
    decimal CalculateDiscount(Customer customer, decimal subtotal);
}

// Strategy 1: Percentage Discount
public class PercentageDiscountStrategy(decimal percentage) : IDiscountStrategy
{
    public string StrategyName => $"{percentage}% Off";
    public decimal CalculateDiscount(Customer customer, decimal subtotal) => subtotal * (percentage / 100m);
}

// Strategy 2: Fixed Amount Discount
public class FixedAmountDiscountStrategy(decimal fixedAmount) : IDiscountStrategy
{
    public string StrategyName => $"${fixedAmount:F2} Fixed Voucher";
    public decimal CalculateDiscount(Customer customer, decimal subtotal) => Math.Min(fixedAmount, subtotal);
}

// Strategy 3: VIP Loyalty Tier Strategy
public class VipTierDiscountStrategy : IDiscountStrategy
{
    public string StrategyName => "VIP Loyalty Tier Dynamic Rate";

    public decimal CalculateDiscount(Customer customer, decimal subtotal)
    {
        var rate = customer.Tier.ToUpperInvariant() switch
        {
            "PLATINUM" => 0.25m,
            "GOLD" => 0.15m,
            "SILVER" => 0.10m,
            _ => 0.05m
        };

        return subtotal * rate;
    }
}

// Strategy 4: Spend-Threshold Progressive Discount
public class ProgressiveThresholdDiscountStrategy : IDiscountStrategy
{
    public string StrategyName => "Progressive Cart Threshold Discount";

    public decimal CalculateDiscount(Customer customer, decimal subtotal) => subtotal switch
    {
        >= 500m => subtotal * 0.20m, // $100+ off
        >= 200m => subtotal * 0.10m,
        >= 100m => subtotal * 0.05m,
        _ => 0m
    };
}

/// <summary>
/// Context: Order Pricing Calculator configured with an interchangeable strategy.
/// </summary>
public class OrderPricingCalculator(IDiscountStrategy initialStrategy)
{
    public IDiscountStrategy CurrentStrategy { get; private set; } = initialStrategy;

    public void SetStrategy(IDiscountStrategy strategy) => CurrentStrategy = strategy;

    public (decimal Discount, decimal FinalTotal, string StrategyUsed) Calculate(Customer customer, decimal subtotal)
    {
        var discount = CurrentStrategy.CalculateDiscount(customer, subtotal);
        var finalTotal = Math.Max(0, subtotal - discount);
        return (discount, finalTotal, CurrentStrategy.StrategyName);
    }
}
