using GoFDesignPatterns.Core;
using GoFDesignPatterns.ModernEnterprise;
using Microsoft.AspNetCore.Mvc;

namespace GoFDesignPatterns.API.Controllers;

[ApiController]
[Route("api/modern")]
[Produces("application/json")]
public class ModernEnterprisePatternsController : ControllerBase
{
    private static readonly ProductStore _cqrsStore = new();
    private static readonly SimulatedDatabaseContext _outboxDb = new();

    /// <summary>
    /// Demonstrates CQRS (Command Query Responsibility Segregation) separating write models from read projections.
    /// </summary>
    [HttpPost("cqrs")]
    [ProducesResponseType(typeof(PatternExecutionReport), StatusCodes.Status200OK)]
    public IActionResult RunCqrsDemo([FromQuery] string sku = "PROD-100", [FromQuery] string name = "Ergonomic Mechanical Keyboard", [FromQuery] decimal price = 149.99m)
    {
        var logs = new List<string>();
        var commandHandler = new CreateProductCommandHandler(_cqrsStore, logs);
        var queryHandler = new GetProductBySkuQueryHandler(_cqrsStore, logs);

        // 1. Execute Command (Mutates domain write store & updates projection)
        var command = new CreateProductCommand(sku, name, price, InitialStock: 50);
        var commandResult = commandHandler.Handle(command);

        // 2. Execute Query (Reads directly from optimized read store projection)
        var queryResult = queryHandler.Handle(sku);

        return Ok(new PatternExecutionReport
        {
            PatternName = "CQRS (Command Query Responsibility Segregation)",
            Category = "Modern Enterprise",
            Intent = "Separate read and update operations for a data store. Commands mutate state, while queries return denormalized views without side effects.",
            RealWorldScenario = "High-throughput e-commerce microservices separating transactional writes from cached search read-projections.",
            ExecutionSteps = logs,
            ResultData = new { CommandResult = commandResult.ValueOrDefault, ReadProjection = queryResult.ValueOrDefault }
        });
    }

    /// <summary>
    /// Demonstrates the Specification pattern dynamically composing business rules with And, Or, and Not combinators.
    /// </summary>
    [HttpPost("specification")]
    [ProducesResponseType(typeof(PatternExecutionReport), StatusCodes.Status200OK)]
    public IActionResult RunSpecificationDemo()
    {
        var logs = new List<string> { "Initializing candidate customer orders for evaluation..." };

        List<CustomerOrder> orders =
        [
            new("ORD-1", "VIP", 1500m, IsFlaggedForFraud: false),       // Premium, High Value, Fraud-free -> MATCH
            new("ORD-2", "Standard", 2500m, IsFlaggedForFraud: false),  // Not VIP
            new("ORD-3", "VIP", 300m, IsFlaggedForFraud: false),        // VIP, Low Value (< $1000)
            new("ORD-4", "Platinum", 5000m, IsFlaggedForFraud: true)    // High Value VIP, but Fraud Flagged!
        ];

        logs.Add("Composing Specification: (PremiumCustomer AND HighValueOrder(>= $1,000) AND FraudFree)...");

        var isPremium = new PremiumCustomerSpecification();
        var isHighValue = new HighValueOrderSpecification(1000m);
        var isFraudFree = new FraudFreeSpecification();

        var eligibleOrderRule = isPremium.And(isHighValue).And(isFraudFree);

        var matchingOrders = orders.Where(eligibleOrderRule.IsSatisfiedBy).ToList();
        logs.Add($"Found {matchingOrders.Count} order(s) satisfying composite business specification.");

        return Ok(new PatternExecutionReport
        {
            PatternName = "Specification",
            Category = "Modern Enterprise",
            Intent = "Encapsulate domain business rules into reusable objects that can be combined using boolean logic and applied to in-memory collections or translated to database SQL via Expressions.",
            RealWorldScenario = "Complex discount eligibility, loan approval criteria, and fraud verification policies in enterprise architectures.",
            ExecutionSteps = logs,
            ResultData = new { TotalOrdersEvaluated = orders.Count, MatchingOrders = matchingOrders }
        });
    }

    /// <summary>
    /// Demonstrates the Result Pattern (Railway-Oriented Programming) chaining multi-step validation without exceptions.
    /// </summary>
    [HttpPost("result-rop")]
    [ProducesResponseType(typeof(PatternExecutionReport), StatusCodes.Status200OK)]
    public IActionResult RunResultRopDemo([FromQuery] decimal amount = 25000m, [FromQuery] int creditScore = 720, [FromQuery] decimal monthlyIncome = 6500m)
    {
        var logs = new List<string>();
        var pipeline = new LoanProcessingPipeline(logs);

        var application = new LoanApplication(Guid.NewGuid(), "Jane Sterling", amount, creditScore, monthlyIncome);
        var result = pipeline.ProcessApplication(application);

        return Ok(new PatternExecutionReport
        {
            PatternName = "Result Pattern (Railway-Oriented Programming)",
            Category = "Modern Enterprise",
            Intent = "Model operations that can succeed or fail explicitly without throwing exceptions for control flow, enabling functional pipeline chaining via Bind and Map.",
            RealWorldScenario = "Financial underwriting and multi-stage compliance verification pipelines.",
            ExecutionSteps = logs,
            ResultData = new
            {
                result.IsSuccess,
                ApprovedLoan = result.ValueOrDefault,
                Error = result.IsFailure ? result.Error : null
            }
        });
    }

    /// <summary>
    /// Demonstrates the Transactional Outbox pattern ensuring reliable distributed messaging and eventual consistency.
    /// </summary>
    [HttpPost("outbox")]
    [ProducesResponseType(typeof(PatternExecutionReport), StatusCodes.Status200OK)]
    public IActionResult RunOutboxDemo([FromQuery] string orderId = "ORD-9901", [FromQuery] decimal amount = 399.50m)
    {
        var logs = new List<string>();
        var orderService = new OrderServiceWithOutbox(_outboxDb, logs);
        var publisher = new OutboxMessagePublisher(_outboxDb, logs);

        // 1. Transactionally write Order + Outbox event in same context
        var placeResult = orderService.PlaceOrder(orderId, "Gold", amount);

        // 2. Background worker polls and publishes outbox messages
        var dispatchedCount = publisher.DispatchPendingMessages();

        return Ok(new PatternExecutionReport
        {
            PatternName = "Transactional Outbox",
            Category = "Modern Enterprise",
            Intent = "Persist domain entities and outbound integration messages atomically in a single local database transaction to prevent dual-write inconsistencies.",
            RealWorldScenario = "Reliable event-driven architectures and microservices publishing events to Kafka or RabbitMQ with zero message loss.",
            ExecutionSteps = logs,
            ResultData = new
            {
                OrderPlaced = placeResult.ValueOrDefault,
                TotalOutboxMessagesInStore = _outboxDb.OutboxMessages.Count,
                MessagesDispatchedThisRun = dispatchedCount
            }
        });
    }

    /// <summary>
    /// Demonstrates the Circuit Breaker pattern handling downstream faults and recovering through Half-Open state.
    /// </summary>
    [HttpPost("circuit-breaker")]
    [ProducesResponseType(typeof(PatternExecutionReport), StatusCodes.Status200OK)]
    public IActionResult RunCircuitBreakerDemo()
    {
        var logs = new List<string> { "Initializing CircuitBreaker (Threshold: 2 failures, Reset: 200ms)..." };
        var breaker = new CircuitBreaker(failureThreshold: 2, resetTimeout: TimeSpan.FromMilliseconds(200));

        // Attempt 1: Simulating downstream 500 error
        logs.Add("--- Request 1: Downstream server returns 500 Internal Error ---");
        breaker.Execute<string>(() => Result<string>.Failure("Http500", "Remote gateway error"), logs);

        // Attempt 2: Simulating second downstream failure -> Should trip the breaker
        logs.Add("--- Request 2: Downstream server returns 500 Internal Error ---");
        breaker.Execute<string>(() => Result<string>.Failure("Http500", "Remote gateway error"), logs);

        // Attempt 3: Circuit is now OPEN -> Should FAIL FAST immediately
        logs.Add("--- Request 3: Fast-fail verification while circuit is OPEN ---");
        var fastFail = breaker.Execute<string>(() => Result<string>.Success("Not Called"), logs);

        // Sleep to exceed reset timeout
        logs.Add("Waiting 250ms for reset timeout to elapse...");
        Thread.Sleep(250);

        // Attempt 4: Probing in HALF-OPEN state with successful call -> Should recover
        logs.Add("--- Request 4: Probe call after reset timeout ---");
        var recovered = breaker.Execute<string>(() => Result<string>.Success("Remote service healthy again!"), logs);

        return Ok(new PatternExecutionReport
        {
            PatternName = "Circuit Breaker",
            Category = "Modern Enterprise",
            Intent = "Prevent an application from repeatedly trying to execute an operation that's likely to fail, allowing it to continue without consuming resources while the fault is being fixed.",
            RealWorldScenario = "Resilient HTTP client communication (Polly) preventing thread starvation during cloud dependency outages.",
            ExecutionSteps = logs,
            ResultData = new
            {
                FastFailError = fastFail.Error.Message,
                RecoveredOutput = recovered.ValueOrDefault,
                FinalCircuitState = breaker.State.ToString()
            }
        });
    }

    /// <summary>
    /// Demonstrates the modern ASP.NET Core Options Pattern with data annotation validation.
    /// </summary>
    [HttpPost("options")]
    [ProducesResponseType(typeof(PatternExecutionReport), StatusCodes.Status200OK)]
    public IActionResult RunOptionsDemo([FromQuery] string containerName = "production-assets", [FromQuery] int maxUploads = 25)
    {
        var logs = new List<string>();

        var options = new StorageOptions
        {
            DefaultContainerName = containerName,
            MaxConcurrentUploads = maxUploads,
            EnableCompression = true
        };

        var service = new StorageServiceWithValidatedOptions(options, logs);
        var result = service.InitializeStorage();

        return Ok(new PatternExecutionReport
        {
            PatternName = "Options Pattern",
            Category = "Modern Enterprise",
            Intent = "Provide strongly-typed access to related configuration settings groups with validation and support for real-time reload notifications.",
            RealWorldScenario = "Validating complex database, cloud storage, and security configurations on application startup.",
            ExecutionSteps = logs,
            ResultData = new { result.IsSuccess, Output = result.ValueOrDefault, ConfiguredOptions = options }
        });
    }
}
