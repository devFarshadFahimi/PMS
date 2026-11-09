using BusinessMakerFramework.Domain.Core.Entities;
using BusinessMakerFramework.Domain.Core.ValueObjects;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Interceptors;

public class EventStoreInterceptor(IServiceScopeFactory serviceScopeFactory) : SaveChangesInterceptor
{
    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        var aggregateRoots = eventData?.Context?.ChangeTracker.Entries<IAggregateRoot>();
        if (aggregateRoots is null)
        {
            return result;
        }

        using var scope = serviceScopeFactory.CreateScope();
        {
            //IEventStoreContext eventStore = scope.ServiceProvider.GetRequiredService<IEventStoreContext>();

            var data = aggregateRoots.Select(p => new
            {
                AggregateName = p.Entity.GetType().Name,
                BusinessId = p.CurrentValues.GetValue<BusinessId>("BusinessId").Value,
                Events = p.Entity.GetEvents()
            }).ToList();

            foreach (var item in data)
            {
                //await eventStore.SaveAsync(item.AggregateName, item.BusinessId, item.Events.ToList());
            }
            return result;
        }
    }
}
