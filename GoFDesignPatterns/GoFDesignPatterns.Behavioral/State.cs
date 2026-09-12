using GoFDesignPatterns.Core;

namespace GoFDesignPatterns.Behavioral;

public interface IOrderFulfillmentState
{
    string StateName { get; }
    Result<string> Pay(OrderFulfillmentContext context);
    Result<string> Ship(OrderFulfillmentContext context);
    Result<string> Deliver(OrderFulfillmentContext context);
    Result<string> Cancel(OrderFulfillmentContext context);
}

public class OrderFulfillmentContext
{
    public string OrderId { get; }
    public decimal Amount { get; }
    public IOrderFulfillmentState CurrentState { get; internal set; }
    public List<string> History { get; } = [];

    public OrderFulfillmentContext(string orderId, decimal amount)
    {
        OrderId = orderId;
        Amount = amount;
        CurrentState = new CreatedOrderState();
        History.Add($"[INIT] Order {OrderId} created in state '{CurrentState.StateName}'");
    }

    public void TransitionTo(IOrderFulfillmentState newState)
    {
        History.Add($"[TRANSITION] '{CurrentState.StateName}' -> '{newState.StateName}'");
        CurrentState = newState;
    }

    public Result<string> Pay() => CurrentState.Pay(this);
    public Result<string> Ship() => CurrentState.Ship(this);
    public Result<string> Deliver() => CurrentState.Deliver(this);
    public Result<string> Cancel() => CurrentState.Cancel(this);
}

// State 1: Created
public class CreatedOrderState : IOrderFulfillmentState
{
    public string StateName => "Created";

    public Result<string> Pay(OrderFulfillmentContext context)
    {
        context.TransitionTo(new PaidOrderState());
        return Result<string>.Success($"Order {context.OrderId} marked as Paid.");
    }

    public Result<string> Ship(OrderFulfillmentContext context) =>
        Result<string>.Failure("InvalidState", "Cannot ship an unpaid order.");

    public Result<string> Deliver(OrderFulfillmentContext context) =>
        Result<string>.Failure("InvalidState", "Cannot deliver an order that has not shipped.");

    public Result<string> Cancel(OrderFulfillmentContext context)
    {
        context.TransitionTo(new CancelledOrderState());
        return Result<string>.Success($"Order {context.OrderId} has been cancelled.");
    }
}

// State 2: Paid
public class PaidOrderState : IOrderFulfillmentState
{
    public string StateName => "Paid";

    public Result<string> Pay(OrderFulfillmentContext context) =>
        Result<string>.Failure("InvalidState", "Order is already paid.");

    public Result<string> Ship(OrderFulfillmentContext context)
    {
        context.TransitionTo(new ShippedOrderState());
        return Result<string>.Success($"Order {context.OrderId} has been dispatched for delivery.");
    }

    public Result<string> Deliver(OrderFulfillmentContext context) =>
        Result<string>.Failure("InvalidState", "Order cannot be delivered before shipping.");

    public Result<string> Cancel(OrderFulfillmentContext context)
    {
        context.TransitionTo(new CancelledOrderState());
        return Result<string>.Success($"Order {context.OrderId} cancelled. Refund issued for {context.Amount:C}.");
    }
}

// State 3: Shipped
public class ShippedOrderState : IOrderFulfillmentState
{
    public string StateName => "Shipped";

    public Result<string> Pay(OrderFulfillmentContext context) =>
        Result<string>.Failure("InvalidState", "Order is already paid and in transit.");

    public Result<string> Ship(OrderFulfillmentContext context) =>
        Result<string>.Failure("InvalidState", "Order is already shipped.");

    public Result<string> Deliver(OrderFulfillmentContext context)
    {
        context.TransitionTo(new DeliveredOrderState());
        return Result<string>.Success($"Order {context.OrderId} delivered to recipient.");
    }

    public Result<string> Cancel(OrderFulfillmentContext context) =>
        Result<string>.Failure("InvalidState", "Cannot cancel an order that is already in transit with the courier.");
}

// State 4: Delivered (Terminal)
public class DeliveredOrderState : IOrderFulfillmentState
{
    public string StateName => "Delivered";

    public Result<string> Pay(OrderFulfillmentContext context) => Result<string>.Failure("InvalidState", "Order is completed.");
    public Result<string> Ship(OrderFulfillmentContext context) => Result<string>.Failure("InvalidState", "Order is completed.");
    public Result<string> Deliver(OrderFulfillmentContext context) => Result<string>.Failure("InvalidState", "Already delivered.");
    public Result<string> Cancel(OrderFulfillmentContext context) => Result<string>.Failure("InvalidState", "Cannot cancel delivered order. Initiate return instead.");
}

// State 5: Cancelled (Terminal)
public class CancelledOrderState : IOrderFulfillmentState
{
    public string StateName => "Cancelled";

    public Result<string> Pay(OrderFulfillmentContext context) => Result<string>.Failure("InvalidState", "Order was cancelled.");
    public Result<string> Ship(OrderFulfillmentContext context) => Result<string>.Failure("InvalidState", "Order was cancelled.");
    public Result<string> Deliver(OrderFulfillmentContext context) => Result<string>.Failure("InvalidState", "Order was cancelled.");
    public Result<string> Cancel(OrderFulfillmentContext context) => Result<string>.Failure("InvalidState", "Order is already cancelled.");
}
