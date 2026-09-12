using GoFDesignPatterns.Core;

namespace GoFDesignPatterns.Behavioral;

public record PurchaseRequest(string RequestId, string Purpose, decimal Amount, string Requester);

public record ApprovalDecision(string RequestId, bool IsApproved, string ApprovedBy, string Comments);

public interface IApprover
{
    IApprover SetNext(IApprover next);
    Result<ApprovalDecision> ProcessRequest(PurchaseRequest request, List<string> logs);
}

public abstract class BaseApprover(string roleName, decimal approvalLimit) : IApprover
{
    protected readonly string RoleName = roleName;
    protected readonly decimal ApprovalLimit = approvalLimit;
    private IApprover? _nextApprover;

    public IApprover SetNext(IApprover next)
    {
        _nextApprover = next;
        return next;
    }

    public virtual Result<ApprovalDecision> ProcessRequest(PurchaseRequest request, List<string> logs)
    {
        if (request.Amount <= ApprovalLimit)
        {
            logs.Add($"[{RoleName}] Approved request '{request.RequestId}' for {request.Amount:C} (Limit: {ApprovalLimit:C}).");
            return Result<ApprovalDecision>.Success(
                new ApprovalDecision(request.RequestId, true, RoleName, $"Approved within {RoleName} authority limit."));
        }

        logs.Add($"[{RoleName}] Amount {request.Amount:C} exceeds limit {ApprovalLimit:C}. Escalating to next approver...");

        if (_nextApprover != null)
        {
            return _nextApprover.ProcessRequest(request, logs);
        }

        logs.Add($"[Chain End] No approver in the chain has authority for amount {request.Amount:C}. Request rejected.");
        return Result<ApprovalDecision>.Failure("ApprovalLimitExceeded",
            $"Purchase request '{request.RequestId}' of {request.Amount:C} exceeds the maximum organizational approval ceiling.");
    }
}

public class TeamLeadApprover() : BaseApprover("Team Lead", 1_000m);
public class DepartmentDirectorApprover() : BaseApprover("Department Director", 25_000m);
public class VicePresidentApprover() : BaseApprover("Vice President", 100_000m);
public class BoardOfDirectorsApprover() : BaseApprover("Board of Directors", 1_000_000m);
