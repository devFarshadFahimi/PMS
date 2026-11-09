using BusinessMakerFramework.Domain.Core.Entities;
using BusinessMakerFramework.Domain.Core.Events;
using Domain.Entities.CustomerAggregate.Events;

namespace Domain.Entities.CustomerAggregate;

/// <summary>
/// This aggregate has been configured with EventStoreDrivenDesign principle.
/// </summary>
public class Customer : AggregateRoot, IAuditableMarker
{
    public Customer(string name)
    {
        Apply(new OnCustomerCreatedEvent(name, 100000000, BusinessId.Value));
    }

    public Customer(IReadOnlyList<IDomainEvent> events)
        : base(events)
    {
    }

    public Customer()
    {
    }

    public string Name { get; private set; } = null!;
    public string Code { get; private set; } = null!;
    public long Balance { get; private set; }

    public void ChangeName(string name)
    {
        Apply(new NameChangedEvent(name));
    }

    public void GenerateCode(string code)
    {
        //var gCode = code + "-0";
        Apply(new CodeGenereatedEvent(code));
    }

    public void Deposit(long amount)
    {
        Apply(new BalanceDepositedEvent(amount));
    }

    public void Withdrawal(long amount)
    {
        Apply(new BalanceWithdrawedEvent(amount));
    }

    private void On(OnCustomerCreatedEvent @event)
    {
        Name = @event.Name;
        Balance = @event.Balance;
        BusinessId = @event.BusinessId;
    }

    private void On(BalanceDepositedEvent @event)
    {
        Balance += @event.Amount;
    }

    private void On(BalanceWithdrawedEvent @event)
    {
        Balance -= @event.Amount;
    }

    private void On(CodeGenereatedEvent @event)
    {
        Code = @event.Code;
    }

    private void On(NameChangedEvent @event)
    {
        Name = @event.Name;
    }
}
