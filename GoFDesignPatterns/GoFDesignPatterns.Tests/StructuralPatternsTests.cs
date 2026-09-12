using GoFDesignPatterns.Core;
using GoFDesignPatterns.Structural;

namespace GoFDesignPatterns.Tests;

public class StructuralPatternsTests
{
    [Fact]
    public void Adapter_LegacyXmlBankingAdapter_ShouldParseResponseIntoDomainModel()
    {
        // Arrange
        IPaymentGateway gateway = new LegacyBankingAdapter(new LegacyXmlBankingSystem());

        // Act
        var result = gateway.ProcessPayment("ACC-7788", 250.00m, "USD");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(250.00m, result.Value.Amount);
        Assert.Equal("Completed", result.Value.Status);
    }

    [Fact]
    public void Bridge_UrgentSlackNotification_ShouldFormatCriticalAlert()
    {
        // Arrange
        Notification notification = new UrgentNotification(new SlackMessageChannel());

        // Act
        var output = notification.Send("incident-room", "Outage Alert", "API is down");

        // Assert
        Assert.Contains("CRITICAL ALERT", output);
        Assert.Contains("Slack -> #incident-room", output);
    }

    [Fact]
    public void Composite_ProductBundle_ShouldRecursivelyCalculateDiscountedPrice()
    {
        // Arrange
        var masterBundle = new ProductBundle("Master Bundle", discountPercentage: 10m);
        masterBundle.Add(new ProductItem("Item 1", 100m));
        
        var subBundle = new ProductBundle("Sub Bundle", discountPercentage: 20m);
        subBundle.Add(new ProductItem("Item 2", 100m)); // Sub bundle price = 80
        masterBundle.Add(subBundle);

        // Act: Total = (100 + 80) = 180 - 10% = 162
        var price = masterBundle.GetPrice();

        // Assert
        Assert.Equal(162m, price);
    }

    [Fact]
    public void Decorator_CachingDecorator_ShouldReturnCachedOrderOnSecondCall()
    {
        // Arrange
        var logs = new List<string>();
        IOrderService service = new OrderService();
        service = new CachingOrderServiceDecorator(service, logs);
        var orderId = Guid.Parse("11111111-1111-1111-1111-111111111111");

        // Act
        var first = service.GetOrderById(orderId);
        var second = service.GetOrderById(orderId);

        // Assert
        Assert.True(first.IsSuccess);
        Assert.True(second.IsSuccess);
        Assert.Contains(logs, l => l.Contains("[CACHE MISS]"));
        Assert.Contains(logs, l => l.Contains("[CACHE HIT]"));
    }

    [Fact]
    public void Facade_OrderCheckoutFacade_ShouldCoordinateSubsystemsSuccessfully()
    {
        // Arrange
        var facade = new OrderCheckoutFacade(new InventoryService(), new PaymentProcessingService(), new ShippingService(), new CustomerNotificationService());
        var request = new CheckoutRequest("C1", "c1@test.com", "Address 1", "SKU-A", 1, 100m);

        // Act
        var (result, logs) = facade.PlaceOrder(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.StartsWith("ORD-", result.Value.OrderId);
        Assert.Contains(logs, l => l.Contains("processed successfully"));
    }

    [Fact]
    public void Flyweight_MarkerFactory_ShouldReuseIdenticalIcons()
    {
        // Arrange
        var factory = new MapMarkerFactory();

        // Act
        var icon1 = factory.GetMarkerIcon("Hospital", "hosp.png", "Red");
        var icon2 = factory.GetMarkerIcon("Hospital", "hosp.png", "Red");

        // Assert
        Assert.Same(icon1, icon2);
        Assert.Equal(1, factory.TotalSharedFlyweightsCount);
    }

    [Fact]
    public void Proxy_SecuredReportProxy_ShouldDenyUnauthorizedRoleAndAllowAdmin()
    {
        // Arrange
        var logs = new List<string>();
        IReportService proxy = new SecuredReportProxy(logs);

        // Act
        var denied = proxy.GenerateFinancialReport("R1", "Guest");
        var allowed = proxy.GenerateFinancialReport("R1", "Admin");

        // Assert
        Assert.True(denied.IsFailure);
        Assert.Equal("Unauthorized", denied.Error.Code);
        Assert.True(allowed.IsSuccess);
        Assert.Contains("Total Revenue", allowed.Value);
    }
}
