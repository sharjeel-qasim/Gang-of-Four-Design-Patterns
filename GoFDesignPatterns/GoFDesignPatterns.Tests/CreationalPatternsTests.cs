using System.Collections.Concurrent;
using GoFDesignPatterns.Core;
using GoFDesignPatterns.Creational;

namespace GoFDesignPatterns.Tests;

public class CreationalPatternsTests
{
    [Fact]
    public void Singleton_LazySingleton_ShouldBeThreadSafeAcrossParallelThreads()
    {
        // Arrange
        const int threadCount = 100;
        var instances = new ConcurrentBag<LazySingleton>();

        // Act
        Parallel.For(0, threadCount, _ =>
        {
            instances.Add(LazySingleton.Instance);
        });

        // Assert
        Assert.Equal(threadCount, instances.Count);
        var firstInstance = instances.First();
        Assert.All(instances, inst => Assert.Same(firstInstance, inst));
    }

    [Fact]
    public void FactoryMethod_SmsNotifier_ShouldFormatAndSendMessage()
    {
        // Arrange
        NotificationService service = new SmsNotificationService();

        // Act
        var result = service.NotifyUser("+12345678901", "Security code: 1234");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Contains("SMS Gateway", result.Value);
    }

    [Fact]
    public void AbstractFactory_AwsAndAzure_ShouldCreateConsistentFamilies()
    {
        // Arrange
        ICloudInfrastructureFactory awsFactory = new AwsInfrastructureFactory();
        ICloudInfrastructureFactory azureFactory = new AzureInfrastructureFactory();

        // Act
        var awsStorage = awsFactory.CreateBlobStorage();
        var azureStorage = azureFactory.CreateBlobStorage();

        // Assert
        Assert.Equal("AWS S3", awsStorage.ProviderName);
        Assert.Equal("Azure Blob Storage", azureStorage.ProviderName);
    }

    [Fact]
    public void Builder_InvoiceBuilder_ShouldCalculateSubtotalTaxAndTotalAccurately()
    {
        // Arrange
        var customer = new Customer(Guid.NewGuid(), "Alice", "alice@test.com");

        // Act
        var invoice = InvoiceBuilder.Create()
            .ForCustomer(customer)
            .AddItem("P1", "Laptop", 1, 1000m)
            .AddItem("P2", "Mouse", 2, 50m) // 1000 + 100 = 1100
            .WithTaxRate(0.10m)
            .WithDiscount(100m) // Discounted subtotal = 1000 -> Tax 10% = 100 -> Total = 1100
            .Build();

        // Assert
        Assert.Equal(1100m, invoice.Subtotal);
        Assert.Equal(100m, invoice.DiscountAmount);
        Assert.Equal(100m, invoice.TaxAmount);
        Assert.Equal(1100m, invoice.TotalAmount);
    }

    [Fact]
    public void Prototype_DeepClone_ShouldBeCompletelyIndependentFromOriginal()
    {
        // Arrange
        var original = new ServerConfiguration("Server-A", 4, 16, new NetworkSettings("10.0.0.1", 80, ["10.0.0.0/24"]), []);

        // Act
        var clone = original.Clone();
        clone.Network.AllowedSubnets.Add("192.168.1.0/24");

        // Assert
        Assert.Single(original.Network.AllowedSubnets);
        Assert.Equal(2, clone.Network.AllowedSubnets.Count);
        Assert.NotSame(original.Network, clone.Network);
    }
}
