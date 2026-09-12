using GoFDesignPatterns.Core;

namespace GoFDesignPatterns.Structural;

// Implementor Interface (Bridge implementor)
public interface IMessageChannel
{
    string ChannelName { get; }
    string Deliver(string title, string body, string destination);
}

// Concrete Implementor 1: Slack
public class SlackMessageChannel : IMessageChannel
{
    public string ChannelName => "Slack";
    public string Deliver(string title, string body, string destination) =>
        $"[Slack -> #{destination}] *{title}*\n>{body}";
}

// Concrete Implementor 2: Email
public class EmailMessageChannel : IMessageChannel
{
    public string ChannelName => "Email";
    public string Deliver(string title, string body, string destination) =>
        $"[SMTP -> {destination}] Subject: {title} | Body: {body}";
}

// Concrete Implementor 3: SMS
public class SmsMessageChannel : IMessageChannel
{
    public string ChannelName => "SMS";
    public string Deliver(string title, string body, string destination) =>
        $"[SMS -> {destination}] {title}: {body}";
}

// Abstraction
public abstract class Notification(IMessageChannel channel)
{
    protected readonly IMessageChannel Channel = channel;

    public abstract string Send(string recipient, string title, string message);
}

// Refined Abstraction 1: Standard Notification
public class StandardNotification(IMessageChannel channel) : Notification(channel)
{
    public override string Send(string recipient, string title, string message) =>
        Channel.Deliver(title, message, recipient);
}

// Refined Abstraction 2: Urgent High-Priority Notification
public class UrgentNotification(IMessageChannel channel) : Notification(channel)
{
    public override string Send(string recipient, string title, string message)
    {
        var urgentTitle = $"🚨 [CRITICAL ALERT] {title}";
        var urgentBody = $"{message}\nImmediate action required! Escalation SLA: 15 mins.";
        return Channel.Deliver(urgentTitle, urgentBody, recipient);
    }
}

// Refined Abstraction 3: Batch Digest Notification
public class DigestNotification(IMessageChannel channel) : Notification(channel)
{
    public override string Send(string recipient, string title, string message)
    {
        var digestTitle = $"📊 [Daily Digest] {title}";
        var digestBody = $"Aggregated summary for {DateTime.UtcNow:yyyy-MM-dd}:\n{message}";
        return Channel.Deliver(digestTitle, digestBody, recipient);
    }
}
