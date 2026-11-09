using BusinessMakerFramework.Domain.Core.Entities;
using Domain.Entities.WalletAggregate.Events;

namespace Domain.Entities.WalletAggregate;

public class Wallet : AggregateRoot<Guid>
{
    public Wallet(string name)
    {
        Id = Guid.NewGuid();
        AddEvent(new OnWalletCreatedEvent(name, BusinessId.Value));
    }

    public string Name { get; private set; } = null!;
    public long Balance { get; private set; }

    public void Deposit(long amount)
    {
        if (amount <= 0)
        {
            throw new InvalidOperationException("Deposit amount must be positive.");
        }

        AddEvent(new WalletBalanceDepositedEvent(Id, amount));
    }

    public void Withdraw(long amount)
    {
        if (amount <= 0)
        {
            throw new InvalidOperationException("Withdrawal amount must be positive.");
        }

        if (Balance < amount)
        {
            throw new InvalidOperationException("Insufficient funds.");
        }

        AddEvent(new WalletBalanceWithdrawedEvent(Id, amount));
    }
}
