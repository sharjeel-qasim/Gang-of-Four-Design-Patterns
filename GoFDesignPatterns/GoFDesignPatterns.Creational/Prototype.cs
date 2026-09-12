namespace GoFDesignPatterns.Creational;

public interface IPrototype<out T>
{
    T Clone();
}

public class NetworkSettings(string ipAddress, int port, List<string> allowedSubnets)
{
    public string IpAddress { get; set; } = ipAddress;
    public int Port { get; set; } = port;
    public List<string> AllowedSubnets { get; set; } = allowedSubnets;

    public NetworkSettings DeepClone() =>
        new(IpAddress, Port, [.. AllowedSubnets]);
}

/// <summary>
/// Heavy Server Configuration Template illustrating Shallow vs Deep Cloning.
/// </summary>
public class ServerConfiguration : IPrototype<ServerConfiguration>
{
    public string ServerName { get; set; }
    public int CpuCores { get; set; }
    public int MemoryGb { get; set; }
    public NetworkSettings Network { get; set; }
    public Dictionary<string, string> EnvironmentVariables { get; set; }

    public ServerConfiguration(
        string serverName,
        int cpuCores,
        int memoryGb,
        NetworkSettings network,
        Dictionary<string, string> environmentVariables)
    {
        ServerName = serverName;
        CpuCores = cpuCores;
        MemoryGb = memoryGb;
        Network = network;
        EnvironmentVariables = environmentVariables;
    }

    /// <summary>
    /// Shallow copy - copies primitive values, but references to Network and EnvironmentVariables are shared.
    /// Mutating the clone's subnets or env vars mutates the original!
    /// </summary>
    public ServerConfiguration ShallowClone() => (ServerConfiguration)MemberwiseClone();

    /// <summary>
    /// Deep copy - completely creates new instances of all reference graph objects.
    /// Mutating the clone has zero side-effects on the original template.
    /// </summary>
    public ServerConfiguration Clone()
    {
        return new ServerConfiguration(
            $"{ServerName}-Clone",
            CpuCores,
            MemoryGb,
            Network.DeepClone(),
            new Dictionary<string, string>(EnvironmentVariables)
        );
    }
}

/// <summary>
/// Modern C# 12 Record Prototype alternative using the built-in 'with' expression.
/// </summary>
public record CloudResourceTemplate(
    string Name,
    string Region,
    string InstanceType,
    IReadOnlyList<string> Tags
)
{
    // Records provide non-destructive mutation via 'with' keyword:
    // var cloned = template with { Name = "Production-Worker", Region = "us-east-1" };
}
