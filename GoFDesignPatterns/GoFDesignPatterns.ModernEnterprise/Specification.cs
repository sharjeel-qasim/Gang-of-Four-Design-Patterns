using System.Linq.Expressions;
using GoFDesignPatterns.Core;

namespace GoFDesignPatterns.ModernEnterprise;

public interface ISpecification<T>
{
    bool IsSatisfiedBy(T entity);
    Expression<Func<T, bool>> ToExpression();
    ISpecification<T> And(ISpecification<T> other);
    ISpecification<T> Or(ISpecification<T> other);
    ISpecification<T> Not();
}

public abstract class Specification<T> : ISpecification<T>
{
    public abstract Expression<Func<T, bool>> ToExpression();

    public bool IsSatisfiedBy(T entity)
    {
        var predicate = ToExpression().Compile();
        return predicate(entity);
    }

    public ISpecification<T> And(ISpecification<T> other) => new AndSpecification<T>(this, other);
    public ISpecification<T> Or(ISpecification<T> other) => new OrSpecification<T>(this, other);
    public ISpecification<T> Not() => new NotSpecification<T>(this);
}

public class AndSpecification<T>(ISpecification<T> left, ISpecification<T> right) : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        var leftExpr = left.ToExpression();
        var rightExpr = right.ToExpression();

        var parameter = Expression.Parameter(typeof(T));
        var body = Expression.AndAlso(
            Expression.Invoke(leftExpr, parameter),
            Expression.Invoke(rightExpr, parameter)
        );

        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}

public class OrSpecification<T>(ISpecification<T> left, ISpecification<T> right) : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        var leftExpr = left.ToExpression();
        var rightExpr = right.ToExpression();

        var parameter = Expression.Parameter(typeof(T));
        var body = Expression.OrElse(
            Expression.Invoke(leftExpr, parameter),
            Expression.Invoke(rightExpr, parameter)
        );

        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}

public class NotSpecification<T>(ISpecification<T> spec) : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        var expr = spec.ToExpression();
        var parameter = Expression.Parameter(typeof(T));
        var body = Expression.Not(Expression.Invoke(expr, parameter));
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}

// Concrete Specifications on Customer Order Domain
public record CustomerOrder(string OrderId, string CustomerTier, decimal TotalAmount, bool IsFlaggedForFraud);

public class PremiumCustomerSpecification : Specification<CustomerOrder>
{
    public override Expression<Func<CustomerOrder, bool>> ToExpression() =>
        order => order.CustomerTier == "VIP" || order.CustomerTier == "Platinum";
}

public class HighValueOrderSpecification(decimal minAmount) : Specification<CustomerOrder>
{
    public override Expression<Func<CustomerOrder, bool>> ToExpression() =>
        order => order.TotalAmount >= minAmount;
}

public class FraudFreeSpecification : Specification<CustomerOrder>
{
    public override Expression<Func<CustomerOrder, bool>> ToExpression() =>
        order => !order.IsFlaggedForFraud;
}
