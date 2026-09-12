using GoFDesignPatterns.Core;
using GoFDesignPatterns.Structural;
using Microsoft.AspNetCore.Mvc;

namespace GoFDesignPatterns.API.Controllers;

[ApiController]
[Route("api/structural")]
[Produces("application/json")]
public class StructuralPatternsController : ControllerBase
{
    /// <summary>
    /// Demonstrates the Adapter pattern adapting a legacy XML SOAP banking service to modern IPaymentGateway.
    /// </summary>
    [HttpPost("adapter")]
    [ProducesResponseType(typeof(PatternExecutionReport), StatusCodes.Status200OK)]
    public IActionResult RunAdapterDemo(
        [FromQuery] string customerId = "ACC-998811",
        [FromQuery] decimal amount = 350.75m,
        [FromQuery] string currency = "USD")
    {
        var logs = new List<string>
        {
            $"Calling modern IPaymentGateway.ProcessPayment for customer '{customerId}' with amount {amount:C}..."
        };

        var legacySystem = new LegacyXmlBankingSystem();
        IPaymentGateway adapter = new LegacyBankingAdapter(legacySystem);

        logs.Add("[Adapter] Converting modern DTO to legacy XML request payload...");
        var result = adapter.ProcessPayment(customerId, amount, currency);

        if (result.IsSuccess)
        {
            logs.Add($"[Adapter] Received legacy XML response, parsed into PaymentTransaction: ID={result.Value.TransactionId}, Status={result.Value.Status}");
        }

        return Ok(new PatternExecutionReport
        {
            PatternName = "Adapter",
            Category = "Structural",
            Intent = "Convert the interface of a class into another interface clients expect. Adapter lets classes work together that couldn't otherwise because of incompatible interfaces.",
            RealWorldScenario = "Integrating legacy XML/SOAP mainframe systems into modern JSON/REST microservices.",
            ExecutionSteps = logs,
            ResultData = result.ValueOrDefault
        });
    }

    /// <summary>
    /// Demonstrates the Bridge pattern decoupling message abstractions (Standard, Urgent, Digest) from delivery channels.
    /// </summary>
    [HttpPost("bridge")]
    [ProducesResponseType(typeof(PatternExecutionReport), StatusCodes.Status200OK)]
    public IActionResult RunBridgeDemo(
        [FromQuery] string priority = "urgent",
        [FromQuery] string channel = "slack",
        [FromQuery] string recipient = "dev-ops-channel")
    {
        var logs = new List<string>();

        // 1. Concrete Implementor
        IMessageChannel messageChannel = channel.ToLowerInvariant() switch
        {
            "email" => new EmailMessageChannel(),
            "sms" => new SmsMessageChannel(),
            _ => new SlackMessageChannel()
        };
        logs.Add($"Selected delivery implementor channel: {messageChannel.ChannelName}");

        // 2. Abstraction
        Notification notification = priority.ToLowerInvariant() switch
        {
            "urgent" => new UrgentNotification(messageChannel),
            "digest" => new DigestNotification(messageChannel),
            _ => new StandardNotification(messageChannel)
        };
        logs.Add($"Constructed notification abstraction: {notification.GetType().Name}");

        var output = notification.Send(recipient, "Database CPU Alert", "Database replication lag has exceeded 45 seconds.");
        logs.Add($"Dispatched notification: {output}");

        return Ok(new PatternExecutionReport
        {
            PatternName = "Bridge",
            Category = "Structural",
            Intent = "Decouple an abstraction from its implementation so that the two can vary independently.",
            RealWorldScenario = "Multi-platform notification pipelines (Standard/Urgent/Digest notifications across Slack/Teams/Email/SMS).",
            ExecutionSteps = logs,
            ResultData = new { DeliveryOutput = output }
        });
    }

    /// <summary>
    /// Demonstrates the Composite pattern calculating recursive bundle pricing across product hierarchies.
    /// </summary>
    [HttpGet("composite")]
    [ProducesResponseType(typeof(PatternExecutionReport), StatusCodes.Status200OK)]
    public IActionResult RunCompositeDemo()
    {
        var logs = new List<string> { "Constructing recursive Product Catalog Composite Tree..." };

        // Leaves
        var laptop = new ProductItem("MacBook Pro M3", 1999.00m);
        var monitor = new ProductItem("4K UltraFine Display", 699.00m);
        var keyboard = new ProductItem("Mechanical Keyboard", 149.00m);
        var mouse = new ProductItem("Ergonomic Mouse", 89.00m);

        // Sub-Bundle with 5% discount
        var peripheralsBundle = new ProductBundle("Desk Peripherals Pack", 5m);
        peripheralsBundle.Add(keyboard);
        peripheralsBundle.Add(mouse);

        // Master Bundle with 10% discount
        var workstationBundle = new ProductBundle("Developer Workstation Bundle", 10m);
        workstationBundle.Add(laptop);
        workstationBundle.Add(monitor);
        workstationBundle.Add(peripheralsBundle);

        var treeVisualization = new List<string>();
        workstationBundle.Display(0, treeVisualization);

        var finalPrice = workstationBundle.GetPrice();
        logs.AddRange(treeVisualization);
        logs.Add($"Final composite price after recursive bundle discounts: ${finalPrice:F2}");

        return Ok(new PatternExecutionReport
        {
            PatternName = "Composite",
            Category = "Structural",
            Intent = "Compose objects into tree structures to represent part-whole hierarchies. Composite lets clients treat individual objects and compositions of objects uniformly.",
            RealWorldScenario = "E-Commerce Product Bundling and Nested Bill of Materials with recursive discount calculations.",
            ExecutionSteps = logs,
            ResultData = new { MasterBundle = workstationBundle.Name, CalculatedTotalPrice = finalPrice }
        });
    }

    /// <summary>
    /// Demonstrates the Decorator pattern dynamically augmenting IOrderService with Logging, Caching, and Timing.
    /// </summary>
    [HttpGet("decorator")]
    [ProducesResponseType(typeof(PatternExecutionReport), StatusCodes.Status200OK)]
    public IActionResult RunDecoratorDemo([FromQuery] Guid? orderId)
    {
        var targetId = orderId ?? Guid.Parse("11111111-1111-1111-1111-111111111111");
        var logs = new List<string> { "Assembling Decorator Pipeline: Metrics -> Cache -> Logging -> Core Service..." };

        IOrderService service = new OrderService();
        service = new LoggingOrderServiceDecorator(service, logs);
        service = new CachingOrderServiceDecorator(service, logs);
        service = new PerformanceOrderServiceDecorator(service, logs);

        logs.Add($"Executing query for OrderId: {targetId} (Call 1: Cache Miss expected)...");
        var call1 = service.GetOrderById(targetId);

        logs.Add($"Executing query for OrderId: {targetId} (Call 2: Cache Hit expected)...");
        var call2 = service.GetOrderById(targetId);

        return Ok(new PatternExecutionReport
        {
            PatternName = "Decorator",
            Category = "Structural",
            Intent = "Attach additional responsibilities to an object dynamically. Decorators provide a flexible alternative to subclassing for extending functionality.",
            RealWorldScenario = "Cross-cutting concerns in service pipelines (In-Memory Caching, Structured Logging, Distributed Tracing).",
            ExecutionSteps = logs,
            ResultData = new { Order = call1.ValueOrDefault, IsBothCallsIdentical = call1.ValueOrDefault == call2.ValueOrDefault }
        });
    }

    /// <summary>
    /// Demonstrates the Facade pattern orchestrating Inventory, Payment, Shipping, and Customer Notifications.
    /// </summary>
    [HttpPost("facade")]
    [ProducesResponseType(typeof(PatternExecutionReport), StatusCodes.Status200OK)]
    public IActionResult RunFacadeDemo()
    {
        var inventory = new InventoryService();
        var payment = new PaymentProcessingService();
        var shipping = new ShippingService();
        var notification = new CustomerNotificationService();

        var facade = new OrderCheckoutFacade(inventory, payment, shipping, notification);

        var request = new CheckoutRequest(
            CustomerId: "CUST-44321",
            CustomerEmail: "customer@domain.com",
            ShippingAddress: "742 Evergreen Terrace, Springfield",
            Sku: "LAPTOP-X1",
            Quantity: 1,
            TotalAmount: 1299.99m
        );

        var (result, logs) = facade.PlaceOrder(request);

        return Ok(new PatternExecutionReport
        {
            PatternName = "Facade",
            Category = "Structural",
            Intent = "Provide a unified interface to a set of interfaces in a subsystem. Facade defines a higher-level interface that makes the subsystem easier to use.",
            RealWorldScenario = "E-Commerce multi-service order checkout orchestration.",
            ExecutionSteps = logs,
            ResultData = result.ValueOrDefault
        });
    }

    /// <summary>
    /// Demonstrates the Flyweight pattern caching immutable graphic glyphs and reducing RAM allocation.
    /// </summary>
    [HttpGet("flyweight")]
    [ProducesResponseType(typeof(PatternExecutionReport), StatusCodes.Status200OK)]
    public IActionResult RunFlyweightDemo()
    {
        var logs = new List<string> { "Rendering 1,000 geographic map markers with Flyweight caching..." };

        var factory = new MapMarkerFactory();
        var markers = new List<MapMarker>();
        var random = new Random(42);

        string[] markerTypes = ["Hospital", "Police", "School", "Restaurant", "GasStation"];
        string[] colors = ["Red", "Blue", "Green", "Orange", "Purple"];

        for (int i = 0; i < 1000; i++)
        {
            var typeIndex = i % markerTypes.Length;
            var markerType = markerTypes[typeIndex];
            var color = colors[typeIndex];

            // Reuses shared flyweight icon!
            var icon = factory.GetMarkerIcon(markerType, $"{markerType.ToLower()}_icon.png", color);

            var lat = 40.7128 + (random.NextDouble() - 0.5);
            var lon = -74.0060 + (random.NextDouble() - 0.5);
            markers.Add(new MapMarker(lat, lon, $"{markerType} #{i + 1}", icon));
        }

        logs.Add($"Total map markers created: {markers.Count}");
        logs.Add($"Total Flyweight icon instances allocated in memory: {factory.TotalSharedFlyweightsCount}");
        logs.Add($"Memory efficiency: Instead of 1,000 separate icon graphics, only {factory.TotalSharedFlyweightsCount} shared instances were instantiated!");

        // Sample render output of first 3
        foreach (var marker in markers.Take(3))
        {
            logs.Add(marker.Draw());
        }

        return Ok(new PatternExecutionReport
        {
            PatternName = "Flyweight",
            Category = "Structural",
            Intent = "Use sharing to support large numbers of fine-grained objects efficiently.",
            RealWorldScenario = "Map rendering engines, game particle systems, high-density text formatting glyph caches.",
            ExecutionSteps = logs,
            ResultData = new
            {
                TotalObjects = markers.Count,
                SharedFlyweights = factory.TotalSharedFlyweightsCount
            }
        });
    }

    /// <summary>
    /// Demonstrates the Proxy pattern verifying role authorization and lazy-loading an expensive reporting engine.
    /// </summary>
    [HttpGet("proxy")]
    [ProducesResponseType(typeof(PatternExecutionReport), StatusCodes.Status200OK)]
    public IActionResult RunProxyDemo([FromQuery] string userRole = "Admin")
    {
        var logs = new List<string>();
        IReportService proxy = new SecuredReportProxy(logs);

        logs.Add($"Invoking proxy report generation with role: '{userRole}'");
        var result = proxy.GenerateFinancialReport("Q3-FINANCIALS", userRole);

        if (result.IsSuccess)
        {
            logs.Add($"Report Output: {result.Value}");
        }

        return Ok(new PatternExecutionReport
        {
            PatternName = "Proxy (Protection & Virtual)",
            Category = "Structural",
            Intent = "Provide a surrogate or placeholder for another object to control access to it.",
            RealWorldScenario = "Securing sensitive financial exports with role-based access control combined with lazy initialization of heavy reporting engines.",
            ExecutionSteps = logs,
            ResultData = new { result.IsSuccess, Output = result.ValueOrDefault, Error = result.Error }
        });
    }
}
