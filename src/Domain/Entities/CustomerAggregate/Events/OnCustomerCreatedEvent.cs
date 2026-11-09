using BusinessMakerFramework.Domain.Core.Events;

namespace Domain.Entities.CustomerAggregate.Events;

public record OnCustomerCreatedEvent(string name, long balance, Guid businessId) : DomainEvent(nameof(OnCustomerCreatedEvent))
{
    public string Name { get; } = name;
    public long Balance { get; } = balance;
    public Guid BusinessId { get; } = businessId;
}

public record NameChangedEvent(string name) : DomainEvent(nameof(NameChangedEvent))
{
    public string Name { get; } = name;
}
public record CodeGenereatedEvent(string code) : DomainEvent(nameof(CodeGenereatedEvent))
{
    public string Code { get; } = code;
}

public record BalanceDepositedEvent(long amount) : DomainEvent(nameof(BalanceDepositedEvent))
{
    public long Amount { get; } = amount;
}

public record BalanceWithdrawedEvent(long amount) : DomainEvent(nameof(BalanceWithdrawedEvent))
{
    public long Amount { get; } = amount;
}