using GoFDesignPatterns.Core;

namespace GoFDesignPatterns.Behavioral;

public interface IRequest<out TResponse>;

public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}

public interface IMediator
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}

/// <summary>
/// Lightweight in-process Mediator (similar to MediatR).
/// Decouples sender from receiver, enabling single-responsibility handlers and CQRS architecture.
/// </summary>
public class InMemoryMediator : IMediator
{
    private readonly Dictionary<Type, Func<object, CancellationToken, Task<object>>> _handlers = [];

    public void RegisterHandler<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler)
        where TRequest : IRequest<TResponse>
    {
        _handlers[typeof(TRequest)] = async (req, ct) =>
        {
            var response = await handler.Handle((TRequest)req, ct);
            return response!;
        };
    }

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        if (!_handlers.TryGetValue(requestType, out var handlerInvoker))
        {
            throw new InvalidOperationException($"No mediator handler registered for request type '{requestType.Name}'");
        }

        var result = await handlerInvoker(request, cancellationToken);
        return (TResponse)result;
    }
}

// Domain Request & Response
public record RegisterCustomerCommand(string Name, string Email, string Tier) : IRequest<Result<CustomerRegistrationDto>>;

public record CustomerRegistrationDto(Guid CustomerId, string Name, string Email, string WelcomeGift);

// Handler
public class RegisterCustomerCommandHandler(List<string> logs)
    : IRequestHandler<RegisterCustomerCommand, Result<CustomerRegistrationDto>>
{
    public Task<Result<CustomerRegistrationDto>> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
    {
        logs.Add($"[Mediator Handler] Received RegisterCustomerCommand for '{request.Name}' ({request.Email})");

        if (string.IsNullOrWhiteSpace(request.Email) || !request.Email.Contains('@'))
        {
            logs.Add("[Mediator Handler] Validation error: Invalid email.");
            return Task.FromResult(Result<CustomerRegistrationDto>.Failure("InvalidEmail", "Customer email is invalid."));
        }

        var customerId = Guid.NewGuid();
        var gift = request.Tier == "VIP" ? "$50 Welcome Credit" : "Free Shipping Coupon";
        logs.Add($"[Mediator Handler] Registered customer {customerId} with tier '{request.Tier}' and gift '{gift}'.");

        var dto = new CustomerRegistrationDto(customerId, request.Name, request.Email, gift);
        return Task.FromResult(Result<CustomerRegistrationDto>.Success(dto));
    }
}
