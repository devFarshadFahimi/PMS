using Application.Features.Customers.Commands.CreateCustomer;
using Application.Features.Customers.Queries.GetCustomer;
using BusinessMakerFramework.SourceGenerator.Shared.Contracts;
using Microsoft.AspNetCore.Mvc;
namespace WebApi.Controllers;

public class CustomerController(IMediator mediator) : ApiControllerBase(mediator)
{
    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        return await SendAsync(command, cancellationToken);
    }

    //[HttpGet("GetHistory/{businessId:guid}")]
    //public async Task<IActionResult> GetHistoryAsync(
    //    Guid businessId, CancellationToken cancellationToken)
    //{
    //    return await SendAsync(new GetAuditHistoryCommand(businessId), cancellationToken);
    //}

    [HttpGet]
    public async Task<IActionResult> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return await SendAsync(new GetCustomerByIdQuery(id), cancellationToken);
    }
}
