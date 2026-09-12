using System.Xml.Linq;
using GoFDesignPatterns.Core;

namespace GoFDesignPatterns.Structural;

// Modern Target Interface used by the rest of the application
public interface IPaymentGateway
{
    Result<PaymentTransaction> ProcessPayment(string customerId, decimal amount, string currency);
}

public record PaymentTransaction(string TransactionId, decimal Amount, string Currency, DateTime Timestamp, string Status);

/// <summary>
/// Adaptee: A legacy 3rd-party banking system that only accepts and emits proprietary XML strings.
/// Cannot be modified directly.
/// </summary>
public class LegacyXmlBankingSystem
{
    public string ExecuteXmlTransfer(string xmlPayload)
    {
        var doc = XDocument.Parse(xmlPayload);
        var account = doc.Root?.Element("Account")?.Value ?? "Unknown";
        var amount = doc.Root?.Element("Amount")?.Value ?? "0.00";
        var reference = $"LEGACY-{Guid.NewGuid().ToString()[..6].ToUpper()}";

        return $"""
            <LegacyResponse>
                <Status>APPROVED</Status>
                <ReferenceCode>{reference}</ReferenceCode>
                <Account>{account}</Account>
                <ProcessedAmount>{amount}</ProcessedAmount>
                <ServerTimestamp>{DateTime.UtcNow:o}</ServerTimestamp>
            </LegacyResponse>
            """;
    }
}

/// <summary>
/// Adapter: Implements the modern IPaymentGateway interface, translates calls into
/// XML format required by LegacyXmlBankingSystem, and parses the XML response back into PaymentTransaction.
/// </summary>
public class LegacyBankingAdapter(LegacyXmlBankingSystem legacySystem) : IPaymentGateway
{
    public Result<PaymentTransaction> ProcessPayment(string customerId, decimal amount, string currency)
    {
        try
        {
            // 1. Transform modern parameters into legacy XML request
            var xmlRequest = $"""
                <LegacyPaymentRequest>
                    <Account>{customerId}</Account>
                    <Amount>{amount:F2}</Amount>
                    <Currency>{currency}</Currency>
                </LegacyPaymentRequest>
                """;

            // 2. Delegate to legacy system
            var rawXmlResponse = legacySystem.ExecuteXmlTransfer(xmlRequest);

            // 3. Parse legacy XML response into modern domain model
            var responseDoc = XDocument.Parse(rawXmlResponse);
            var status = responseDoc.Root?.Element("Status")?.Value;

            if (status != "APPROVED")
            {
                return Result<PaymentTransaction>.Failure("PaymentRejected", $"Legacy system rejected payment with status: {status}");
            }

            var refCode = responseDoc.Root?.Element("ReferenceCode")?.Value ?? Guid.NewGuid().ToString();
            var processedAmount = decimal.Parse(responseDoc.Root?.Element("ProcessedAmount")?.Value ?? amount.ToString());

            var transaction = new PaymentTransaction(
                TransactionId: refCode,
                Amount: processedAmount,
                Currency: currency,
                Timestamp: DateTime.UtcNow,
                Status: "Completed"
            );

            return Result<PaymentTransaction>.Success(transaction);
        }
        catch (Exception ex)
        {
            return Result<PaymentTransaction>.Failure("AdapterError", $"Adapter failed to translate legacy response: {ex.Message}");
        }
    }
}
