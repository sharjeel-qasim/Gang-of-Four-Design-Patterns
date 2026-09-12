using GoFDesignPatterns.Behavioral;
using GoFDesignPatterns.Core;
using Microsoft.AspNetCore.Mvc;

namespace GoFDesignPatterns.API.Controllers;

[ApiController]
[Route("api/behavioral")]
[Produces("application/json")]
public class BehavioralPatternsController : ControllerBase
{
    /// <summary>
    /// Demonstrates Chain of Responsibility by evaluating purchase approvals through hierarchical authority levels.
    /// </summary>
    [HttpPost("chain-of-responsibility")]
    [ProducesResponseType(typeof(PatternExecutionReport), StatusCodes.Status200OK)]
    public IActionResult RunChainOfResponsibilityDemo([FromQuery] decimal amount = 15000m)
    {
        var logs = new List<string> { "Setting up approval chain: Team Lead -> Director -> VP -> Board..." };

        var teamLead = new TeamLeadApprover();
        var director = new DepartmentDirectorApprover();
        var vp = new VicePresidentApprover();
        var board = new BoardOfDirectorsApprover();

        teamLead.SetNext(director).SetNext(vp).SetNext(board);

        var request = new PurchaseRequest("REQ-8088", "Kubernetes Server Cluster Upgrade", amount, "Cloud Architect");
        logs.Add($"Submitted purchase request '{request.RequestId}' for {request.Amount:C}");

        var result = teamLead.ProcessRequest(request, logs);

        return Ok(new PatternExecutionReport
        {
            PatternName = "Chain of Responsibility",
            Category = "Behavioral",
            Intent = "Avoid coupling the sender of a request to its receiver by giving more than one object a chance to handle the request.",
            RealWorldScenario = "Corporate purchase request and expense approval hierarchy.",
            ExecutionSteps = logs,
            ResultData = result.ValueOrDefault
        });
    }

    /// <summary>
    /// Demonstrates the Command pattern with transactions supporting execution, rollback (undo), and audit history.
    /// </summary>
    [HttpPost("command")]
    [ProducesResponseType(typeof(PatternExecutionReport), StatusCodes.Status200OK)]
    public IActionResult RunCommandDemo([FromQuery] decimal depositAmount = 500m, [FromQuery] decimal withdrawAmount = 200m)
    {
        var logs = new List<string> { "Initializing Bank Account and TransactionManager..." };
        var account = new BankAccount("ACCT-987654", 1000m);
        var manager = new TransactionManager();

        logs.Add($"Initial Account Balance: {account.Balance:C}");

        var deposit = new DepositCommand(account, depositAmount);
        manager.ExecuteTransaction(deposit);

        var withdraw = new WithdrawCommand(account, withdrawAmount);
        manager.ExecuteTransaction(withdraw);

        logs.Add($"Current Account Balance: {account.Balance:C}");

        logs.Add("Triggering UNDO on the last transaction...");
        manager.UndoLastTransaction();

        logs.Add($"Final Account Balance after undo: {account.Balance:C}");

        return Ok(new PatternExecutionReport
        {
            PatternName = "Command",
            Category = "Behavioral",
            Intent = "Encapsulate a request as an object, thereby letting you parameterize clients with different requests, queue or log requests, and support undoable operations.",
            RealWorldScenario = "Financial transaction journal, undo/redo buffers, and transactional workflow rollback.",
            ExecutionSteps = [.. logs, .. manager.AuditTrail],
            ResultData = new { account.AccountNumber, FinalBalance = account.Balance }
        });
    }

    /// <summary>
    /// Demonstrates the Interpreter pattern by evaluating boolean query rules against a product catalog.
    /// </summary>
    [HttpPost("interpreter")]
    [ProducesResponseType(typeof(PatternExecutionReport), StatusCodes.Status200OK)]
    public IActionResult RunInterpreterDemo([FromQuery] string category = "Electronics", [FromQuery] decimal maxPrice = 500m)
    {
        var logs = new List<string> { "Populating domain product catalog..." };

        List<ProductCatalogItem> catalog =
        [
            new("SKU-1", "Noise Cancelling Headphones", "Electronics", 299.99m, true),
            new("SKU-2", "OLED Smart TV", "Electronics", 1299.99m, true),
            new("SKU-3", "Wireless Ergonomic Mouse", "Electronics", 49.99m, false), // Out of stock
            new("SKU-4", "Ergonomic Office Chair", "Furniture", 349.99m, true),
            new("SKU-5", "Bluetooth Speaker", "Electronics", 89.99m, true)
        ];

        // Compose AST (Abstract Syntax Tree): (Category == requested AND Price <= maxPrice AND InStock == true)
        IProductExpression rule = new AndExpression(
            new AndExpression(
                new CategoryEqualsExpression(category),
                new PriceUnderExpression(maxPrice)
            ),
            new InStockExpression()
        );

        var matches = ProductRuleEvaluator.Filter(catalog, rule, logs);

        return Ok(new PatternExecutionReport
        {
            PatternName = "Interpreter",
            Category = "Behavioral",
            Intent = "Given a language, define a representation for its grammar along with an interpreter that uses the representation to interpret sentences in the language.",
            RealWorldScenario = "Dynamic SQL-like rule filter engine evaluating composite conditions on catalog collections.",
            ExecutionSteps = logs,
            ResultData = new { FilterRule = rule.Describe(), Matches = matches }
        });
    }

    /// <summary>
    /// Demonstrates the Iterator pattern traversing a paginated remote source using IEnumerable and IAsyncEnumerable.
    /// </summary>
    [HttpGet("iterator")]
    [ProducesResponseType(typeof(PatternExecutionReport), StatusCodes.Status200OK)]
    public async Task<IActionResult> RunIteratorDemo()
    {
        var logs = new List<string> { "Initializing remote paginated user directory..." };
        var directory = new RemoteUserDirectory();
        var collection = new PaginatedUserCollection(directory, pageSize: 2);

        logs.Add("Consuming PaginatedUserCollection asynchronously with 'await foreach'...");
        var users = new List<ApiUser>();

        await foreach (var user in collection.StreamUsersAsync())
        {
            users.Add(user);
            logs.Add($"Iterated User #{user.Id}: {user.Name} ({user.Email})");
        }

        logs.Add($"Completed streaming {users.Count} user(s) across paginated API batches.");

        return Ok(new PatternExecutionReport
        {
            PatternName = "Iterator",
            Category = "Behavioral",
            Intent = "Provide a way to access the elements of an aggregate object sequentially without exposing its underlying representation.",
            RealWorldScenario = "Streaming paginated 3rd-party REST API batches into an asynchronous pipeline via IAsyncEnumerable<T>.",
            ExecutionSteps = logs,
            ResultData = users
        });
    }

    /// <summary>
    /// Demonstrates the Mediator pattern using an in-process CQRS request handler (MediatR style).
    /// </summary>
    [HttpPost("mediator")]
    [ProducesResponseType(typeof(PatternExecutionReport), StatusCodes.Status200OK)]
    public async Task<IActionResult> RunMediatorDemo([FromQuery] string name = "John Wick", [FromQuery] string email = "j.wick@continental.com", [FromQuery] string tier = "VIP")
    {
        var logs = new List<string> { "Setting up in-process CQRS Mediator and registering handlers..." };
        var mediator = new InMemoryMediator();
        mediator.RegisterHandler(new RegisterCustomerCommandHandler(logs));

        var command = new RegisterCustomerCommand(name, email, tier);
        logs.Add($"Sending {nameof(RegisterCustomerCommand)} through Mediator...");

        var result = await mediator.Send(command);

        return Ok(new PatternExecutionReport
        {
            PatternName = "Mediator",
            Category = "Behavioral",
            Intent = "Define an object that encapsulates how a set of objects interact. Mediator promotes loose coupling by keeping objects from referring to each other explicitly.",
            RealWorldScenario = "MediatR-style CQRS command and query dispatching in ASP.NET Core applications.",
            ExecutionSteps = logs,
            ResultData = result.ValueOrDefault
        });
    }

    /// <summary>
    /// Demonstrates the Memento pattern taking shopping cart snapshots and supporting checkpoint rollbacks.
    /// </summary>
    [HttpPost("memento")]
    [ProducesResponseType(typeof(PatternExecutionReport), StatusCodes.Status200OK)]
    public IActionResult RunMementoDemo()
    {
        var logs = new List<string> { "Creating Cart Originator and Caretaker..." };
        var cart = new ShoppingCartOriginator();
        var caretaker = new CartCaretaker();

        cart.AddItem("SKU-1", "Mechanical Keyboard", 1, 120.00m);
        cart.AddItem("SKU-2", "Wireless Mouse", 1, 60.00m);
        caretaker.SaveCheckpoint(cart, "Baseline Tech Accessories");

        logs.Add($"Checkpoint 1 created. Total: {cart.TotalAmount:C}");

        // Add accidental expensive item
        cart.AddItem("SKU-99", "Gold-Plated HDMI Cable", 5, 500.00m);
        logs.Add($"Accidentally added expensive items. New Total: {cart.TotalAmount:C}");

        // Undo to checkpoint
        logs.Add("Rolling back cart to previous checkpoint...");
        var undoResult = caretaker.Undo(cart);

        logs.Add($"Restored Total: {cart.TotalAmount:C} (Item count: {cart.Items.Count})");

        return Ok(new PatternExecutionReport
        {
            PatternName = "Memento",
            Category = "Behavioral",
            Intent = "Without violating encapsulation, capture and externalize an object's internal state so that the object can be restored to this state later.",
            RealWorldScenario = "Shopping cart checkout state checkpointing and multi-step form rollback.",
            ExecutionSteps = [.. logs, .. caretaker.HistoryLog],
            ResultData = new { cart.TotalAmount, Items = cart.Items }
        });
    }

    /// <summary>
    /// Demonstrates the Observer pattern broadcasting stock price changes to algorithmic traders and risk auditors.
    /// </summary>
    [HttpPost("observer")]
    [ProducesResponseType(typeof(PatternExecutionReport), StatusCodes.Status200OK)]
    public IActionResult RunObserverDemo([FromQuery] string symbol = "MSFT", [FromQuery] decimal newPrice = 380.00m)
    {
        var logs = new List<string> { "Setting up StockMarketTicker Subject and subscribing observers..." };

        var ticker = new StockMarketTicker();
        var trader = new AlgorithmicTraderObserver("AlphaQuant", buyThresholdDropPercentage: 5.0m, logs);
        var risk = new RiskManagementObserver(logs);

        using var sub1 = ticker.Subscribe(trader);
        using var sub2 = ticker.Subscribe(risk);

        logs.Add($"Emitting baseline price for {symbol} at $420.00...");
        ticker.UpdateStockPrice(symbol, 420.00m);

        logs.Add($"Emitting new price drop for {symbol} to {newPrice:C}...");
        ticker.UpdateStockPrice(symbol, newPrice);

        return Ok(new PatternExecutionReport
        {
            PatternName = "Observer",
            Category = "Behavioral",
            Intent = "Define a one-to-many dependency between objects so that when one object changes state, all its dependents are notified and updated automatically.",
            RealWorldScenario = "Stock market real-time price feeds broadcasting to automated algorithmic trading desks and risk audit systems.",
            ExecutionSteps = logs,
            ResultData = new { Symbol = symbol, CurrentPrice = newPrice }
        });
    }

    /// <summary>
    /// Demonstrates the State pattern enforcing order fulfillment lifecycle transitions.
    /// </summary>
    [HttpPost("state")]
    [ProducesResponseType(typeof(PatternExecutionReport), StatusCodes.Status200OK)]
    public IActionResult RunStateDemo()
    {
        var order = new OrderFulfillmentContext("ORD-7766", 299.99m);

        // Valid transition: Pay
        var payResult = order.Pay();

        // Invalid transition: Try to Pay again
        var doublePayResult = order.Pay();

        // Valid transition: Ship
        var shipResult = order.Ship();

        // Valid transition: Deliver
        var deliverResult = order.Deliver();

        // Invalid transition: Try to cancel delivered order
        var cancelResult = order.Cancel();

        return Ok(new PatternExecutionReport
        {
            PatternName = "State",
            Category = "Behavioral",
            Intent = "Allow an object to alter its behavior when its internal state changes. The object will appear to change its class.",
            RealWorldScenario = "Order fulfillment lifecycle (Created -> Paid -> Shipped -> Delivered / Cancelled) with state-enforced business rules.",
            ExecutionSteps = order.History,
            ResultData = new
            {
                FinalState = order.CurrentState.StateName,
                PayResult = payResult.ValueOrDefault,
                DoublePayRejection = doublePayResult.Error.Message,
                ShipResult = shipResult.ValueOrDefault,
                DeliverResult = deliverResult.ValueOrDefault,
                CancelDeliveredRejection = cancelResult.Error.Message
            }
        });
    }

    /// <summary>
    /// Demonstrates the Strategy pattern dynamically swapping pricing and discount calculation strategies.
    /// </summary>
    [HttpPost("strategy")]
    [ProducesResponseType(typeof(PatternExecutionReport), StatusCodes.Status200OK)]
    public IActionResult RunStrategyDemo([FromQuery] string strategyType = "progressive", [FromQuery] decimal cartTotal = 350.00m)
    {
        var logs = new List<string> { "Configuring OrderPricingCalculator context..." };
        var customer = new Customer(Guid.NewGuid(), "Robert Vance", "robert@vance-refrig.com", "Gold");

        IDiscountStrategy strategy = strategyType.ToLowerInvariant() switch
        {
            "vip" => new VipTierDiscountStrategy(),
            "percentage" => new PercentageDiscountStrategy(15m),
            "fixed" => new FixedAmountDiscountStrategy(50m),
            _ => new ProgressiveThresholdDiscountStrategy()
        };

        var calculator = new OrderPricingCalculator(strategy);
        logs.Add($"Applied Strategy: {calculator.CurrentStrategy.StrategyName}");

        var (discount, finalTotal, strategyUsed) = calculator.Calculate(customer, cartTotal);
        logs.Add($"Cart Subtotal: {cartTotal:C} | Discount Applied: {discount:C} | Final Payable: {finalTotal:C}");

        return Ok(new PatternExecutionReport
        {
            PatternName = "Strategy",
            Category = "Behavioral",
            Intent = "Define a family of algorithms, encapsulate each one, and make them interchangeable. Strategy lets the algorithm vary independently from clients that use it.",
            RealWorldScenario = "Dynamic e-commerce discount calculators based on promotional campaigns, cart totals, and VIP tiers.",
            ExecutionSteps = logs,
            ResultData = new { Subtotal = cartTotal, Discount = discount, FinalTotal = finalTotal, Strategy = strategyUsed }
        });
    }

    /// <summary>
    /// Demonstrates the Template Method pattern executing an ETL data ingestion pipeline with format hooks.
    /// </summary>
    [HttpPost("template-method")]
    [ProducesResponseType(typeof(PatternExecutionReport), StatusCodes.Status200OK)]
    public IActionResult RunTemplateMethodDemo([FromQuery] string format = "csv")
    {
        var logs = new List<string>();

        DataIngestionPipeline pipeline = format.ToLowerInvariant() == "json"
            ? new JsonDataIngestionPipeline()
            : new CsvDataIngestionPipeline();

        var sampleCsv = "Id,Name,Value\nREC-001,Azure Cloud Hosting,1250.00\nREC-002,GitHub Enterprise,450.00\nREC-003,Monitoring Service,200.00";
        var result = pipeline.ExecutePipeline(sampleCsv, logs);

        return Ok(new PatternExecutionReport
        {
            PatternName = "Template Method",
            Category = "Behavioral",
            Intent = "Define the skeleton of an algorithm in an operation, deferring some steps to subclasses. Template Method lets subclasses redefine certain steps of an algorithm without changing the algorithm's structure.",
            RealWorldScenario = "Data Ingestion (ETL) pipeline with invariant sequence (Extract -> Validate -> Transform -> Load) and format-specific hooks.",
            ExecutionSteps = logs,
            ResultData = result.ValueOrDefault
        });
    }

    /// <summary>
    /// Demonstrates the Visitor pattern exporting document elements into Markdown and HTML formats.
    /// </summary>
    [HttpPost("visitor")]
    [ProducesResponseType(typeof(PatternExecutionReport), StatusCodes.Status200OK)]
    public IActionResult RunVisitorDemo()
    {
        var logs = new List<string> { "Constructing technical document with Heading, Paragraph, and CodeBlock elements..." };

        var doc = new TechnicalDocument();
        doc.AddElement(new HeadingElement(1, "Design Patterns in .NET 8"));
        doc.AddElement(new ParagraphElement("The Visitor pattern allows adding new operations to existing object structures without modifying their classes."));
        doc.AddElement(new CodeBlockElement("csharp", "public void Accept(IDocumentVisitor visitor) => visitor.Visit(this);"));

        var markdownVisitor = new MarkdownExportVisitor();
        var markdownOutput = doc.ExportWith(markdownVisitor);
        logs.Add("Exported document to Markdown format.");

        var htmlVisitor = new HtmlExportVisitor();
        var htmlOutput = doc.ExportWith(htmlVisitor);
        logs.Add("Exported document to HTML format.");

        return Ok(new PatternExecutionReport
        {
            PatternName = "Visitor",
            Category = "Behavioral",
            Intent = "Represent an operation to be performed on the elements of an object structure. Visitor lets you define a new operation without changing the classes of the elements on which it operates.",
            RealWorldScenario = "Document and Abstract Syntax Tree (AST) exporters rendering into Markdown, HTML, and PDF without changing domain element classes.",
            ExecutionSteps = logs,
            ResultData = new { Markdown = markdownOutput, Html = htmlOutput }
        });
    }
}
