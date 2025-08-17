using DevicePulse.Application.Contracts;
using DevicePulse.Application.Features.Devices.Events.Device.BatteryChargingEvent;
using DevicePulse.Application.Features.Devices.Events.Device.BatteryDropEvent;
using DevicePulse.Application.Features.Devices.Events.Device.DeviceMovedEvent;
using DevicePulse.Application.Features.Devices.Events.Device.DeviceOfflineEvent;
using DevicePulse.Application.Features.Devices.Events.Device.FallDetectedEvent;
using DevicePulse.Application.models;
using DevicePulse.Domain.Entities;
using DevicePulse.Domain.Events;
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
        private readonly IClientNotificationService _clientNotificationService;


        public  ProcessTelemetryCommandHandler(IMediator mediator , 
                ILogger<ProcessTelemetryCommandHandler> logger, 
                IDeviceRepository deviceRepository,
                IClientNotificationService clientNotificationService
                )
        {

            _mediator = mediator;
            _logger = logger;
            _deviceRepository = deviceRepository;
            _clientNotificationService = clientNotificationService;
        }
        public async Task<Unit> Handle(ProcessTelemetryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[ProcessTelemetryCommandHandler] : Processing telemetry for device {DeviceId}", request.DeviceId);

            //getting device or creating it 
            var device = await _deviceRepository.GetByDeviceIdAsync(request.DeviceId)
                     ?? new Device { DeviceId = request.DeviceId, Name = request.DeviceName ?? "Mi 11T Pro" };
            if (device != null)
            {
                var telemetry = new TelemetryReading
                {
                    DeviceId = request.DeviceId,
                    Timestamp = DateTime.UtcNow,
                    Acceleration = request.Telemetry.Acceleration,
                    Battery = request.Telemetry.Battery,
                    Gps = request.Telemetry.Gps,
                };
                var events = device.UpdateTelemetry(telemetry);

                foreach (var domainEvent in events)
                {
                    switch (domainEvent)
                    {
                        case FallDetectedDomainEvent fall:
                            _logger.LogInformation(
                                "[ProcessTelemetryCommandHandler] : Device {DeviceId} FALL detected | Old X={OldX} Y={OldY} Z={OldZ} | New X={NewX} Y={NewY} Z={NewZ}",
                                device.DeviceId,
                                fall.Old_X, fall.Old_Y, fall.Old_Z,
                                fall.X, fall.Y, fall.Z
                            );
                            var fallEvent = new DeviceEvent("FallDetected", "Device fall detected.");
                            fallEvent.DeviceId = device.DeviceId;
                            device.AddEvent(fallEvent);
                            await _clientNotificationService.NotifyEventAsync(device.DeviceId, fallEvent);
                            await _mediator.Publish(new FallDetectedEvent(device.DeviceId), cancellationToken);
                            break;

                        case BatteryDropDomainEvent battery:
                            _logger.LogInformation(
                                "[ProcessTelemetryCommandHandler] : Device {DeviceId} Battery drop | Old={OldLevel} New={NewLevel}",
                                device.DeviceId,
                                battery._previousLevel,
                                battery.Level
                            );
                            var batteryDropEvent = new DeviceEvent("BatteryDrop", "Battery dropped by ≥ 1%.");
                             batteryDropEvent.DeviceId = device.DeviceId;
                            device.AddEvent(batteryDropEvent);
                            
                            await _clientNotificationService.NotifyEventAsync(device.DeviceId, batteryDropEvent);
                            await _mediator.Publish(new BatteryDropEvent(device.DeviceId), cancellationToken);
                            break;

                        case DeviceMovedDomainEvent gps:
                            _logger.LogInformation(
                                "[ProcessTelemetryCommandHandler] : Device {DeviceId} MOVED | OldLat={OldLat} OldLng={OldLng} | NewLat={NewLat} NewLng={NewLng}",
                                device.DeviceId,
                                gps.OldLatitude, gps.OldLongitude,
                                gps.NewLatitude, gps.NewLongitude
                            );
                            // Notify about device movement
                            var deviceMovedEvent = new DeviceEvent("DeviceMoved", "Device moved to a new location.");
                            deviceMovedEvent.DeviceId = device.DeviceId;
                            device.AddEvent(deviceMovedEvent);
                            await _clientNotificationService.NotifyEventAsync(device.DeviceId, deviceMovedEvent);
                            await _mediator.Publish(new DeviceMovedEvent(device.DeviceId), cancellationToken);
                            break;

                        case BatteryChargingDomainEvent charge:
                            _logger.LogInformation("[TelemetryHandler] Device {DeviceId} BATTERY CHARGING | Old: {Old} New: {New}",
                                device.DeviceId, charge.PreviousLevel, charge.Level);
                            var batteryChargingEvent = new DeviceEvent("BatteryCharging", "Battery charging detected.");
                             batteryChargingEvent.DeviceId = device.DeviceId;
                            device.AddEvent(batteryChargingEvent);
                            await _clientNotificationService.NotifyEventAsync(device.DeviceId, batteryChargingEvent);
                            await _mediator.Publish(new BatteryChargingEvent(device.DeviceId), cancellationToken);
                            break;
                    }
                }

                // Save updated device state
                await _deviceRepository.SaveAsync(device);
                // Push live telemetry to SignalR
                await _clientNotificationService.NotifyTelemetryAsync(device.DeviceId, telemetry);

                //Push device status to SignalR
                var deviceStatus = new DeviceStatus
                {
                    DeviceId = device.DeviceId,
                    DeviceName = device.Name,
                    LastSeen = DateTime.UtcNow,
                    Status = telemetry.Battery.Level > 20 ? "Healthy" : "Battery low",
                    BatteryLevel = telemetry.Battery.Level,
                    Location = telemetry.Gps.Longitude.ToString(),
                    IsCharging = telemetry.Battery.Level > 90, 
                    IsMoving = device.Gps.PositionChanged(), 
                    FallDetected = device.Acceleration.IsFallDetected() 
                };

                await _clientNotificationService.NotifyDeviceStatusAsync(device.DeviceId ,deviceStatus);

                
                return Unit.Value;
            }
            _logger.LogInformation("[ProcessTelemetryCommandHandler] : Telemetry can;t processed for device {DeviceId}", request.DeviceId);
            return Unit.Value;
        }
    }
}
