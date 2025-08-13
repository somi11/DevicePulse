using Azure.Messaging.EventHubs.Consumer;
using DevicePulse.Application.Features.Devices.Commands.ProcessTelemetry;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DevicePulse.Infrastructure.Services
{
    public class TelemetryListenerService : BackgroundService
    {
        private readonly EventHubConsumerClient _consumer;
        private readonly IMediator _mediator;
        private readonly ILogger<TelemetryListenerService> _logger;

        public TelemetryListenerService(string connectionString, string eventHubName, IMediator mediator, ILogger<TelemetryListenerService> logger)
        {
            _consumer = new EventHubConsumerClient(EventHubConsumerClient.DefaultConsumerGroupName, connectionString, eventHubName);
            _mediator = mediator;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (PartitionEvent partitionEvent in _consumer.ReadEventsAsync(stoppingToken))
            {
                var payload = Encoding.UTF8.GetString(partitionEvent.Data.Body.ToArray());
                _logger.LogInformation("[TelemetryListenerService] : Telemetry received: {Payload}", payload);

                // parse JSON and map to TelemetryDto
                var dto = JsonSerializer.Deserialize<TelemetryDto>(payload);
                //await _mediator.Send(new ProcessTelemetryCommand(dto.DeviceId, dto.DeviceName, dto), stoppingToken);
            }
        }
    }
}
