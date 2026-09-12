namespace GoFDesignPatterns.Behavioral;

public record ProductCatalogItem(string Sku, string Name, string Category, decimal Price, bool InStock);

// Abstract Expression
public interface IProductExpression
{
    bool Interpret(ProductCatalogItem item);
    string Describe();
}

// Terminal Expression 1: Category Match
public class CategoryEqualsExpression(string category) : IProductExpression
{
    public bool Interpret(ProductCatalogItem item) =>
        string.Equals(item.Category, category, StringComparison.OrdinalIgnoreCase);

    public string Describe() => $"Category == '{category}'";
}

// Terminal Expression 2: Price Less Than Or Equal
public class PriceUnderExpression(decimal maxPrice) : IProductExpression
{
    public bool Interpret(ProductCatalogItem item) => item.Price <= maxPrice;

    public string Describe() => $"Price <= {maxPrice:C}";
}

// Terminal Expression 3: In Stock Check
public class InStockExpression : IProductExpression
{
    public bool Interpret(ProductCatalogItem item) => item.InStock;

    public string Describe() => "InStock == true";
}

// Non-Terminal Expression: AND Operator
public class AndExpression(IProductExpression left, IProductExpression right) : IProductExpression
{
    public bool Interpret(ProductCatalogItem item) => left.Interpret(item) && right.Interpret(item);

    public string Describe() => $"({left.Describe()} AND {right.Describe()})";
}

// Non-Terminal Expression: OR Operator
public class OrExpression(IProductExpression left, IProductExpression right) : IProductExpression
{
    public bool Interpret(ProductCatalogItem item) => left.Interpret(item) || right.Interpret(item);

    public string Describe() => $"({left.Describe()} OR {right.Describe()})";
}

// Non-Terminal Expression: NOT Operator
public class NotExpression(IProductExpression expression) : IProductExpression
{
    public bool Interpret(ProductCatalogItem item) => !expression.Interpret(item);

    public string Describe() => $"NOT({expression.Describe()})";
}

public class ProductRuleEvaluator
{
    public static List<ProductCatalogItem> Filter(
        IEnumerable<ProductCatalogItem> catalog,
        IProductExpression rule,
        List<string> logs)
    {
        logs.Add($"Evaluating rule: {rule.Describe()}");
        var matches = catalog.Where(rule.Interpret).ToList();
        logs.Add($"Found {matches.Count} matching item(s).");
        return matches;
    }
}
