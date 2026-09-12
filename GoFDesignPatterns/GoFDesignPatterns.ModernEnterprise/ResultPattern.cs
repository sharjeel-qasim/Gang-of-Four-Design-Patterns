using GoFDesignPatterns.Core;

namespace GoFDesignPatterns.ModernEnterprise;

public record LoanApplication(Guid ApplicantId, string FullName, decimal RequestedAmount, int CreditScore, decimal MonthlyIncome);

public record ApprovedLoan(Guid ApplicationId, Guid ApplicantId, decimal Amount, decimal MonthlyPayment, decimal InterestRate);

/// <summary>
/// Senior .NET Pattern: Railway-Oriented Programming (ROP) using Result&lt;T&gt;.
/// Each step takes input, validates, and returns Result. Bind() chains successes and immediately short-circuits on failure.
/// Zero exceptions thrown for standard business rejections.
/// </summary>
public class LoanProcessingPipeline(List<string> logs)
{
    public Result<ApprovedLoan> ProcessApplication(LoanApplication app)
    {
        logs.Add($"[ROP Pipeline] Starting evaluation for applicant '{app.FullName}' (Score: {app.CreditScore})");

        return ValidateApplication(app)
            .Bind(CheckCreditScore)
            .Bind(CheckDebtToIncomeRatio)
            .Map(approved =>
            {
                logs.Add($"[ROP Pipeline] SUCCESS: Loan approved for {approved.Amount:C} at {approved.InterestRate * 100}% APR");
                return approved;
            });
    }

    private Result<LoanApplication> ValidateApplication(LoanApplication app)
    {
        logs.Add("[Step 1] Validating application parameters...");
        if (app.RequestedAmount <= 0)
            return Result<LoanApplication>.Failure("InvalidAmount", "Requested loan amount must be greater than zero.");
        if (app.MonthlyIncome <= 0)
            return Result<LoanApplication>.Failure("InvalidIncome", "Monthly income must be greater than zero.");

        logs.Add("[Step 1] Application data is valid.");
        return Result<LoanApplication>.Success(app);
    }

    private Result<LoanApplication> CheckCreditScore(LoanApplication app)
    {
        logs.Add($"[Step 2] Checking credit score: {app.CreditScore}...");
        if (app.CreditScore < 640)
        {
            logs.Add("[Step 2] REJECTED: Credit score below minimum threshold of 640.");
            return Result<LoanApplication>.Failure("CreditScoreTooLow", $"Credit score {app.CreditScore} is below minimum 640.");
        }

        logs.Add("[Step 2] Credit score approved.");
        return Result<LoanApplication>.Success(app);
    }

    private Result<ApprovedLoan> CheckDebtToIncomeRatio(LoanApplication app)
    {
        logs.Add("[Step 3] Calculating estimated monthly payment and debt ratio...");
        var interestRate = app.CreditScore >= 750 ? 0.055m : 0.085m;
        var monthlyPayment = (app.RequestedAmount * (1m + interestRate)) / 36m; // 3-year term

        if (monthlyPayment > (app.MonthlyIncome * 0.40m))
        {
            logs.Add("[Step 3] REJECTED: Estimated monthly payment exceeds 40% debt-to-income threshold.");
            return Result<ApprovedLoan>.Failure("HighDebtRatio", "Estimated monthly payment exceeds debt-to-income limits.");
        }

        logs.Add("[Step 3] Debt-to-income ratio approved.");
        return Result<ApprovedLoan>.Success(new ApprovedLoan(
            ApplicationId: Guid.NewGuid(),
            ApplicantId: app.ApplicantId,
            Amount: app.RequestedAmount,
            MonthlyPayment: Math.Round(monthlyPayment, 2),
            InterestRate: interestRate
        ));
    }
}
