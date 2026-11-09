using BusinessMakerFramework.Infrastructure.Common.Contracts.EventSourcing;
using Domain.Entities.CustomerAggregate;
using Mapster;

namespace Application.Features.Customers.Queries.GetCustomer;

public class GetCustomerByIdQuery(Guid id) : IQueryRequest<CustomerInfoDTO>
{
    public Guid Id { get; } = id;
}


public class GetCustomerByIdQueryHandler(IEventStoreContext eventStore) : QueryRequestHandler<GetCustomerByIdQuery, CustomerInfoDTO>
{
    public override async Task<CustomerInfoDTO> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var events = await eventStore.GetAsync<Customer>(request.Id);
        var customer = new Customer(events);

        return customer.Adapt<CustomerInfoDTO>();
    }
}

public class CustomerInfoDTO
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public long Balance { get; set; }
    public Guid BusinessId { get; set; }
}