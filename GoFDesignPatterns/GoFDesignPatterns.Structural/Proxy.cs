using GoFDesignPatterns.Core;

namespace GoFDesignPatterns.Structural;

public interface IReportService
{
    Result<string> GenerateFinancialReport(string reportId, string userRole);
}

/// <summary>
/// Real Subject: Heavy computation / data retrieval service.
/// Expensive to initialize and execute.
/// </summary>
public class ExpensiveFinancialReportService : IReportService
{
    public ExpensiveFinancialReportService()
    {
        // Simulating expensive database schema warmup or ML model initialization
        Thread.Sleep(30);
    }

    public Result<string> GenerateFinancialReport(string reportId, string userRole)
    {
        return Result<string>.Success(
            $"[Financial Report {reportId}] Generated at {DateTime.UtcNow:s}. " +
            $"Total Revenue: $1,420,500.00 | Operating Margin: 24.3%");
    }
}

/// <summary>
/// Virtual & Protection Proxy:
/// 1. Protection: Verifies caller possesses 'Admin' or 'Auditor' privileges before allowing access.
/// 2. Virtual (Lazy): Postpones creating the expensive real subject until the first valid request.
/// </summary>
public class SecuredReportProxy(List<string> logs) : IReportService
{
    private ExpensiveFinancialReportService? _realService;

    public Result<string> GenerateFinancialReport(string reportId, string userRole)
    {
        logs.Add($"[Proxy] Security check: Requesting report '{reportId}' by role '{userRole}'");

        // 1. Protection check
        if (!string.Equals(userRole, "Admin", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(userRole, "Auditor", StringComparison.OrdinalIgnoreCase))
        {
            logs.Add($"[Proxy - ACCESS DENIED] Role '{userRole}' does not have permission to view financial reports.");
            return Result<string>.Failure("Unauthorized", "Access denied: Required role is Admin or Auditor.");
        }

        logs.Add("[Proxy - ACCESS GRANTED] Role authorized.");

        // 2. Virtual Proxy (Lazy initialization)
        if (_realService == null)
        {
            logs.Add("[Proxy - VIRTUAL] Initializing expensive report engine for the first time...");
            _realService = new ExpensiveFinancialReportService();
            logs.Add("[Proxy - VIRTUAL] Heavy service initialized.");
        }
        else
        {
            logs.Add("[Proxy - VIRTUAL] Reusing existing report engine instance.");
        }

        return _realService.GenerateFinancialReport(reportId, userRole);
    }
}
