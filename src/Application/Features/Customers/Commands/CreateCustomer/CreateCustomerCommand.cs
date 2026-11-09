using BusinessMakerFramework.SourceGenerator.Shared.Contracts;
using Domain.Entities.CustomerAggregate;
using Microsoft.Extensions.Logging;

namespace Application.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerCommand : ICommandRequest<bool>
{
    public string Name { get; set; } = null!;
}

public class CreateCustomerCommandHandler(IApplicationDbContext context) : CommandRequestHandler<CreateCustomerCommand, bool>
{
    public override async Task<Result<bool>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken = default)
    {
        var customer = new Customer(request.Name);
        customer.GenerateCode("Code");
        _ = await context.Customers.AddAsync(customer, cancellationToken);
        await context.SaveChangeAsync(cancellationToken);
        return Ok(true);
    }
}

public class GetAuditHistoryCommand(Guid businessId) : IRequest<bool>
{
    public Guid BusinessId { get; } = businessId;
}

public class GetAuditHistoryCommandHandler(ILogger<GetAuditHistoryCommand> logger) : IRequestHandler<GetAuditHistoryCommand, bool>
{
    public Task<bool> Handle(GetAuditHistoryCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Framework Mediator is working");
        return Task.FromResult(true);
    }
}


//public class GetAuditHistoryCommandHandler(IEventStoreContext eventStore) : CommandRequestHandler<GetAuditHistoryCommand, List<AuditedTracker>>
//{
//    public override async Task<Result<List<AuditedTracker>>> Handle(GetAuditHistoryCommand request, CancellationToken cancellationToken = default)
//    {
//        var items = await eventStore.GetAuditedHistoryAsync(nameof(Customer), request.BusinessId);
//        return Ok(items);
//    }
//}
