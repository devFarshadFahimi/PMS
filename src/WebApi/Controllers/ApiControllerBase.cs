using Asp.Versioning;
using BusinessMakerFramework.Shared.Utilities.Utilities;
using BusinessMakerFramework.SourceGenerator.Shared.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiVersion(1.0)]
[ApiController]
//[ApiVersion(1.0, Deprecated = true)]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class ApiControllerBase(IMediator mediator) : ControllerBase
{
    protected async Task<IActionResult> SendAsync<T>(
     IRequest<Result<T>> commandOrQuery,
     CancellationToken cancellationToken)
    {
        var result = await mediator.SendAsync(commandOrQuery!, cancellationToken);
        return result.ToActionResult();
    }
    protected async Task<IActionResult> SendAsync<T>(
     IRequest<T> commandOrQuery,
     CancellationToken cancellationToken)
    {
        var result = await mediator.SendAsync(commandOrQuery!, cancellationToken);
        return Ok(result);
    }

    protected async Task<IActionResult> SendAsync(
        IRequest<Result> commandOrQuery,
        CancellationToken cancellationToken)
    {
        var result = await mediator.SendAsync(commandOrQuery!, cancellationToken);
        return result.ToActionResult();
    }
}

[ApiController]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
public abstract class BasicController : ControllerBase
{
}

[ApiVersion(2.0, Deprecated = true)]
public class YTApiController : BasicController
{
    [HttpGet]
    public IActionResult GetUsers()
    {
        throw new NotImplementedException();
    }
}

[ApiVersion(2.2)]
public class TestController : BasicController
{
    [HttpGet]
    public IActionResult GetUsers()
    {
        throw new NotImplementedException();
    }
}

[ApiVersion(2.1, status: "workinprogress")]
public class ProductsController : BasicController
{
    [HttpGet]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult GetProductById()
    {
        return Ok();
    }
}