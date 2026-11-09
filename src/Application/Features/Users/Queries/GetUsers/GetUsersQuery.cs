using Application.Common.Utils;
using Domain.Entities.UserAggregate;
using Microsoft.AspNetCore.DataProtection;

namespace Application.Features.Users.Queries.GetUsers;

public class UserDTO
{
    public long Id { get; set; }
    public bool IsSystem { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}


public class GetUsersQuery : IQueryRequest<PaginationResult<UserDTO>>
{
    public GetUsersQuery(PaginationParams paginatedParams)
    {
        PaginatedParams = paginatedParams;
    }

    public PaginationParams PaginatedParams { get; set; }
}

public class GetUsersQueryHandler : QueryRequestHandler<GetUsersQuery, PaginationResult<UserDTO>>
{
    private readonly IAuthDbContext _context;

    public GetUsersQueryHandler(IDataProtectionProvider dataProtectionProvider, IAuthDbContext context)
        : base(dataProtectionProvider, "dataProtectorKey")
    {
        _context = context;
    }

    public override async Task<PaginationResult<UserDTO>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.Users.GetPaginatedDataAsync<UserDTO, User>(request.PaginatedParams);
        return result;
    }
}