using Azure.Messaging.EventHubs.Consumer;
using DevicePulse.API.model;
using DevicePulse.Application.Features.Devices.Commands.ProcessTelemetry;
using DevicePulse.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;

namespace DevicePulse.Infrastructure.Services
{
    public class TelemetryListenerService : BackgroundService
    {
        private readonly EventHubConsumerClient _consumer;
        private readonly IMediator _mediator;
        private readonly ILogger<TelemetryListenerService> _logger;
        private readonly IServiceProvider _serviceProvider;

        private readonly ConcurrentDictionary<Guid, TelemetryDto> _telemetryBuffer = new();
        public TelemetryListenerService(
            IOptions<AzureIoTHubOptions> options,
            IServiceProvider serviceProvider,
            IMediator mediator,
            ILogger<TelemetryListenerService> logger)
        {
            var cfg = options.Value;

            _consumer = new EventHubConsumerClient(
                EventHubConsumerClient.DefaultConsumerGroupName,
                cfg.EventHubConnectionString,
                cfg.EventHubName);

            _serviceProvider = serviceProvider;

            _mediator = mediator;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("[TelemetryListenerService] : TelemetryListenerService started. Listening for telemetry events...");
           await foreach (var partitionEvent in _consumer.ReadEventsAsync(
            startReadingAtEarliestEvent: false, // start from latest events
            cancellationToken: stoppingToken))
            {
                try
                {
                    // Get Device ID from IoT Hub system properties
                    var deviceIdStr = partitionEvent.Data.SystemProperties["iothub-connection-device-id"]?.ToString();
                    if (string.IsNullOrWhiteSpace(deviceIdStr) || !Guid.TryParse(deviceIdStr, out Guid deviceId))
                    {
                        _logger.LogWarning("Invalid or missing deviceId: {DeviceId}", deviceIdStr);
                        continue;
                    }

                    // Parse JSON body
                    var payloadJson = Encoding.UTF8.GetString(partitionEvent.Data.Body.ToArray());
                    var newData = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(payloadJson);
                    if (newData == null)
                    {
                        _logger.LogWarning("Payload JSON could not be deserialized: {PayloadJson}", payloadJson);
                        continue;
                    }

                    //Merge with existing buffered telemetry
                    var telemetry = _telemetryBuffer.GetOrAdd(deviceId, _ => new TelemetryDto());

                    foreach (var prop in newData)
                    {
                        switch (prop.Key.ToLower())
                        {
                            case "accelerometer":
                                if (prop.Value.TryGetProperty("x", out var x) &&
                                    prop.Value.TryGetProperty("y", out var y) &&
                                    prop.Value.TryGetProperty("z", out var z))
                                {
                                    telemetry.Acceleration = new AccelerationData
                                    {
                                        X = x.GetDouble(),
                                        Y = y.GetDouble(),
                                        Z = z.GetDouble()
                                    };
                                }

                                break;

                            case "battery":
                                telemetry.Battery = new BatteryData
                                {
                                    Level = prop.Value.GetDouble()
                                };
                                break;

                            case "geolocation":
                                if (prop.Value.TryGetProperty("lat", out var lat) &&
                                    prop.Value.TryGetProperty("lon", out var lon))
                                {
                                    telemetry.Gps = new GpsData
                                    {
                                        Latitude = lat.GetDouble(),
                                        Longitude = lon.GetDouble()
                                    };
                                } else
                                {
                                    telemetry.Gps = new GpsData
                                    {
                                        Latitude = 222122.2323,
                                        Longitude = 432323.23
                                    };
                                }
                                break;
                        }
                    }

                    // Send when all required fields are present
                    if (telemetry.Acceleration != null &&
                        telemetry.Battery != null && telemetry.Gps !=null)
                    {
                        //_logger.LogInformation(
                        //"[TelemetryListenerService] : Sending command telemetry for device {DeviceId}: {@Telemetry}",
                        //deviceId,
                        //telemetry);
                        var command = new ProcessTelemetryCommand(deviceId, "Mi 11T Pro", telemetry);
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                            await mediator.Send(command, stoppingToken);
                        }


                        _telemetryBuffer.TryRemove(deviceId, out _);
                       
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[TelemetryListenerService] : Error processing telemetry message");
                }
            }
        }


    }
}

