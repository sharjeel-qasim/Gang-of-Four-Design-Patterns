using GoFDesignPatterns.Core;

namespace GoFDesignPatterns.ModernEnterprise;

public enum CircuitState
{
    Closed,    // Normal operation: Requests pass through
    Open,      // Tripped: Fast fail without calling external service
    HalfOpen   // Probing: Testing if downstream dependency has recovered
}

/// <summary>
/// Senior Cloud & Microservices Resilience Pattern: Circuit Breaker.
/// Protects system from cascading failures when an external API or database is degraded.
/// </summary>
public class CircuitBreaker(int failureThreshold = 2, TimeSpan resetTimeout = default)
{
    private readonly int _failureThreshold = failureThreshold;
    private readonly TimeSpan _resetTimeout = resetTimeout == default ? TimeSpan.FromMilliseconds(500) : resetTimeout;
    private int _consecutiveFailures;
    private DateTime _lastStateChange = DateTime.UtcNow;

    public CircuitState State { get; private set; } = CircuitState.Closed;

    public Result<T> Execute<T>(Func<Result<T>> protectedAction, List<string> logs)
    {
        // Check if Open circuit should transition to Half-Open
        if (State == CircuitState.Open)
        {
            if (DateTime.UtcNow - _lastStateChange > _resetTimeout)
            {
                State = CircuitState.HalfOpen;
                _lastStateChange = DateTime.UtcNow;
                logs.Add("[CircuitBreaker] Reset timeout elapsed. Transitioning from OPEN -> HALF-OPEN (Testing probe request).");
            }
            else
            {
                logs.Add("[CircuitBreaker - FAILING FAST] Circuit is OPEN! Request blocked immediately to protect service.");
                return Result<T>.Failure("CircuitOpen", "Circuit breaker is OPEN. Downstream dependency is unavailable.");
            }
        }

        try
        {
            logs.Add($"[CircuitBreaker] Executing action under state: {State}");
            var result = protectedAction();

            if (result.IsSuccess)
            {
                OnSuccess(logs);
                return result;
            }

            OnFailure(logs);
            return result;
        }
        catch (Exception ex)
        {
            OnFailure(logs);
            return Result<T>.Failure("UnhandledException", ex.Message);
        }
    }

    private void OnSuccess(List<string> logs)
    {
        if (State == CircuitState.HalfOpen)
        {
            State = CircuitState.Closed;
            _consecutiveFailures = 0;
            _lastStateChange = DateTime.UtcNow;
            logs.Add("[CircuitBreaker - RECOVERED] Probe request succeeded. Transitioning from HALF-OPEN -> CLOSED.");
        }
        else
        {
            _consecutiveFailures = 0;
        }
    }

    private void OnFailure(List<string> logs)
    {
        _consecutiveFailures++;
        logs.Add($"[CircuitBreaker - FAILURE] Failure registered. Consecutive count: {_consecutiveFailures}/{_failureThreshold}");

        if (State == CircuitState.HalfOpen || _consecutiveFailures >= _failureThreshold)
        {
            State = CircuitState.Open;
            _lastStateChange = DateTime.UtcNow;
            logs.Add("[CircuitBreaker - TRIPPED] Failure threshold breached. Transitioning to OPEN!");
        }
    }
}
