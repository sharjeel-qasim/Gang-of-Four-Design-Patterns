using GoFDesignPatterns.Core;
using GoFDesignPatterns.Creational;
using Microsoft.AspNetCore.Mvc;

namespace GoFDesignPatterns.API.Controllers;

[ApiController]
[Route("api/creational")]
[Produces("application/json")]
public class CreationalPatternsController : ControllerBase
{
    /// <summary>
    /// Demonstrates the Singleton pattern comparing Naive, Thread-Safe Lock, and Modern Lazy&lt;T&gt; approaches.
    /// </summary>
    [HttpGet("singleton")]
    [ProducesResponseType(typeof(PatternExecutionReport), StatusCodes.Status200OK)]
    public IActionResult RunSingletonDemo()
    {
        var logs = new List<string>();

        // 1. Classic Naive Singleton
        logs.Add("[Naive Singleton] Requesting instance twice...");
        var naive1 = SingletonDesignPattern.GetInstance();
        var naive2 = SingletonDesignPattern.GetInstance();
        logs.Add($"[Naive Singleton] Both references equal: {ReferenceEquals(naive1, naive2)}");

        // 2. ThreadSafeLockSingleton
        logs.Add("[ThreadSafe Lock Singleton] Requesting instance twice...");
        var lock1 = ThreadSafeLockSingleton.Instance;
        var lock2 = ThreadSafeLockSingleton.Instance;
        logs.Add($"[ThreadSafe Lock Singleton] InstanceId: {lock1.InstanceId} | Both equal: {ReferenceEquals(lock1, lock2)}");

        // 3. Modern LazySingleton
        logs.Add("[Modern Lazy<T> Singleton] Calling LazySingleton.Instance.DoWork()...");
        var lazy1 = LazySingleton.Instance;
        var lazy2 = LazySingleton.Instance;
        var workResult = lazy1.DoWork("API Client");
        logs.Add($"[Modern Lazy<T> Singleton] InstanceId: {lazy1.InstanceId} | Both equal: {ReferenceEquals(lazy1, lazy2)}");

        var report = new PatternExecutionReport
        {
            PatternName = "Singleton",
            Category = "Creational",
            Intent = "Ensure a class has only one instance, while providing a global access point to this instance.",
            RealWorldScenario = "Centralized logging, configuration cache, or thread-safe connection pooling.",
            ExecutionSteps = logs,
            ResultData = new
            {
                NaiveIsSameInstance = ReferenceEquals(naive1, naive2),
                ThreadSafeIsSameInstance = ReferenceEquals(lock1, lock2),
                LazyIsSameInstance = ReferenceEquals(lazy1, lazy2),
                LazySampleWork = workResult,
                BestPracticeNote = "In modern ASP.NET Core, prefer registering dependencies with services.AddSingleton<T>() into DI container instead of static singletons."
            }
        };

        return Ok(report);
    }

    /// <summary>
    /// Demonstrates the Factory Method pattern by creating and dispatching notifications dynamically.
    /// </summary>
    /// <param name="channel">Channel type: 'email', 'sms', or 'push'</param>
    /// <param name="recipient">Recipient address or phone number</param>
    /// <param name="message">Notification message body</param>
    [HttpPost("factory-method")]
    [ProducesResponseType(typeof(PatternExecutionReport), StatusCodes.Status200OK)]
    public IActionResult RunFactoryMethodDemo(
        [FromQuery] string channel = "email",
        [FromQuery] string recipient = "dev@company.com",
        [FromQuery] string message = "Your security token is ready.")
    {
        var logs = new List<string> { $"Selecting NotificationService creator based on requested channel: '{channel}'" };

        NotificationService service = channel.ToLowerInvariant() switch
        {
            "sms" => new SmsNotificationService(),
            "push" => new PushNotificationService(),
            _ => new EmailNotificationService()
        };

        logs.Add($"Instantiated concrete creator: {service.GetType().Name}");
        var sendResult = service.NotifyUser(recipient, message);
        logs.Add($"Result: {(sendResult.IsSuccess ? sendResult.Value : sendResult.Error.Message)}");

        return Ok(new PatternExecutionReport
        {
            PatternName = "Factory Method",
            Category = "Creational",
            Intent = "Define an interface for creating an object, but let subclasses decide which class to instantiate.",
            RealWorldScenario = "Multi-channel notification dispatcher routing messages across Email, SMS, and Push gateways.",
            ExecutionSteps = logs,
            ResultData = new { sendResult.IsSuccess, Output = sendResult.ValueOrDefault, Error = sendResult.Error }
        });
    }

    /// <summary>
    /// Demonstrates the Abstract Factory pattern by provisioning a cloud ecosystem without coupling to vendors.
    /// </summary>
    /// <param name="provider">Cloud provider: 'aws' or 'azure'</param>
    /// <param name="serviceName">Microservice name to deploy</param>
    [HttpPost("abstract-factory")]
    [ProducesResponseType(typeof(PatternExecutionReport), StatusCodes.Status200OK)]
    public IActionResult RunAbstractFactoryDemo(
        [FromQuery] string provider = "azure",
        [FromQuery] string serviceName = "PaymentsService")
    {
        ICloudInfrastructureFactory factory = provider.ToLowerInvariant() == "aws"
            ? new AwsInfrastructureFactory()
            : new AzureInfrastructureFactory();

        var orchestrator = new CloudDeploymentOrchestrator(factory);
        var logs = orchestrator.DeployMicroserviceEnvironment(serviceName);

        return Ok(new PatternExecutionReport
        {
            PatternName = "Abstract Factory",
            Category = "Creational",
            Intent = "Provide an interface for creating families of related or dependent objects without specifying their concrete classes.",
            RealWorldScenario = "Multi-cloud infrastructure abstraction (AWS vs Azure Storage, Queues, and Compute).",
            ExecutionSteps = logs,
            ResultData = new { Provider = factory.ProviderName, ServiceName = serviceName }
        });
    }

    /// <summary>
    /// Demonstrates the Builder pattern (Step-Builder) constructing a financial invoice with compile-time validation.
    /// </summary>
    [HttpPost("builder")]
    [ProducesResponseType(typeof(PatternExecutionReport), StatusCodes.Status200OK)]
    public IActionResult RunBuilderDemo()
    {
        var logs = new List<string> { "Initiating Step-Builder sequence for Financial Invoice..." };

        var customer = new Customer(Guid.NewGuid(), "Jane Doe", "jane.doe@enterprise.com", "VIP");
        logs.Add($"Set customer: {customer.FullName} ({customer.Email})");

        var invoice = InvoiceBuilder.Create()
            .ForCustomer(customer)
            .AddItem("SKU-100", "Enterprise Cloud License", 2, 450.00m)
            .AddItem("SKU-200", "Dedicated Support Tier", 1, 150.00m)
            .WithTaxRate(0.08m)
            .WithDiscount(100.00m)
            .WithNotes("Net 30 payment terms applied.")
            .Build();

        logs.Add($"Invoice built: {invoice.InvoiceNumber} | Subtotal: {invoice.Subtotal:C} | Discount: {invoice.DiscountAmount:C} | Total: {invoice.TotalAmount:C}");

        return Ok(new PatternExecutionReport
        {
            PatternName = "Builder (Fluent Step-Builder)",
            Category = "Creational",
            Intent = "Separate the construction of a complex object from its representation so that the same construction process can create different representations.",
            RealWorldScenario = "Financial Invoice and Complex Order composition with compile-time step guarantees.",
            ExecutionSteps = logs,
            ResultData = invoice
        });
    }

    /// <summary>
    /// Demonstrates the Prototype pattern showing Shallow vs Deep cloning and modern C# 'with' records.
    /// </summary>
    [HttpPost("prototype")]
    [ProducesResponseType(typeof(PatternExecutionReport), StatusCodes.Status200OK)]
    public IActionResult RunPrototypeDemo()
    {
        var logs = new List<string> { "Creating baseline master ServerConfiguration..." };

        var original = new ServerConfiguration(
            "Production-AppServer-01",
            16,
            64,
            new NetworkSettings("10.0.1.50", 443, ["10.0.1.0/24", "10.0.2.0/24"]),
            new Dictionary<string, string> { ["ENV"] = "Production", ["REGION"] = "us-east-1" }
        );

        logs.Add($"Master: {original.ServerName} | Subnets: {string.Join(", ", original.Network.AllowedSubnets)}");

        // 1. Deep Clone
        logs.Add("Cloning server template using Deep Clone...");
        var deepClone = original.Clone();
        deepClone.ServerName = "Staging-AppServer-01";
        deepClone.Network.AllowedSubnets.Add("192.168.1.0/24");

        logs.Add($"Deep Clone modified allowed subnets. Master subnets count: {original.Network.AllowedSubnets.Count} | Clone subnets count: {deepClone.Network.AllowedSubnets.Count}");
        logs.Add($"Notice: Deep copy safely preserved master template subnets: {string.Join(", ", original.Network.AllowedSubnets)}");

        // 2. Modern C# 12 Record 'with'
        logs.Add("Demonstrating modern C# record non-destructive mutation via 'with' keyword...");
        var recordTemplate = new CloudResourceTemplate("BaseTemplate", "us-east-1", "m5.large", ["Production"]);
        var clonedRecord = recordTemplate with { Name = "WorkerNode-01", InstanceType = "c5.xlarge" };
        logs.Add($"Record cloned: {clonedRecord.Name} ({clonedRecord.InstanceType}) in {clonedRecord.Region}");

        return Ok(new PatternExecutionReport
        {
            PatternName = "Prototype",
            Category = "Creational",
            Intent = "Specify the kinds of objects to create using a prototypical instance, and create new objects by copying this prototype.",
            RealWorldScenario = "Duplicating complex server or container configurations avoiding expensive database reconstruction.",
            ExecutionSteps = logs,
            ResultData = new
            {
                OriginalSubnetCount = original.Network.AllowedSubnets.Count,
                CloneSubnetCount = deepClone.Network.AllowedSubnets.Count,
                ClonedRecord = clonedRecord
            }
        });
    }
}
