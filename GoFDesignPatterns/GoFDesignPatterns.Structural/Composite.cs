namespace GoFDesignPatterns.Structural;

/// <summary>
/// Component: Declares common operations for both simple (leaf) and complex (composite) catalog items.
/// </summary>
public interface ICatalogComponent
{
    string Name { get; }
    decimal GetPrice();
    void Display(int depth, List<string> output);
}

/// <summary>
/// Leaf: Represents individual products with no children.
/// </summary>
public class ProductItem(string name, decimal price) : ICatalogComponent
{
    public string Name { get; } = name;
    public decimal Price { get; } = price;

    public decimal GetPrice() => Price;

    public void Display(int depth, List<string> output)
    {
        output.Add($"{new string('-', depth * 2)}> [Product] {Name}: ${Price:F2}");
    }
}

/// <summary>
/// Composite: Represents a product bundle or category containing other components (leaves or nested composites).
/// Supports discount rate applied across the bundle.
/// </summary>
public class ProductBundle(string name, decimal discountPercentage = 0m) : ICatalogComponent
{
    private readonly List<ICatalogComponent> _children = [];

    public string Name { get; } = name;
    public decimal DiscountPercentage { get; } = discountPercentage;
    public IReadOnlyList<ICatalogComponent> Children => _children.AsReadOnly();

    public void Add(ICatalogComponent component) => _children.Add(component);
    public void Remove(ICatalogComponent component) => _children.Remove(component);

    public decimal GetPrice()
    {
        var rawTotal = _children.Sum(child => child.GetPrice());
        var discount = rawTotal * (DiscountPercentage / 100m);
        return rawTotal - discount;
    }

    public void Display(int depth, List<string> output)
    {
        var discountInfo = DiscountPercentage > 0 ? $" ({DiscountPercentage}% bundle discount)" : "";
        output.Add($"{new string('-', depth * 2)}+ [Bundle] {Name}{discountInfo} -> Effective Price: ${GetPrice():F2}");

        foreach (var child in _children)
        {
            child.Display(depth + 1, output);
        }
    }
}
