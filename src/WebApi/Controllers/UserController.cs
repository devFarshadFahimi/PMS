using System.Security.Claims;
using Application.Common.Utils;
using Application.Features.Users.Queries.GetUsers;
using BusinessMakerFramework.Shared.Utilities.Extensions;
using BusinessMakerFramework.SourceGenerator.Shared.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Controllers;

//[Authorization("Admin")]
public class UserController(IMediator mediator) : ApiControllerBase(mediator)
{
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] PaginationParams paginationParams,
        CancellationToken cancellationToken)
    {
        return await SendAsync(new GetUsersQuery(paginationParams), cancellationToken);
    }

    //[AllowAnonymous]
    //[HttpGet("GetUsers")]
    //public async Task<IActionResult> GetUsers([FromQuery] FilteredQueryParameters queryParams)
    //{
    //    var query = _dbContext.Users.AsQueryable();

    //    query = query.ApplyFilters(queryParams.Filters);

    //    query = query.ApplySorting(queryParams.SortBy, queryParams.SortDir);

    //    var totalCount = await query.CountAsync();
    //    var data = await query
    //        .Skip(queryParams.Page * queryParams.Size)
    //        .Take(queryParams.Size)
    //        .ToListAsync();

    //    return Ok(new PagedResult<User>
    //    {
    //        Data = data,
    //        Count = totalCount
    //    });
    //}
}

public class PagedResult<T>
{
    public List<T> Data { get; set; } = [];
    public int Count { get; set; }
}

public class QueryParameters
{
    public int Page { get; set; } = 0;
    public int Size { get; set; } = 10;
    public string SortBy { get; set; } = "id";
    public string SortDir { get; set; } = "asc";
}

[ModelBinder(BinderType = typeof(FilteredQueryParametersBinder))]
public class FilteredQueryParameters : QueryParameters
{
    // همه‌ی چیزی که query string با "filter_" شروع بشه، میره اینجا
    public Dictionary<string, string> Filters { get; set; } = [];
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class Authorization : Attribute, IAsyncAuthorizationFilter
{
    private readonly string[] _roles = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="Authorization"/> class.
    /// Pass roles that exists in user claims and separated the by comma like below.
    /// </summary>
    /// <param name="roles">Admin,Customer.</param>
    public Authorization(string roles)
    {
        _roles = [.. roles.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(r => r.Trim())];
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        if (user.Identity is not { IsAuthenticated: true })
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var sender = context.HttpContext.RequestServices.GetRequiredService<IMediator>();
        var userId = context.HttpContext.User.GetClaims(ClaimTypes.NameIdentifier);
        //var userIsLoggedIn = await sender.Send(new UserIsLoginQuery(userId), default);
        //if (!userIsLoggedIn.Body)
        //{
        //    context.Result = new UnauthorizedResult();
        //    return;
        //}

        if (_roles.Length != 0)
        {
            var userRoles = user.Claims
                            .Where(c => c.Type == ClaimsIdentity.DefaultRoleClaimType)
                            .Select(c => c.Value);

            var hasAccess = _roles.Intersect(userRoles).Any();

            if (!hasAccess)
            {
                context.Result = new ForbidResult();
                return;
            }
        }

    }
}