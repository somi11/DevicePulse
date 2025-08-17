using DevicePulse.API.model;
using DevicePulse.Application.Contracts;
using DevicePulse.Application.Features.Devices.Commands.ProcessTelemetry;
using DevicePulse.Infrastructure.Repositories;
using DevicePulse.Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace DevicePulse.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<AzureIoTHubOptions>(config.GetSection("AzureIoTHub"));

            services.AddHostedService<TelemetryListenerService>();

            services.AddSingleton<IDeviceNotificationService>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DeviceNotificationService>>();
                var options = sp.GetRequiredService<IOptions<AzureIoTHubOptions>>().Value;
                return new DeviceNotificationService(options.ServiceConnectionString, logger);
            });

            services.AddScoped<IDeviceRepository, InMemoryDeviceRepository>();
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(ProcessTelemetryCommand).Assembly); // Application layer
            });
            services.AddSignalR();

            services.AddScoped<IClientNotificationService, ClientNotifierService>();

            return services;
        }

    }
}
