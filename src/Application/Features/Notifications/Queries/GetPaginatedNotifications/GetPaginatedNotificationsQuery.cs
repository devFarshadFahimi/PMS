using BusinessMakerTemplate.Application.Common;
using BusinessMakerTemplate.Application.Common.BaseImplementations;
using BusinessMakerTemplate.Application.Common.Contracts;
using BusinessMakerTemplate.Application.Common.Extensions;
using BusinessMakerTemplate.Application.Common.Models;
using BusinessMakerTemplate.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using static BusinessMakerTemplate.Application.Common.Utils.Consts;

namespace BusinessMakerTemplate.Application.Features.Notifications.Queries.GetPaginatedNotifications;

public record Link(string Href, string Rel, string Method);

public class NotificationDatatableDTO : BaseLongId, IHasHashedId, IHasLink
{
    public bool IsDelivered { get; set; }

    public string? Title { get; set; }
    public string Content { get; set; } = null!;

    public NotificationType NotificationType { get; set; }

    public DateTime SendDate { get; set; }
    public string HashedId { get; set; } = null!;

    public List<Link> Links { get; set; } = [];
}

public class GetPaginatedNotificationsQuery : PagedQueryRequest<NotificationDatatableDTO>
{
    public static GetPaginatedNotificationsQuery CreatePaginationQuery(int pageSize, int pageNumber)
        => new(pageSize, pageNumber);

    public GetPaginatedNotificationsQuery(int pageSize, int pageNumber)
    {
        PageSize = pageSize;
        PageNumber = pageNumber;
    }
}

public class GetPaginatedNotificationsQueryHandler(IApplicationDbContext context, IDataProtectionProvider dataProtectionProvider,
    LinkGenerator linkGenerator,
    IHttpContextAccessor httpContextAccessor)
    : PagedQueryRequestHandler<GetPaginatedNotificationsQuery, NotificationDatatableDTO>(dataProtectionProvider, ProtectorKeys.NotificationKey)
{
    public override async Task<Result<PagedQueryResponse<NotificationDatatableDTO>>> Handle(GetPaginatedNotificationsQuery request, CancellationToken cancellationToken)
    {
        var iss = context.Notifications.FirstOrDefault();
        var resultx = await context.Notifications.ToPagedQueryResponse(
            request,
            DataProtector,
            linkGenerator,
            httpContextAccessor.HttpContext,
            cancellationToken: cancellationToken);
        return Ok(resultx);
    }
}
