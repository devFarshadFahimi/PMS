using System.Reflection;
using BusinessMakerFramework.Application.Common.Extensions;
using BusinessMakerFramework.Shared.Extensions.Caching.Distributed.Redis.DependencyInjections;
using BusinessMakerFramework.Shared.Extensions.Serializer.Microsoft.DependencyInjections;
using BusinessMakerFramework.SourceGenerator;
using BusinessMakerFramework.SourceGenerator.Shared.Extensions;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjections
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        _ = TypeAdapterConfig.GlobalSettings.Default.MapToConstructor(true);
        _ = TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());

        _ = services.AddFrameworkMediatR(Assembly.GetExecutingAssembly());
        _ = services.AddFrameworkValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        _ = services.AddFrameworkUnhandledExceptionBehaviour();
        _ = services.AddFrameworkValidationBehaviour();
        _ = services.AddValidationCultureInfo();
        _ = services.AddBuildingBlockMicrosoftSerializer();
        _ = services.AddBuildingBlockRedisDistributedCache(option =>
        {
            option.Configuration = "localhost:6379";
            option.InstanceName = "BusinessMaker.Sample.";
        });
        _ = services.AddCommandHandlers();
        _ = services.AddFrameworkMediator();

        return services;
    }
}

//public class PerformanceBehaviour<TRequest, TResponse>(PerformanceMetrics performanceMetrics) : IPipelineBehavior<TRequest, TResponse>
//    where TRequest : IRequest<TResponse>
//{
//    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
//    {
//        var startTime = Stopwatch.GetTimestamp();
//        TResponse response = await next(cancellationToken);
//        var elapsedTicks = Stopwatch.GetTimestamp() - startTime; // Calculate elapsed ticks
//        var elapsedMilliseconds = elapsedTicks * 1000 / Stopwatch.Frequency; // Convert ticks to milliseconds
//        if (elapsedMilliseconds > 500)
//        {
//            string requestName = typeof(TRequest).Name;
//            performanceMetrics.RecordLongRequests(elapsedMilliseconds);
//            performanceMetrics.IncreaseTotalLongRequests(requestName);
//        }
//        return response;
//    }
//}


//public class PerformanceMetrics
//{
//    public PerformanceMetrics(IMeterFactory meterFactory, IConfiguration configuration)
//    {
//        var meter = meterFactory.Create("BusinessMakerServiceTemplate");

//        LongRequestsHistogram = meter.CreateHistogram<double>("request_performance_duration_milliseconds", "Request", "Histogram for the duration of requests in milliseconds");

//        TotalLongRequestsCounter = meter.CreateCounter<int>("total_long_requests", "Request");
//    }

//    //Order meters
//    private Histogram<double> LongRequestsHistogram { get; }
//    private Counter<int> TotalLongRequestsCounter { get; }

//    //Orders meters
//    public void RecordLongRequests(double price) => LongRequestsHistogram.Record(price);
//    public void IncreaseTotalLongRequests(string requestName) => TotalLongRequestsCounter.Add(1, KeyValuePair.Create<string, object>("requestName", requestName)!);
//}