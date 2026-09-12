namespace GoFDesignPatterns.Core;

/// <summary>
/// Uniform response model providing comprehensive details and execution traces
/// for interactive API consumers and the CLI runner.
/// </summary>
public record PatternExecutionReport
{
    public required string PatternName { get; init; }
    public required string Category { get; init; }
    public required string Intent { get; init; }
    public required string RealWorldScenario { get; init; }
    public required List<string> ExecutionSteps { get; init; } = [];
    public object? ResultData { get; init; }
    public bool IsSuccess { get; init; } = true;
}
