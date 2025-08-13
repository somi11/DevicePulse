using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Application.Contracts
{
    public interface IDeviceNotificationService
    {
      
        Task SendFallAlertAsync(Guid deviceId, CancellationToken cancellationToken = default);

        Task SendBatteryDropAlertAsync(Guid deviceId, CancellationToken cancellationToken = default);

        Task SendOfflineAlertAsync(Guid deviceId, CancellationToken cancellationToken = default);

        Task SendMovementAlertAsync(Guid deviceId, CancellationToken cancellationToken = default);
    }
}
