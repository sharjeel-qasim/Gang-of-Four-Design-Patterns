namespace GoFDesignPatterns.Creational;

/// <summary>
/// Naive classic Singleton implementation.
/// WARNING: NOT thread-safe. Included for educational comparison.
/// In a multi-threaded application, multiple threads can evaluate (instance == null) simultaneously.
/// </summary>
public class SingletonDesignPattern
{
    private static SingletonDesignPattern? _instance;

    private SingletonDesignPattern() 
    {
        CreatedAt = DateTime.UtcNow;
    }

    public DateTime CreatedAt { get; }

    public static SingletonDesignPattern GetInstance()
    {
        if (_instance == null)
        {
            _instance = new SingletonDesignPattern();
        }
        return _instance;
    }
}

/// <summary>
/// Thread-safe Singleton using Double-Checked Locking.
/// Traditional senior approach before System.Lazy&lt;T&gt; was introduced in .NET 4.0.
/// </summary>
public sealed class ThreadSafeLockSingleton
{
    private static ThreadSafeLockSingleton? _instance;
    private static readonly object _lock = new();

    private ThreadSafeLockSingleton()
    {
        InstanceId = Guid.NewGuid();
    }

    public Guid InstanceId { get; }

    public static ThreadSafeLockSingleton Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new ThreadSafeLockSingleton();
                    }
                }
            }
            return _instance;
        }
    }
}

/// <summary>
/// Modern, idiomatic C# Singleton using System.Lazy&lt;T&gt;.
/// Guaranteed thread-safe, lazy-initialized, and high performance without manual locking.
/// Recommended when a static singleton is genuinely required.
/// </summary>
public sealed class LazySingleton
{
    private static readonly Lazy<LazySingleton> _lazyInstance =
        new(() => new LazySingleton(), LazyThreadSafetyMode.ExecutionAndPublication);

    private LazySingleton()
    {
        InstanceId = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    public Guid InstanceId { get; }
    public DateTime CreatedAt { get; }

    public static LazySingleton Instance => _lazyInstance.Value;

    public string DoWork(string caller) =>
        $"[LazySingleton: {InstanceId}] Processed work for '{caller}' at {DateTime.UtcNow:HH:mm:ss.fff}";
}
