using DevicePulse.Application.Contracts;
using DevicePulse.Application.Features.Devices.Events.Device.BatteryDropEvent;
using DevicePulse.Application.Features.Devices.Events.Device.DeviceMovedEvent;
using DevicePulse.Application.Features.Devices.Events.Device.DeviceOfflineEvent;
using DevicePulse.Application.Features.Devices.Events.Device.FallDetectedEvent;
using DevicePulse.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Application.Features.Devices.Commands.ProcessTelemetry
{
    public class ProcessTelemetryCommandHandler : IRequestHandler<ProcessTelemetryCommand , Unit>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProcessTelemetryCommandHandler> _logger;
        private readonly IDeviceRepository _deviceRepository;


        public  ProcessTelemetryCommandHandler(IMediator mediator , 
                ILogger<ProcessTelemetryCommandHandler> logger, 
                IDeviceRepository deviceRepository)
        {

            _mediator = mediator;
            _logger = logger;
            _deviceRepository = deviceRepository;
        }
        public async Task<Unit> Handle(ProcessTelemetryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[ProcessTelemetryCommandHandler] : Processing telemetry for device {DeviceId}", request.DeviceId);

            //getting device or creating it 
            var device = await _deviceRepository.GetByDeviceIdAsync(request.DeviceId)
                     ?? new Device { DeviceId = request.DeviceId, Name = request.DeviceName ?? "New Device" };

            if (device != null)
            {
                var telemetry = new TelemetryReading
                {
                    DeviceId = request.DeviceId,
                    Timestamp = DateTime.UtcNow,
                    Acceleration = request.Telemetry.Acceleration,
                    Battery = request.Telemetry.Battery,
                    Gps = request.Telemetry.Gps,
                    Network = request.Telemetry.Network
                };

                device.UpdateFromTelemetry(telemetry);
                device.AddTelemetryReading(telemetry);

                // Business rules
                if (device.Acceleration.IsFallDetected())
                {
                    _logger.LogWarning("Fall detected for device {DeviceId}", request.DeviceId);
                    device.AddEvent(new DeviceEvent("FallDetected", "Device fall detected."));
                    await _mediator.Publish(new FallDetectedEvent(request.DeviceId), cancellationToken);
                }

                if (device.Battery.HasDroppedByAtLeastOnePercent())
                {
                    _logger.LogInformation("[ProcessTelemetryCommandHandler] : Battery dropped by ≥ 1% for device {DeviceId}", request.DeviceId);
                    device.AddEvent(new DeviceEvent("[ProcessTelemetryCommandHandler] : BatteryDrop", "Battery dropped by ≥ 1%."));
                    await _mediator.Publish(new BatteryDropEvent(request.DeviceId), cancellationToken);
                }

                if (!device.Network.IsOnline)
                {
                    _logger.LogWarning("[ProcessTelemetryCommandHandler]: Device {DeviceId} is offline", request.DeviceId);
                    device.AddEvent(new DeviceEvent("[ProcessTelemetryCommandHandler] : DeviceOffline", "Device appears offline."));
                    await _mediator.Publish(new DeviceOfflineEvent(request.DeviceId), cancellationToken);
                }

                if (device.Gps.PositionChanged())
                {
                    _logger.LogInformation("[ProcessTelemetryCommandHandler] :Device {DeviceId} moved to a new position", request.DeviceId);
                    device.AddEvent(new DeviceEvent("[ProcessTelemetryCommandHandler] : DeviceMoved", "Device changed location."));
                    await _mediator.Publish(new DeviceMovedEvent(request.DeviceId), cancellationToken);
                }
 
                // Save updated device state
                await _deviceRepository.SaveAsync(device);

                _logger.LogInformation("[ProcessTelemetryCommandHandler] : Telemetry processed for device {DeviceId}", request.DeviceId);

                return Unit.Value;
            }
            _logger.LogInformation("[ProcessTelemetryCommandHandler] : Telemetry can;t processed for device {DeviceId}", request.DeviceId);
            return Unit.Value;
        }
    }
}
