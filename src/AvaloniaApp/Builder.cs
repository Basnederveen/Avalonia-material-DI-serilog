using System.IO;
using AvaloniaApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AvaloniaApp;

internal static class Builder
{
    private const string logFolder = @"C:\temp\logs";

    public static ServiceProvider BuildLauncherServices()
    {
        // retrieve service collection, add all required services & view models
        var serviceCollection = GetServiceCollection();

        // add the serilog logging configuration
        AddLogger(serviceCollection);

        // build the service provider
        var services = serviceCollection.BuildServiceProvider();

        return services;
    }

    private static void AddLogger(IServiceCollection serviceCollection)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Trace()
            .MinimumLevel.Information()
            .WriteTo.File(Path.Combine(logFolder, "log.txt"),
                rollingInterval: RollingInterval.Day)
            .CreateLogger();

        serviceCollection.AddLogging(loggingBuilder =>
            loggingBuilder.AddSerilog(dispose: true));
    }

    public static IServiceCollection GetServiceCollection()
    {
        var builder = new ServiceCollection()

            // add all application viewmodels and services
            .AddSingleton<MainViewModel>()

            // add mediatr for messaging
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
        
        return builder;
    }
}

