using Application.Common.Utils;
using Domain.Entities.UserAggregate;
using Microsoft.AspNetCore.DataProtection;

namespace Application.Features.Users.Queries.GetUsers;
//public class UserDto
//{
//    public long Id { get; set; }
//    public DateTime ValidTo { get; set; }

//    [MapToEntity("User.UserName")]
//    public string UserUserName { get; set; }

//    [MapToEntity("User.Id")]
//    public long UserId { get; set; }
//}


public class UserDto
{
    public long Id { get; set; }
    public bool IsSystem { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}


public class GetUsersQuery : IQueryRequest<PaginationResult<UserDto>>
{
    public GetUsersQuery(PaginationParams paginatedParams)
    {
        PaginatedParams = paginatedParams;
    }

    public PaginationParams PaginatedParams { get; set; }
}

public class GetUsersQueryHandler : QueryRequestHandler<GetUsersQuery, PaginationResult<UserDto>>
{
    private readonly IAuthDbContext _context;

    public GetUsersQueryHandler(IDataProtectionProvider dataProtectionProvider, IAuthDbContext context)
        : base(dataProtectionProvider, "dataProtectorKey")
    {
        _context = context;
    }

    public override async Task<Result<PaginationResult<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.Users.GetPaginatedDataAsync<UserDto, User>(request.PaginatedParams);
        return Ok(result);
    }
}