using BusinessMakerFramework.Domain.Core.Events;
using BusinessMakerFramework.Domain.Core.ValueObjects;

namespace Domain.Entities.WalletAggregate.Events;

public record OnWalletCreatedEvent(string name, Guid businessId) : DomainEvent(nameof(OnWalletCreatedEvent))
{
    public string Name { get; } = name;
    public long Balance { get; } = 0;
    public Guid BusinessId { get; } = businessId;
}


public record WalletNameChangedEvent(string name) : DomainEvent(nameof(WalletNameChangedEvent))
{
    public string Name { get; } = name;
}

public record WalletBalanceDepositedEvent(Guid walletId, long amount) : DomainEvent(nameof(WalletBalanceDepositedEvent))
{
    public Guid WalletId { get; } = walletId;
    public long Amount { get; } = amount;
}

public record WalletBalanceWithdrawedEvent(Guid walletId, long amount) : DomainEvent(nameof(WalletBalanceWithdrawedEvent))
{
    public Guid WalletId { get; } = walletId;
    public long Amount { get; } = amount;
}

public record WalletSnapshootEvent(string name, long balance, BusinessId businessId, int version) : DomainEvent(nameof(WalletSnapshootEvent))
{
    public string Name { get; } = name;
    public long Balance { get; } = balance;
    public BusinessId BusinessId { get; } = businessId;
}
