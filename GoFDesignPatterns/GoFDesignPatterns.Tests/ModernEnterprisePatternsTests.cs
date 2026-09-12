using GoFDesignPatterns.Core;
using GoFDesignPatterns.ModernEnterprise;

namespace GoFDesignPatterns.Tests;

public class ModernEnterprisePatternsTests
{
    [Fact]
    public void CQRS_CommandShouldUpdateWriteStoreAndSynchronizeReadProjection()
    {
        // Arrange
        var store = new ProductStore();
        var cmd = new CreateProductCommandHandler(store, []);
        var qry = new GetProductBySkuQueryHandler(store, []);

        // Act
        var createResult = cmd.Handle(new CreateProductCommand("SKU-99", "Ergo Chair", 450m, 10));
        var projectionResult = qry.Handle("SKU-99");

        // Assert
        Assert.True(createResult.IsSuccess);
        Assert.True(projectionResult.IsSuccess);
        Assert.Equal("Ergo Chair", projectionResult.Value.Name);
        Assert.True(projectionResult.Value.IsAvailable);
    }

    [Fact]
    public void Specification_ShouldCombineRulesUsingBooleanLogic()
    {
        // Arrange
        var isVip = new PremiumCustomerSpecification();
        var isLarge = new HighValueOrderSpecification(1000m);
        var rule = isVip.And(isLarge);

        var matchingOrder = new CustomerOrder("O-1", "VIP", 1500m, false);
        var nonMatchingOrder = new CustomerOrder("O-2", "Standard", 1500m, false);

        // Act & Assert
        Assert.True(rule.IsSatisfiedBy(matchingOrder));
        Assert.False(rule.IsSatisfiedBy(nonMatchingOrder));
    }

    [Fact]
    public void ResultROP_LoanApplication_ShouldApproveValidAndRejectLowCredit()
    {
        // Arrange
        var pipeline = new LoanProcessingPipeline([]);
        var goodApp = new LoanApplication(Guid.NewGuid(), "Alice", 10000m, 750, 6000m);
        var badApp = new LoanApplication(Guid.NewGuid(), "Bob", 10000m, 500, 6000m);

        // Act
        var goodResult = pipeline.ProcessApplication(goodApp);
        var badResult = pipeline.ProcessApplication(badApp);

        // Assert
        Assert.True(goodResult.IsSuccess);
        Assert.True(badResult.IsFailure);
        Assert.Equal("CreditScoreTooLow", badResult.Error.Code);
    }

    [Fact]
    public void TransactionalOutbox_ShouldPersistOrderAndDispatchEvent()
    {
        // Arrange
        var db = new SimulatedDatabaseContext();
        var service = new OrderServiceWithOutbox(db, []);
        var publisher = new OutboxMessagePublisher(db, []);

        // Act
        var orderResult = service.PlaceOrder("ORD-55", "Gold", 300m);
        Assert.True(orderResult.IsSuccess);
        Assert.Single(db.Orders);
        Assert.Single(db.OutboxMessages);
        Assert.False(db.OutboxMessages[0].IsProcessed);

        var dispatched = publisher.DispatchPendingMessages();

        // Assert
        Assert.Equal(1, dispatched);
        Assert.True(db.OutboxMessages[0].IsProcessed);
    }

    [Fact]
    public void CircuitBreaker_ShouldTripToOpenAfterFailureThreshold()
    {
        // Arrange
        var breaker = new CircuitBreaker(failureThreshold: 2, resetTimeout: TimeSpan.FromMilliseconds(100));
        var logs = new List<string>();

        // Act
        breaker.Execute<string>(() => Result<string>.Failure("E1", "Fail 1"), logs);
        breaker.Execute<string>(() => Result<string>.Failure("E2", "Fail 2"), logs);

        // Third call should fail fast
        var fastFail = breaker.Execute<string>(() => Result<string>.Success("Not run"), logs);

        // Assert
        Assert.Equal(CircuitState.Open, breaker.State);
        Assert.True(fastFail.IsFailure);
        Assert.Equal("CircuitOpen", fastFail.Error.Code);
    }

    [Fact]
    public void OptionsPattern_ShouldValidateCorrectProperties()
    {
        // Arrange
        var validOptions = new StorageOptions { DefaultContainerName = "valid-container", MaxConcurrentUploads = 5 };
        var invalidOptions = new StorageOptions { DefaultContainerName = "", MaxConcurrentUploads = 999 };

        // Act
        var validSvc = new StorageServiceWithValidatedOptions(validOptions, []);
        var invalidSvc = new StorageServiceWithValidatedOptions(invalidOptions, []);

        // Assert
        Assert.True(validSvc.InitializeStorage().IsSuccess);
        Assert.True(invalidSvc.InitializeStorage().IsFailure);
    }
}
