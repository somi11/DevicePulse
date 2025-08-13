using DevicePulse.Domain.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Infrastructure.Repositories
{
    public class InMemoryDeviceRepository
    {
        private static readonly ConcurrentDictionary<Guid, Device> _devices = new();

        public Task<Device?> GetByDeviceIdAsync(Guid id)
        {
            _devices.TryGetValue(id, out var device);
            return Task.FromResult(device);
        }

        public Task SaveAsync(Device device)
        {
            _devices[device.DeviceId] = device;
            return Task.CompletedTask;
        }
    }
}
