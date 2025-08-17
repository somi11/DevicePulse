using DevicePulse.Application.Contracts;
using DevicePulse.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevicePulse.Infrastructure.Repositories
{
    public class InMemoryDeviceRepository : IDeviceRepository
    {
        private static readonly ConcurrentDictionary<Guid, Device> _devices = new();
        private readonly ILogger<InMemoryDeviceRepository> _logger;

        public InMemoryDeviceRepository(ILogger<InMemoryDeviceRepository> logger)
        {
            _logger = logger;
            _logger.LogInformation("[InMemoryDeviceRepository] : Initializing InMemoryDeviceRepository... ");
            _logger.LogInformation("[InMemoryDeviceRepository] : Only latest 10 Events and 10 Telemetries will be saved to Database... ");
            _logger.LogInformation("[InMemoryDeviceRepository] : Repository ready. Initial device count: {Count}", _devices.Count);
        }

        // Save or update a device
        public Task SaveAsync(Device device)
        {
            _devices[device.DeviceId] = device;
            _logger.LogInformation("[InMemoryDeviceRepository] : Device saved with {DeviceId}",
                device.DeviceId);
            _logger.LogInformation("[InMemoryDeviceRepository] : Total Telemetry: {TelemetryCount} | Total Events: {EventCount}",
                          device.TelemetryReadings.Count, device.Events.Count);
            return Task.CompletedTask;
        }

        // Get device by ID
        public Task<Device?> GetByDeviceIdAsync(Guid deviceId)
        {
            _devices.TryGetValue(deviceId, out var device);
            _logger.LogInformation("[InMemoryDeviceRepository] : Get device {DeviceId} => Found: {Found}", deviceId, device != null);
            return Task.FromResult(device);
        }

        // Get all devices
        public Task<IEnumerable<Device>> GetAllAsync()
        {
            return Task.FromResult(_devices.Values.AsEnumerable());
        }

        // Get telemetry for a specific device
        public Task<IEnumerable<TelemetryReading>> GetTelemetryByDeviceIdAsync(Guid deviceId, CancellationToken cancellationToken)
        {
            if (_devices.TryGetValue(deviceId, out var device))
            {
                return Task.FromResult(device.TelemetryReadings.AsEnumerable());
            }
            return Task.FromResult(Enumerable.Empty<TelemetryReading>());
        }

        // Get events for a specific device
        public Task<IEnumerable<DeviceEvent>> GetEventsByDeviceIdAsync(Guid deviceId, CancellationToken cancellationToken)
        {
            if (_devices.TryGetValue(deviceId, out var device))
            {
                return Task.FromResult(device.Events.AsEnumerable());
            }
            return Task.FromResult(Enumerable.Empty<DeviceEvent>());
        }


    }
}
