using BusinessMakerTemplate.Application.Common.Extensions;
using BusinessMakerTemplate.Domain.Events.NotificationEvents;

namespace BusinessMakerTemplate.Application.Features.Notifications.EventHandlers;
internal class NotificationCreatedHandler(ILogger<NotificationCreatedHandler> logger)
    : INotificationHandler<NotificationCreated>
{
    private const string Message = "CleanArchitecture Domain Event: {DomainEvent} --- {Data}";

    public Task Handle(NotificationCreated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            Message,
            notification.GetType().Name,
            notification.ToSerializedString());

        return Task.CompletedTask;
    }
}
