using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Application.Contracts
{
    public interface IClientNotificationService
    {
        Task NotifyTelemetryAsync(Guid deviceId, object telemetryData);
        Task NotifyEventAsync(Guid deviceId, object eventData);
        Task NotifyDeviceStatusAsync(Guid deviceId, object deviceData);
    }
}
