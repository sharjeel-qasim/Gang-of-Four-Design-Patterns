using GoFDesignPatterns.Behavioral;
using GoFDesignPatterns.Core;
using GoFDesignPatterns.Creational;
using GoFDesignPatterns.ModernEnterprise;
using GoFDesignPatterns.Structural;

Console.OutputEncoding = System.Text.Encoding.UTF8;

PrintBanner();

if (args.Length > 0 && args[0].Equals("all", StringComparison.OrdinalIgnoreCase))
{
    RunAllPatterns();
    return;
}

while (true)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("\n===============================================================");
    Console.WriteLine("  SELECT A CATEGORY OR OPTION:");
    Console.WriteLine("===============================================================");
    Console.ResetColor();
    Console.WriteLine("  1. Creational Patterns       (Singleton, Factory, Builder...)");
    Console.WriteLine("  2. Structural Patterns       (Adapter, Decorator, Facade...)");
    Console.WriteLine("  3. Behavioral Patterns       (Chain, Command, Mediator...)");
    Console.WriteLine("  4. Modern Enterprise Patterns (CQRS, Specification, Outbox...)");
    Console.WriteLine("  5. Run ALL Patterns Demo");
    Console.WriteLine("  0. Exit");
    Console.Write("\nEnter choice [0-5]: ");

    var choice = Console.ReadLine()?.Trim();
    switch (choice)
    {
        case "1": RunCreationalMenu(); break;
        case "2": RunStructuralMenu(); break;
        case "3": RunBehavioralMenu(); break;
        case "4": RunModernEnterpriseMenu(); break;
        case "5": RunAllPatterns(); break;
        case "0":
            Console.WriteLine("\nExiting. Happy coding with Design Patterns!");
            return;
        default:
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid option. Please enter a number between 0 and 5.");
            Console.ResetColor();
            break;
    }
}

static void PrintBanner()
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine(@"
   ______            ______            ___             ____         __  __                         
  / ____/___ _____  / ____/____  _____/   |  ____     / __ \___  __/ /_/ /____  _________  _____
 / / __/ __ `/ __ \/ /_  / __ \/ ___/ /| | / __ \   / /_/ / _ \/ __/ __/ __ \/ ___/ __ \/ ___/
/ /_/ / /_/ / / / / __/ / /_/ / /  / ___ |/ / / /  / ____/  __/ /_/ /_/  __/ /  / / / /__  ) 
\____/\__,_/_/ /_/_/    \____/_/  /_/  |_/_/ /_/  /_/    \___/\__/\__/\___/_/  /_/ /_/____/  
");
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("  .NET 8 LTS | C# 12 | Senior-Level Interactive Architecture Showcase");
    Console.ResetColor();
}

static void PrintReport(PatternExecutionReport report)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"\n[PATTERN] {report.PatternName} ({report.Category})");
    Console.ResetColor();

    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.WriteLine($"Intent: {report.Intent}");
    Console.WriteLine($"Senior Scenario: {report.RealWorldScenario}");
    Console.ResetColor();

    Console.WriteLine("\nExecution Trace:");
    foreach (var step in report.ExecutionSteps)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine($"  -> {step}");
    }
    Console.ResetColor();

    if (report.ResultData != null)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"Result: {System.Text.Json.JsonSerializer.Serialize(report.ResultData, new System.Text.Json.JsonSerializerOptions { WriteIndented = false })}");
        Console.ResetColor();
    }
}

static void RunCreationalMenu()
{
    Console.WriteLine("\n--- Creational Patterns ---");
    Console.WriteLine("1. Singleton (ThreadSafe & Lazy)");
    Console.WriteLine("2. Factory Method (Notification Dispatcher)");
    Console.WriteLine("3. Abstract Factory (Cloud Infrastructure)");
    Console.WriteLine("4. Builder (Fluent Step-Builder)");
    Console.WriteLine("5. Prototype (Deep vs Shallow Copy)");
    Console.Write("Select pattern [1-5]: ");

    switch (Console.ReadLine()?.Trim())
    {
        case "1": RunSingleton(); break;
        case "2": RunFactoryMethod(); break;
        case "3": RunAbstractFactory(); break;
        case "4": RunBuilder(); break;
        case "5": RunPrototype(); break;
        default: Console.WriteLine("Invalid selection."); break;
    }
}

static void RunStructuralMenu()
{
    Console.WriteLine("\n--- Structural Patterns ---");
    Console.WriteLine("1. Adapter (Legacy XML Banking)");
    Console.WriteLine("2. Bridge (Notification Channels)");
    Console.WriteLine("3. Composite (Product Bundle Tree)");
    Console.WriteLine("4. Decorator (Service Pipeline Caching/Logging)");
    Console.WriteLine("5. Facade (Checkout Orchestrator)");
    Console.WriteLine("6. Flyweight (Graphic Marker Cache)");
    Console.WriteLine("7. Proxy (Protection & Virtual)");
    Console.Write("Select pattern [1-7]: ");

    switch (Console.ReadLine()?.Trim())
    {
        case "1": RunAdapter(); break;
        case "2": RunBridge(); break;
        case "3": RunComposite(); break;
        case "4": RunDecorator(); break;
        case "5": RunFacade(); break;
        case "6": RunFlyweight(); break;
        case "7": RunProxy(); break;
        default: Console.WriteLine("Invalid selection."); break;
    }
}

static void RunBehavioralMenu()
{
    Console.WriteLine("\n--- Behavioral Patterns ---");
    Console.WriteLine("1.  Chain of Responsibility (Approval Hierarchy)");
    Console.WriteLine("2.  Command (Undoable Bank Transactions)");
    Console.WriteLine("3.  Interpreter (Query Rule Evaluator)");
    Console.WriteLine("4.  Iterator (Paginated API Stream)");
    Console.WriteLine("5.  Mediator (In-Process CQRS)");
    Console.WriteLine("6.  Memento (Cart State Snapshot)");
    Console.WriteLine("7.  Observer (Stock Market Feed)");
    Console.WriteLine("8.  State (Order Fulfillment Machine)");
    Console.WriteLine("9.  Strategy (Dynamic Discount Engine)");
    Console.WriteLine("10. Template Method (ETL Pipeline)");
    Console.WriteLine("11. Visitor (Document Exporter)");
    Console.Write("Select pattern [1-11]: ");

    switch (Console.ReadLine()?.Trim())
    {
        case "1": RunChain(); break;
        case "2": RunCommand(); break;
        case "3": RunInterpreter(); break;
        case "4": RunIterator(); break;
        case "5": RunMediator(); break;
        case "6": RunMemento(); break;
        case "7": RunObserver(); break;
        case "8": RunState(); break;
        case "9": RunStrategy(); break;
        case "10": RunTemplateMethod(); break;
        case "11": RunVisitor(); break;
        default: Console.WriteLine("Invalid selection."); break;
    }
}

static void RunModernEnterpriseMenu()
{
    Console.WriteLine("\n--- Modern Enterprise Patterns ---");
    Console.WriteLine("1. CQRS (Write Model & Read Projection)");
    Console.WriteLine("2. Specification (Composable Rules)");
    Console.WriteLine("3. Result Pattern / ROP (Railway-Oriented)");
    Console.WriteLine("4. Transactional Outbox (Reliable Events)");
    Console.WriteLine("5. Circuit Breaker (Resilient Fault Tolerance)");
    Console.WriteLine("6. Options Pattern (Validated Configuration)");
    Console.Write("Select pattern [1-6]: ");

    switch (Console.ReadLine()?.Trim())
    {
        case "1": RunCQRS(); break;
        case "2": RunSpecification(); break;
        case "3": RunResultROP(); break;
        case "4": RunOutbox(); break;
        case "5": RunCircuitBreaker(); break;
        case "6": RunOptions(); break;
        default: Console.WriteLine("Invalid selection."); break;
    }
}

static void RunAllPatterns()
{
    Console.WriteLine("\n>>> RUNNING FULL PATTERN DEMONSTRATION SUITE <<<\n");
    RunSingleton();
    RunFactoryMethod();
    RunAbstractFactory();
    RunBuilder();
    RunPrototype();
    RunAdapter();
    RunBridge();
    RunComposite();
    RunDecorator();
    RunFacade();
    RunFlyweight();
    RunProxy();
    RunChain();
    RunCommand();
    RunInterpreter();
    RunIterator();
    RunMediator();
    RunMemento();
    RunObserver();
    RunState();
    RunStrategy();
    RunTemplateMethod();
    RunVisitor();
    RunCQRS();
    RunSpecification();
    RunResultROP();
    RunOutbox();
    RunCircuitBreaker();
    RunOptions();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("\n✔ ALL 29 DESIGN PATTERNS EXECUTED SUCCESSFULLY!\n");
    Console.ResetColor();
}

// ---------------- Implementation Run Helpers ----------------

static void RunSingleton()
{
    var s1 = LazySingleton.Instance;
    var s2 = LazySingleton.Instance;
    PrintReport(new PatternExecutionReport
    {
        PatternName = "Singleton",
        Category = "Creational",
        Intent = "Ensure a class has only one instance and provide a global access point to it.",
        RealWorldScenario = "Application-wide state cache or thread-safe connection manager.",
        ExecutionSteps =
        [
            $"Retrieved LazySingleton instance 1: ID = {s1.InstanceId}",
            $"Retrieved LazySingleton instance 2: ID = {s2.InstanceId}",
            $"ReferenceEquals(s1, s2): {ReferenceEquals(s1, s2)}"
        ],
        ResultData = new { SameInstance = ReferenceEquals(s1, s2), s1.InstanceId }
    });
}

static void RunFactoryMethod()
{
    NotificationService service = new SmsNotificationService();
    var result = service.NotifyUser("+15550198234", "Your multi-factor authentication code is 849201.");
    PrintReport(new PatternExecutionReport
    {
        PatternName = "Factory Method",
        Category = "Creational",
        Intent = "Define an interface for creating an object, but let subclasses decide which class to instantiate.",
        RealWorldScenario = "Routing domain events to specialized channel communicators.",
        ExecutionSteps =
        [
            "Created SmsNotificationService",
            "Invoked NotifyUser() which calls internal CreateSender() factory method",
            $"Result: {result.Value}"
        ],
        ResultData = result.Value
    });
}

static void RunAbstractFactory()
{
    var orchestrator = new CloudDeploymentOrchestrator(new AzureInfrastructureFactory());
    var logs = orchestrator.DeployMicroserviceEnvironment("OrderService");
    PrintReport(new PatternExecutionReport
    {
        PatternName = "Abstract Factory",
        Category = "Creational",
        Intent = "Provide an interface for creating families of related or dependent objects without specifying their concrete classes.",
        RealWorldScenario = "Multi-cloud infrastructure provider (Azure vs AWS).",
        ExecutionSteps = logs,
        ResultData = "Azure Microservice Environment Deployed"
    });
}

static void RunBuilder()
{
    var customer = new Customer(Guid.NewGuid(), "Jane Doe", "jane@company.com");
    var invoice = InvoiceBuilder.Create()
        .ForCustomer(customer)
        .AddItem("LIC-001", "Enterprise Cloud License", 5, 200m)
        .WithTaxRate(0.10m)
        .WithDiscount(50m)
        .Build();

    PrintReport(new PatternExecutionReport
    {
        PatternName = "Builder",
        Category = "Creational",
        Intent = "Separate the construction of a complex object from its representation.",
        RealWorldScenario = "Financial Invoice and Complex Order composition with compile-time step guarantees.",
        ExecutionSteps =
        [
            $"ForCustomer: {customer.FullName}",
            "AddItem: 5x Enterprise Cloud License @ $200",
            $"Subtotal: {invoice.Subtotal:C}, Tax: {invoice.TaxAmount:C}, Total: {invoice.TotalAmount:C}"
        ],
        ResultData = invoice
    });
}

static void RunPrototype()
{
    var original = new ServerConfiguration("WebNode-01", 8, 32, new NetworkSettings("10.0.0.1", 80, ["10.0.0.0/16"]), []);
    var clone = original.Clone();
    clone.ServerName = "WebNode-02";
    clone.Network.AllowedSubnets.Add("192.168.1.0/24");

    PrintReport(new PatternExecutionReport
    {
        PatternName = "Prototype",
        Category = "Creational",
        Intent = "Specify the kinds of objects to create using a prototypical instance, and create new objects by copying this prototype.",
        RealWorldScenario = "Deep cloning infrastructure templates avoiding reference leakage.",
        ExecutionSteps =
        [
            $"Original Subnets Count: {original.Network.AllowedSubnets.Count}",
            $"Clone Subnets Count: {clone.Network.AllowedSubnets.Count} (Mutated safely without affecting original)"
        ],
        ResultData = new { original.ServerName, CloneName = clone.ServerName }
    });
}

static void RunAdapter()
{
    IPaymentGateway adapter = new LegacyBankingAdapter(new LegacyXmlBankingSystem());
    var result = adapter.ProcessPayment("ACC-12345", 500m, "USD");
    PrintReport(new PatternExecutionReport
    {
        PatternName = "Adapter",
        Category = "Structural",
        Intent = "Convert the interface of a class into another interface clients expect.",
        RealWorldScenario = "Adapting legacy XML banking system to modern IPaymentGateway.",
        ExecutionSteps =
        [
            "Calling IPaymentGateway.ProcessPayment()",
            "Adapter serialized parameters into <LegacyPaymentRequest> XML",
            "Adapter parsed <LegacyResponse> XML and returned modern PaymentTransaction DTO"
        ],
        ResultData = result.Value
    });
}

static void RunBridge()
{
    Notification notification = new UrgentNotification(new SlackMessageChannel());
    var output = notification.Send("devops", "Disk Full Alert", "Disk /dev/sda1 is at 98% capacity.");
    PrintReport(new PatternExecutionReport
    {
        PatternName = "Bridge",
        Category = "Structural",
        Intent = "Decouple an abstraction from its implementation so that the two can vary independently.",
        RealWorldScenario = "Decoupling Notification urgency from delivery messaging platform.",
        ExecutionSteps = [output],
        ResultData = output
    });
}

static void RunComposite()
{
    var bundle = new ProductBundle("Gaming PC Pack", 10m);
    bundle.Add(new ProductItem("RTX 4090 GPU", 1600m));
    bundle.Add(new ProductItem("Gaming Monitor", 400m));
    var logs = new List<string>();
    bundle.Display(0, logs);
    PrintReport(new PatternExecutionReport
    {
        PatternName = "Composite",
        Category = "Structural",
        Intent = "Compose objects into tree structures to represent part-whole hierarchies.",
        RealWorldScenario = "Hierarchical catalog bundling and recursive discount calculation.",
        ExecutionSteps = logs,
        ResultData = new { CalculatedPrice = bundle.GetPrice() }
    });
}

static void RunDecorator()
{
    var logs = new List<string>();
    IOrderService service = new OrderService();
    service = new LoggingOrderServiceDecorator(service, logs);
    service = new CachingOrderServiceDecorator(service, logs);
    var id = Guid.Parse("11111111-1111-1111-1111-111111111111");
    service.GetOrderById(id);
    service.GetOrderById(id);
    PrintReport(new PatternExecutionReport
    {
        PatternName = "Decorator",
        Category = "Structural",
        Intent = "Attach additional responsibilities to an object dynamically.",
        RealWorldScenario = "Adding Caching and Logging decorators around IOrderService.",
        ExecutionSteps = logs,
        ResultData = "Pipeline executed with Cache Miss then Cache Hit"
    });
}

static void RunFacade()
{
    var facade = new OrderCheckoutFacade(new InventoryService(), new PaymentProcessingService(), new ShippingService(), new CustomerNotificationService());
    var (result, logs) = facade.PlaceOrder(new CheckoutRequest("C-1", "user@domain.com", "123 Main St", "SKU-9", 1, 99.99m));
    PrintReport(new PatternExecutionReport
    {
        PatternName = "Facade",
        Category = "Structural",
        Intent = "Provide a unified interface to a set of interfaces in a subsystem.",
        RealWorldScenario = "Orchestrating checkout across Inventory, Payment, Shipping, and Notifications.",
        ExecutionSteps = logs,
        ResultData = result.Value
    });
}

static void RunFlyweight()
{
    var factory = new MapMarkerFactory();
    var marker1 = new MapMarker(40.7, -74.0, "Hospital #1", factory.GetMarkerIcon("Hospital", "hosp.png", "Red"));
    var marker2 = new MapMarker(40.8, -74.1, "Hospital #2", factory.GetMarkerIcon("Hospital", "hosp.png", "Red"));
    PrintReport(new PatternExecutionReport
    {
        PatternName = "Flyweight",
        Category = "Structural",
        Intent = "Use sharing to support large numbers of fine-grained objects efficiently.",
        RealWorldScenario = "Sharing immutable icon metadata across hundreds of map markers.",
        ExecutionSteps =
        [
            marker1.Draw(),
            marker2.Draw(),
            $"Both markers share identical icon reference: {ReferenceEquals(marker1.Icon, marker2.Icon)}"
        ],
        ResultData = new { factory.TotalSharedFlyweightsCount }
    });
}

static void RunProxy()
{
    var logs = new List<string>();
    var proxy = new SecuredReportProxy(logs);
    var resDenied = proxy.GenerateFinancialReport("REP-01", "Guest");
    var resAllowed = proxy.GenerateFinancialReport("REP-01", "Admin");
    PrintReport(new PatternExecutionReport
    {
        PatternName = "Proxy",
        Category = "Structural",
        Intent = "Provide a surrogate or placeholder for another object to control access to it.",
        RealWorldScenario = "Protection and lazy virtual loading of financial reports.",
        ExecutionSteps = logs,
        ResultData = new { DeniedMessage = resDenied.Error.Message, AllowedOutput = resAllowed.Value }
    });
}

static void RunChain()
{
    var teamLead = new TeamLeadApprover();
    var director = new DepartmentDirectorApprover();
    teamLead.SetNext(director);
    var logs = new List<string>();
    var res = teamLead.ProcessRequest(new PurchaseRequest("PR-1", "Monitor", 5000m, "Dev"), logs);
    PrintReport(new PatternExecutionReport
    {
        PatternName = "Chain of Responsibility",
        Category = "Behavioral",
        Intent = "Pass requests along a chain of handlers.",
        RealWorldScenario = "Expense approval threshold hierarchy.",
        ExecutionSteps = logs,
        ResultData = res.Value
    });
}

static void RunCommand()
{
    var acc = new BankAccount("AC-1", 100m);
    var mgr = new TransactionManager();
    mgr.ExecuteTransaction(new DepositCommand(acc, 50m));
    mgr.UndoLastTransaction();
    PrintReport(new PatternExecutionReport
    {
        PatternName = "Command",
        Category = "Behavioral",
        Intent = "Encapsulate a request as an object with undo capability.",
        RealWorldScenario = "Transactional bank account operations and undo logging.",
        ExecutionSteps = [.. mgr.AuditTrail],
        ResultData = new { acc.Balance }
    });
}

static void RunInterpreter()
{
    var item = new ProductCatalogItem("SKU-1", "Monitor", "Electronics", 250m, true);
    var rule = new AndExpression(new CategoryEqualsExpression("Electronics"), new PriceUnderExpression(300m));
    var logs = new List<string>();
    var matches = ProductRuleEvaluator.Filter([item], rule, logs);
    PrintReport(new PatternExecutionReport
    {
        PatternName = "Interpreter",
        Category = "Behavioral",
        Intent = "Given a language, define a representation for its grammar with an interpreter.",
        RealWorldScenario = "Evaluating dynamic catalog filtering queries.",
        ExecutionSteps = logs,
        ResultData = new { MatchedCount = matches.Count }
    });
}

static void RunIterator()
{
    var coll = new PaginatedUserCollection(new RemoteUserDirectory());
    var users = coll.ToList();
    PrintReport(new PatternExecutionReport
    {
        PatternName = "Iterator",
        Category = "Behavioral",
        Intent = "Access elements of an aggregate object sequentially without exposing internal representation.",
        RealWorldScenario = "Transparently traversing paginated REST API endpoints.",
        ExecutionSteps = users.Select(u => $"Iterated user: {u.Name}").ToList(),
        ResultData = new { TotalUsersLoaded = users.Count }
    });
}

static void RunMediator()
{
    var mediator = new InMemoryMediator();
    var logs = new List<string>();
    mediator.RegisterHandler(new RegisterCustomerCommandHandler(logs));
    var task = mediator.Send(new RegisterCustomerCommand("Diana Prince", "diana@themyscira.gov", "VIP"));
    task.Wait();
    PrintReport(new PatternExecutionReport
    {
        PatternName = "Mediator",
        Category = "Behavioral",
        Intent = "Define an object that encapsulates how a set of objects interact.",
        RealWorldScenario = "MediatR-style CQRS command execution in Clean Architecture.",
        ExecutionSteps = logs,
        ResultData = task.Result.Value
    });
}

static void RunMemento()
{
    var cart = new ShoppingCartOriginator();
    var caretaker = new CartCaretaker();
    cart.AddItem("SKU-1", "Book", 1, 20m);
    caretaker.SaveCheckpoint(cart, "1 Item Added");
    cart.AddItem("SKU-2", "Yacht", 1, 1000000m);
    caretaker.Undo(cart);
    PrintReport(new PatternExecutionReport
    {
        PatternName = "Memento",
        Category = "Behavioral",
        Intent = "Capture and externalize object internal state so it can be restored later.",
        RealWorldScenario = "Shopping cart checkout state checkpointing and rollback.",
        ExecutionSteps = [.. caretaker.HistoryLog],
        ResultData = new { cart.TotalAmount, cart.Items.Count }
    });
}

static void RunObserver()
{
    var ticker = new StockMarketTicker();
    var logs = new List<string>();
    using var sub = ticker.Subscribe(new AlgorithmicTraderObserver("HFT-Bot", 5m, logs));
    ticker.UpdateStockPrice("AAPL", 200m);
    ticker.UpdateStockPrice("AAPL", 185m);
    PrintReport(new PatternExecutionReport
    {
        PatternName = "Observer",
        Category = "Behavioral",
        Intent = "Define a one-to-many dependency so when one object changes state, dependents are notified.",
        RealWorldScenario = "Stock market real-time price feeds notifying algorithmic traders.",
        ExecutionSteps = logs,
        ResultData = "Observer triggered on price drop"
    });
}

static void RunState()
{
    var order = new OrderFulfillmentContext("ORD-1", 100m);
    order.Pay();
    order.Ship();
    order.Deliver();
    PrintReport(new PatternExecutionReport
    {
        PatternName = "State",
        Category = "Behavioral",
        Intent = "Allow an object to alter its behavior when its internal state changes.",
        RealWorldScenario = "Order fulfillment state machine with state-enforced transitions.",
        ExecutionSteps = order.History,
        ResultData = new { CurrentState = order.CurrentState.StateName }
    });
}

static void RunStrategy()
{
    var calc = new OrderPricingCalculator(new ProgressiveThresholdDiscountStrategy());
    var (discount, total, name) = calc.Calculate(new Customer(Guid.NewGuid(), "Sam", "sam@test.com"), 300m);
    PrintReport(new PatternExecutionReport
    {
        PatternName = "Strategy",
        Category = "Behavioral",
        Intent = "Define a family of algorithms, encapsulate each one, and make them interchangeable.",
        RealWorldScenario = "Interchangeable checkout discount calculations.",
        ExecutionSteps =
        [
            $"Applied strategy: {name}",
            $"Original: $300.00 | Discount: {discount:C} | Final: {total:C}"
        ],
        ResultData = new { Total = total, Discount = discount }
    });
}

static void RunTemplateMethod()
{
    var logs = new List<string>();
    var pipeline = new CsvDataIngestionPipeline();
    var result = pipeline.ExecutePipeline("Id,Name,Value\n1,Server,500", logs);
    PrintReport(new PatternExecutionReport
    {
        PatternName = "Template Method",
        Category = "Behavioral",
        Intent = "Define algorithm skeleton in an operation, deferring steps to subclasses.",
        RealWorldScenario = "Data Ingestion (ETL) pipeline with format-specific hooks.",
        ExecutionSteps = logs,
        ResultData = result.Value
    });
}

static void RunVisitor()
{
    var doc = new TechnicalDocument();
    doc.AddElement(new HeadingElement(1, "Modern Design Patterns"));
    doc.AddElement(new ParagraphElement("Visitor pattern enables operations on object structures."));
    var md = doc.ExportWith(new MarkdownExportVisitor());
    var html = doc.ExportWith(new HtmlExportVisitor());
    PrintReport(new PatternExecutionReport
    {
        PatternName = "Visitor",
        Category = "Behavioral",
        Intent = "Represent an operation to be performed on the elements of an object structure.",
        RealWorldScenario = "Document and AST exporters rendering into Markdown and HTML.",
        ExecutionSteps =
        [
            "Exported to Markdown:",
            md,
            "Exported to HTML:",
            html
        ],
        ResultData = new { Markdown = md, Html = html }
    });
}

static void RunCQRS()
{
    var store = new ProductStore();
    var logs = new List<string>();
    var cmdHandler = new CreateProductCommandHandler(store, logs);
    var qryHandler = new GetProductBySkuQueryHandler(store, logs);
    cmdHandler.Handle(new CreateProductCommand("P-1", "Gaming Mouse", 79.99m, 100));
    var queryRes = qryHandler.Handle("P-1");
    PrintReport(new PatternExecutionReport
    {
        PatternName = "CQRS",
        Category = "Modern Enterprise",
        Intent = "Segregate write models and commands from read projections and queries.",
        RealWorldScenario = "E-Commerce high-scale product management.",
        ExecutionSteps = logs,
        ResultData = queryRes.Value
    });
}

static void RunSpecification()
{
    var spec = new PremiumCustomerSpecification().And(new HighValueOrderSpecification(500m));
    var order = new CustomerOrder("ORD-1", "VIP", 800m, false);
    var satisfied = spec.IsSatisfiedBy(order);
    PrintReport(new PatternExecutionReport
    {
        PatternName = "Specification",
        Category = "Modern Enterprise",
        Intent = "Encapsulate business rules into combinable boolean specifications.",
        RealWorldScenario = "Composable enterprise order validation policies.",
        ExecutionSteps =
        [
            "Created Specification: PremiumCustomer AND HighValueOrder(>= $500)",
            $"Evaluating on VIP order with $800 total: IsSatisfiedBy = {satisfied}"
        ],
        ResultData = new { IsSatisfied = satisfied }
    });
}

static void RunResultROP()
{
    var logs = new List<string>();
    var pipeline = new LoanProcessingPipeline(logs);
    var res = pipeline.ProcessApplication(new LoanApplication(Guid.NewGuid(), "Jane", 10000m, 780, 5000m));
    PrintReport(new PatternExecutionReport
    {
        PatternName = "Result Pattern (ROP)",
        Category = "Modern Enterprise",
        Intent = "Chain operations functionally via Bind/Map without throwing exceptions.",
        RealWorldScenario = "Financial underwriting evaluation pipeline.",
        ExecutionSteps = logs,
        ResultData = res.Value
    });
}

static void RunOutbox()
{
    var db = new SimulatedDatabaseContext();
    var logs = new List<string>();
    var svc = new OrderServiceWithOutbox(db, logs);
    var pub = new OutboxMessagePublisher(db, logs);
    svc.PlaceOrder("ORD-001", "VIP", 500m);
    pub.DispatchPendingMessages();
    PrintReport(new PatternExecutionReport
    {
        PatternName = "Transactional Outbox",
        Category = "Modern Enterprise",
        Intent = "Persist domain entities and outbound events in a single transaction to prevent dual-writes.",
        RealWorldScenario = "Event-driven distributed systems messaging.",
        ExecutionSteps = logs,
        ResultData = "Outbox message created and dispatched reliably"
    });
}

static void RunCircuitBreaker()
{
    var logs = new List<string>();
    var breaker = new CircuitBreaker(failureThreshold: 2, resetTimeout: TimeSpan.FromMilliseconds(50));
    breaker.Execute<string>(() => Result<string>.Failure("E1", "Error 1"), logs);
    breaker.Execute<string>(() => Result<string>.Failure("E2", "Error 2"), logs);
    breaker.Execute<string>(() => Result<string>.Success("Tripped"), logs); // Fast fails
    PrintReport(new PatternExecutionReport
    {
        PatternName = "Circuit Breaker",
        Category = "Modern Enterprise",
        Intent = "Prevent cascading failures by failing fast when downstream services degrade.",
        RealWorldScenario = "Resilient cloud microservice communication.",
        ExecutionSteps = logs,
        ResultData = new { FinalState = breaker.State.ToString() }
    });
}

static void RunOptions()
{
    var logs = new List<string>();
    var svc = new StorageServiceWithValidatedOptions(new StorageOptions { DefaultContainerName = "blobs", MaxConcurrentUploads = 15 }, logs);
    var res = svc.InitializeStorage();
    PrintReport(new PatternExecutionReport
    {
        PatternName = "Options Pattern",
        Category = "Modern Enterprise",
        Intent = "Provide strongly-typed, validated configuration access in ASP.NET Core.",
        RealWorldScenario = "Startup configuration validation.",
        ExecutionSteps = logs,
        ResultData = res.Value
    });
}
