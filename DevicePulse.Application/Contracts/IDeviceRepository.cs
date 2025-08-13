using DevicePulse.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Application.Contracts
{
    public interface IDeviceRepository
    {
        Task<Device?> GetByDeviceIdAsync(Guid id);
        Task SaveAsync(Device device);

    }
}
