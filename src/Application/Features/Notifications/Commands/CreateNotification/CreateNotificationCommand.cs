using BusinessMakerTemplate.Application.Common;
using BusinessMakerTemplate.Application.Common.BaseImplementations;
using BusinessMakerTemplate.Application.Common.Models;
using BusinessMakerTemplate.Domain.Entities;
using BusinessMakerTemplate.Domain.Enums;
using Mapster;

namespace BusinessMakerTemplate.Application.Features.Notifications.Commands.CreateNotification;


public class CreateNotificationCommand : ICommandRequest
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public NotificationType NotificationType { get; set; }
}

public class CreateNotificationCommandHandler(IApplicationDbContext context)
    : CommandRequestHandler<CreateNotificationCommand>
{
    public override async Task<Result> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var notificationEntity = request.Adapt<Notification>();
        await context.Notifications.AddAsync(notificationEntity, cancellationToken);
        await context.SaveChangeAsync(cancellationToken);
        return Ok();
    }
}