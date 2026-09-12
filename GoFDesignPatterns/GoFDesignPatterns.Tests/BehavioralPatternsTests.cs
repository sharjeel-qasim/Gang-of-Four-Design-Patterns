using GoFDesignPatterns.Behavioral;
using GoFDesignPatterns.Core;

namespace GoFDesignPatterns.Tests;

public class BehavioralPatternsTests
{
    [Fact]
    public void ChainOfResponsibility_ShouldEscalateAboveTeamLeadLimit()
    {
        // Arrange
        var teamLead = new TeamLeadApprover();
        var director = new DepartmentDirectorApprover();
        teamLead.SetNext(director);
        var logs = new List<string>();

        // Act
        var reqLow = new PurchaseRequest("R1", "Office Supplies", 500m, "Dev");
        var resLow = teamLead.ProcessRequest(reqLow, logs);

        var reqHigh = new PurchaseRequest("R2", "Server Cluster", 15000m, "Dev");
        var resHigh = teamLead.ProcessRequest(reqHigh, logs);

        // Assert
        Assert.Equal("Team Lead", resLow.Value.ApprovedBy);
        Assert.Equal("Department Director", resHigh.Value.ApprovedBy);
    }

    [Fact]
    public void Command_TransactionManager_ShouldUndoLastDeposit()
    {
        // Arrange
        var account = new BankAccount("AC-100", 500m);
        var manager = new TransactionManager();

        // Act
        manager.ExecuteTransaction(new DepositCommand(account, 200m));
        Assert.Equal(700m, account.Balance);

        manager.UndoLastTransaction();

        // Assert
        Assert.Equal(500m, account.Balance);
    }

    [Fact]
    public void Interpreter_ShouldFilterCatalogByCompositeExpression()
    {
        // Arrange
        List<ProductCatalogItem> catalog =
        [
            new("S1", "Keyboard", "Tech", 100m, true),
            new("S2", "Desk", "Furniture", 200m, true),
            new("S3", "Mouse", "Tech", 50m, false) // Out of stock
        ];

        var rule = new AndExpression(
            new CategoryEqualsExpression("Tech"),
            new InStockExpression()
        );

        // Act
        var matches = ProductRuleEvaluator.Filter(catalog, rule, []);

        // Assert
        Assert.Single(matches);
        Assert.Equal("Keyboard", matches.First().Name);
    }

    [Fact]
    public void Iterator_ShouldStreamAllPagesTransparently()
    {
        // Arrange
        var directory = new RemoteUserDirectory();
        var collection = new PaginatedUserCollection(directory, pageSize: 2);

        // Act
        var users = collection.ToList();

        // Assert
        Assert.Equal(6, users.Count);
        Assert.Equal("Alice Morgan", users[0].Name);
    }

    [Fact]
    public async Task Mediator_ShouldDispatchCommandToHandler()
    {
        // Arrange
        var mediator = new InMemoryMediator();
        mediator.RegisterHandler(new RegisterCustomerCommandHandler([]));

        // Act
        var result = await mediator.Send(new RegisterCustomerCommand("Clark Kent", "clark@dailyplanet.com", "VIP"));

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("$50 Welcome Credit", result.Value.WelcomeGift);
    }

    [Fact]
    public void Memento_ShouldRestoreShoppingCartToPriorSnapshot()
    {
        // Arrange
        var cart = new ShoppingCartOriginator();
        var caretaker = new CartCaretaker();

        cart.AddItem("P1", "Pen", 2, 5m); // $10
        caretaker.SaveCheckpoint(cart, "Checkpoint 1");

        cart.AddItem("P2", "Notebook", 1, 20m); // $30 total
        Assert.Equal(30m, cart.TotalAmount);

        // Act
        var undoResult = caretaker.Undo(cart);

        // Assert
        Assert.True(undoResult.IsSuccess);
        Assert.Equal(10m, cart.TotalAmount);
        Assert.Single(cart.Items);
    }

    [Fact]
    public void Observer_StockMarketTicker_ShouldNotifyAlgorithmicTraderOnDrop()
    {
        // Arrange
        var ticker = new StockMarketTicker();
        var logs = new List<string>();
        using var sub = ticker.Subscribe(new AlgorithmicTraderObserver("Alpha", 5.0m, logs));

        // Act
        ticker.UpdateStockPrice("GOOG", 100m);
        ticker.UpdateStockPrice("GOOG", 90m); // 10% drop -> triggers buy signal

        // Assert
        Assert.Contains(logs, l => l.Contains("BUY SIGNAL triggered for GOOG"));
    }

    [Fact]
    public void State_OrderFulfillment_ShouldEnforceOrderLifecycle()
    {
        // Arrange
        var order = new OrderFulfillmentContext("O-1", 100m);

        // Act & Assert
        Assert.True(order.Pay().IsSuccess);
        Assert.True(order.Ship().IsSuccess);
        Assert.True(order.Deliver().IsSuccess);

        // Cannot cancel already delivered order
        var cancelAttempt = order.Cancel();
        Assert.True(cancelAttempt.IsFailure);
        Assert.Equal("InvalidState", cancelAttempt.Error.Code);
    }

    [Fact]
    public void Strategy_OrderPricingCalculator_ShouldApplySelectedAlgorithm()
    {
        // Arrange
        var customer = new Customer(Guid.NewGuid(), "Jane", "jane@test.com", "VIP");
        var calc = new OrderPricingCalculator(new PercentageDiscountStrategy(20m));

        // Act
        var (discount, total, _) = calc.Calculate(customer, 200m);

        // Assert
        Assert.Equal(40m, discount);
        Assert.Equal(160m, total);
    }

    [Fact]
    public void TemplateMethod_CsvIngestion_ShouldExecuteWorkflowSteps()
    {
        // Arrange
        var pipeline = new CsvDataIngestionPipeline();
        var csv = "Id,Name,Value\nREC-1,Licensing,300.00";
        var logs = new List<string>();

        // Act
        var result = pipeline.ExecutePipeline(csv, logs);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value);
        Assert.Equal("Licensing", result.Value[0].Name);
    }

    [Fact]
    public void Visitor_TechnicalDocument_ShouldExportBothMarkdownAndHtml()
    {
        // Arrange
        var doc = new TechnicalDocument();
        doc.AddElement(new HeadingElement(1, "Title"));
        doc.AddElement(new ParagraphElement("Body text"));

        // Act
        var md = doc.ExportWith(new MarkdownExportVisitor());
        var html = doc.ExportWith(new HtmlExportVisitor());

        // Assert
        Assert.Contains("# Title", md);
        Assert.Contains("<h1>Title</h1>", html);
        Assert.Contains("<p>Body text</p>", html);
    }
}
