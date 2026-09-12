using GoFDesignPatterns.Core;

namespace GoFDesignPatterns.Creational;

// Abstract Products
public interface IBlobStorage
{
    string ProviderName { get; }
    string UploadBlob(string containerName, string blobName, byte[] data);
}

public interface IMessageQueue
{
    string ProviderName { get; }
    string EnqueueMessage(string queueName, string payload);
}

public interface IComputeInstance
{
    string ProviderName { get; }
    string ProvisionVM(string instanceSize, string osImage);
}

// Concrete Products for AWS
public class AwsS3Storage : IBlobStorage
{
    public string ProviderName => "AWS S3";
    public string UploadBlob(string containerName, string blobName, byte[] data) =>
        $"[AWS S3] Uploaded '{blobName}' ({data.Length} bytes) to bucket 's3://{containerName}'";
}

public class AwsSqsQueue : IMessageQueue
{
    public string ProviderName => "AWS SQS";
    public string EnqueueMessage(string queueName, string payload) =>
        $"[AWS SQS] Enqueued message to 'https://sqs.us-east-1.amazonaws.com/{queueName}' with payload: {payload}";
}

public class AwsEc2Compute : IComputeInstance
{
    public string ProviderName => "AWS EC2";
    public string ProvisionVM(string instanceSize, string osImage) =>
        $"[AWS EC2] Provisioned {instanceSize} instance running {osImage}";
}

// Concrete Products for Azure
public class AzureBlobStorage : IBlobStorage
{
    public string ProviderName => "Azure Blob Storage";
    public string UploadBlob(string containerName, string blobName, byte[] data) =>
        $"[Azure Blob] Uploaded '{blobName}' ({data.Length} bytes) to container 'https://mystorage.blob.core.windows.net/{containerName}'";
}

public class AzureServiceBusQueue : IMessageQueue
{
    public string ProviderName => "Azure Service Bus";
    public string EnqueueMessage(string queueName, string payload) =>
        $"[Azure Service Bus] Published message to queue '{queueName}' with payload: {payload}";
}

public class AzureVirtualMachineCompute : IComputeInstance
{
    public string ProviderName => "Azure VM";
    public string ProvisionVM(string instanceSize, string osImage) =>
        $"[Azure VM] Provisioned Standard_{instanceSize} VM running {osImage}";
}

/// <summary>
/// Abstract Factory: Declares a set of creation methods for each abstract product in the family.
/// </summary>
public interface ICloudInfrastructureFactory
{
    string ProviderName { get; }
    IBlobStorage CreateBlobStorage();
    IMessageQueue CreateMessageQueue();
    IComputeInstance CreateComputeInstance();
}

public class AwsInfrastructureFactory : ICloudInfrastructureFactory
{
    public string ProviderName => "Amazon Web Services (AWS)";
    public IBlobStorage CreateBlobStorage() => new AwsS3Storage();
    public IMessageQueue CreateMessageQueue() => new AwsSqsQueue();
    public IComputeInstance CreateComputeInstance() => new AwsEc2Compute();
}

public class AzureInfrastructureFactory : ICloudInfrastructureFactory
{
    public string ProviderName => "Microsoft Azure";
    public IBlobStorage CreateBlobStorage() => new AzureBlobStorage();
    public IMessageQueue CreateMessageQueue() => new AzureServiceBusQueue();
    public IComputeInstance CreateComputeInstance() => new AzureVirtualMachineCompute();
}

/// <summary>
/// Client class that consumes the family of products without coupling to concrete cloud vendors.
/// </summary>
public class CloudDeploymentOrchestrator(ICloudInfrastructureFactory factory)
{
    public List<string> DeployMicroserviceEnvironment(string serviceName)
    {
        var logs = new List<string>
        {
            $"Initializing cloud environment using provider: {factory.ProviderName}"
        };

        var storage = factory.CreateBlobStorage();
        logs.Add(storage.UploadBlob("artifacts", $"{serviceName}-build.zip", [1, 2, 3, 4]));

        var queue = factory.CreateMessageQueue();
        logs.Add(queue.EnqueueMessage($"{serviceName}-events", "DeploymentStarted"));

        var compute = factory.CreateComputeInstance();
        logs.Add(compute.ProvisionVM("t3.xlarge", "Ubuntu-22.04-LTS"));

        logs.Add($"Microservice '{serviceName}' successfully deployed.");
        return logs;
    }
}
