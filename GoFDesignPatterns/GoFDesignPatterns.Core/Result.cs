namespace GoFDesignPatterns.Core;

/// <summary>
/// Represents standard error details in functional Result-based operations.
/// </summary>
public record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "A null value was provided.");

    public static Error NotFound(string entityName, object id) =>
        new($"{entityName}.NotFound", $"{entityName} with identifier '{id}' was not found.");

    public static Error Validation(string propertyName, string details) =>
        new("Validation.Error", $"Validation failed for '{propertyName}': {details}");

    public static Error Failure(string code, string message) => new(code, message);
}

/// <summary>
/// Non-generic Result type representing success or failure of an operation without payload.
/// Senior-level alternative to throwing exceptions for domain logic control flow.
/// </summary>
public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
            throw new InvalidOperationException("Successful result cannot have an error.");
        if (!isSuccess && error == Error.None)
            throw new InvalidOperationException("Failing result must have an error.");

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);
    public static Result Failure(string code, string message) => new(false, new Error(code, message));

    public TResult Match<TResult>(Func<TResult> onSuccess, Func<Error, TResult> onFailure) =>
        IsSuccess ? onSuccess() : onFailure(Error);
}

/// <summary>
/// Generic Result type encapsulating a strongly-typed value or an error.
/// </summary>
public class Result<T> : Result
{
    private readonly T? _value;

    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException($"Cannot access Value of a failure result: {Error.Message}");

    public T? ValueOrDefault => _value;

    private Result(T value) : base(true, Error.None)
    {
        _value = value;
    }

    private Result(Error error) : base(false, error)
    {
        _value = default;
    }

    public static Result<T> Success(T value) => new(value);
    public static new Result<T> Failure(Error error) => new(error);
    public static new Result<T> Failure(string code, string message) => new(new Error(code, message));

    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator Result<T>(Error error) => Failure(error);

    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onFailure) =>
        IsSuccess ? onSuccess(_value!) : onFailure(Error);

    public Result<TOutput> Map<TOutput>(Func<T, TOutput> mapper) =>
        IsSuccess ? Result<TOutput>.Success(mapper(_value!)) : Result<TOutput>.Failure(Error);

    public async Task<Result<TOutput>> MapAsync<TOutput>(Func<T, Task<TOutput>> mapper) =>
        IsSuccess ? Result<TOutput>.Success(await mapper(_value!)) : Result<TOutput>.Failure(Error);

    public Result<TOutput> Bind<TOutput>(Func<T, Result<TOutput>> binder) =>
        IsSuccess ? binder(_value!) : Result<TOutput>.Failure(Error);
}
