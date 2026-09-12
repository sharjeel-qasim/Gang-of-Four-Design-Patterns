using GoFDesignPatterns.Core;

namespace GoFDesignPatterns.Creational;

/// <summary>
/// Product interface defined by the Factory Method pattern.
/// </summary>
public interface INotificationSender
{
    string ChannelType { get; }
    Result<string> Send(string recipient, string message);
}

/// <summary>
/// Concrete Product 1: Email notification.
/// </summary>
public class EmailNotificationSender : INotificationSender
{
    public string ChannelType => "Email";

    public Result<string> Send(string recipient, string message)
    {
        if (!recipient.Contains('@'))
            return Result<string>.Failure("InvalidEmail", $"Recipient '{recipient}' is not a valid email address.");

        return Result<string>.Success($"[SMTP] Sent email to '{recipient}': {message}");
    }
}

/// <summary>
/// Concrete Product 2: SMS notification.
/// </summary>
public class SmsNotificationSender : INotificationSender
{
    public string ChannelType => "SMS";

    public Result<string> Send(string recipient, string message)
    {
        if (recipient.Length < 10)
            return Result<string>.Failure("InvalidPhone", $"Recipient '{recipient}' is not a valid phone number.");

        return Result<string>.Success($"[SMS Gateway] Dispatched SMS to '{recipient}': {message}");
    }
}

/// <summary>
/// Concrete Product 3: Push notification.
/// </summary>
public class PushNotificationSender : INotificationSender
{
    public string ChannelType => "Push";

    public Result<string> Send(string recipient, string message)
    {
        return Result<string>.Success($"[FCM/APNS] Dispatched push notification to device token '{recipient}': {message}");
    }
}

/// <summary>
/// Creator (Abstract Class) defining the factory method CreateSender().
/// Core principle: The creator's primary responsibility is NOT creating products,
/// but contains core business logic that relies on the product returned by the factory method.
/// </summary>
public abstract class NotificationService
{
    // The Factory Method
    public abstract INotificationSender CreateSender();

    // Core business workflow that relies on the product
    public Result<string> NotifyUser(string recipient, string message)
    {
        var sender = CreateSender();
        var formattedMessage = $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] {message}";
        return sender.Send(recipient, formattedMessage);
    }
}

public class EmailNotificationService : NotificationService
{
    public override INotificationSender CreateSender() => new EmailNotificationSender();
}

public class SmsNotificationService : NotificationService
{
    public override INotificationSender CreateSender() => new SmsNotificationSender();
}

public class PushNotificationService : NotificationService
{
    public override INotificationSender CreateSender() => new PushNotificationSender();
}
