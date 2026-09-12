using GoFDesignPatterns.Core;

namespace GoFDesignPatterns.Behavioral;

// Receiver
public class BankAccount(string accountNumber, decimal initialBalance = 0m)
{
    public string AccountNumber { get; } = accountNumber;
    public decimal Balance { get; private set; } = initialBalance;

    public void Deposit(decimal amount) => Balance += amount;

    public bool TryWithdraw(decimal amount)
    {
        if (Balance < amount) return false;
        Balance -= amount;
        return true;
    }
}

// Command Interface
public interface ITransactionCommand
{
    string Description { get; }
    Result<string> Execute();
    Result<string> Undo();
}

// Concrete Command 1: Deposit
public class DepositCommand(BankAccount account, decimal amount) : ITransactionCommand
{
    public string Description => $"Deposit {amount:C} to {account.AccountNumber}";

    public Result<string> Execute()
    {
        account.Deposit(amount);
        return Result<string>.Success($"Deposited {amount:C}. New Balance: {account.Balance:C}");
    }

    public Result<string> Undo()
    {
        if (account.TryWithdraw(amount))
        {
            return Result<string>.Success($"Reversed deposit of {amount:C}. New Balance: {account.Balance:C}");
        }
        return Result<string>.Failure("UndoFailed", "Insufficient funds to undo deposit.");
    }
}

// Concrete Command 2: Withdraw
public class WithdrawCommand(BankAccount account, decimal amount) : ITransactionCommand
{
    public string Description => $"Withdraw {amount:C} from {account.AccountNumber}";

    public Result<string> Execute()
    {
        if (account.TryWithdraw(amount))
        {
            return Result<string>.Success($"Withdrew {amount:C}. New Balance: {account.Balance:C}");
        }
        return Result<string>.Failure("InsufficientFunds", $"Account {account.AccountNumber} has insufficient funds for {amount:C}.");
    }

    public Result<string> Undo()
    {
        account.Deposit(amount);
        return Result<string>.Success($"Reversed withdrawal of {amount:C}. New Balance: {account.Balance:C}");
    }
}

// Invoker: Transaction Manager with Audit Log and Undo capability
public class TransactionManager
{
    private readonly Stack<ITransactionCommand> _history = [];
    private readonly List<string> _auditTrail = [];

    public IReadOnlyList<string> AuditTrail => _auditTrail.AsReadOnly();

    public Result<string> ExecuteTransaction(ITransactionCommand command)
    {
        _auditTrail.Add($"[EXECUTE] Invoking: {command.Description}");
        var result = command.Execute();

        if (result.IsSuccess)
        {
            _history.Push(command);
            _auditTrail.Add($"[SUCCESS] {result.Value}");
        }
        else
        {
            _auditTrail.Add($"[FAILED] {result.Error.Message}");
        }

        return result;
    }

    public Result<string> UndoLastTransaction()
    {
        if (_history.Count == 0)
        {
            return Result<string>.Failure("EmptyHistory", "No transactions available to undo.");
        }

        var command = _history.Pop();
        _auditTrail.Add($"[UNDO] Rolling back: {command.Description}");
        var result = command.Undo();

        if (result.IsSuccess)
        {
            _auditTrail.Add($"[UNDO SUCCESS] {result.Value}");
        }
        else
        {
            _auditTrail.Add($"[UNDO FAILED] {result.Error.Message}");
        }

        return result;
    }
}
