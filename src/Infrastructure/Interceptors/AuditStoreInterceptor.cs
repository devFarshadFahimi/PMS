using Application.Common;
using BusinessMakerFramework.Domain.Core.Entities;
using BusinessMakerFramework.Domain.Core.ValueObjects;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using static Application.Common.Utils.Consts;

namespace Infrastructure.Interceptors;

public class AuditStoreInterceptor(IServiceScopeFactory serviceScopeFactory) : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        _ = await base.SavingChangesAsync(eventData, result, cancellationToken);
        var auditableEntites = eventData?.Context?.ChangeTracker
            .Entries<IAuditableMarker>()
            .ToList();
        using var scope = serviceScopeFactory.CreateScope();
        {
            //IEventStoreContext eventStore = scope.ServiceProvider.GetRequiredService<IEventStoreContext>();
            if (auditableEntites == null || auditableEntites.Count == 0)
            {
                return result;
            }

            foreach (var item in auditableEntites)
            {
                List<Dictionary<string, object?>> changedProps =
                item.State == EntityState.Added
                 ?
                    item
                    .Properties
                    .Select(prop => new Dictionary<string, object?>
                    {
                        { prop.Metadata.Name,  prop.CurrentValue }
                    })
                    .ToList()
                 :
                    item
                    .Properties
                    .Where(prop => prop.IsModified)
                    .Select(prop => new Dictionary<string, object?>
                    {
                        { prop.Metadata.Name,  prop.CurrentValue }
                    })
                    .ToList();

                var auditedProps = new AuditedTracker
                {
                    OperatedBy = $"userid-{Guid.NewGuid()}",
                    SubmitDate = DateTime.Now,
                    State = item.State.ToString(),
                    ChangedProps = changedProps!
                };

                var streamId = EventStreamPrefix.MakeStreamId(item.Entity.GetType().Name, item.CurrentValues.GetValue<BusinessId>("BusinessId").Value.ToString());
                //await eventStore.SaveAsync(streamId, auditedProps);
            }
            return result;
        }
    }
}
