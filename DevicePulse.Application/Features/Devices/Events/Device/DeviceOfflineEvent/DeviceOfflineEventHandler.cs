using DevicePulse.Application.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Application.Features.Devices.Events.Device.DeviceOfflineEvent
{
    public class DeviceOfflineEventHandler : INotificationHandler<DeviceOfflineEvent>
    {
        private readonly ILogger<DeviceOfflineEventHandler> _logger;
        private readonly IDeviceNotificationService _notificationService;

        public DeviceOfflineEventHandler(
            ILogger<DeviceOfflineEventHandler> logger,
            IDeviceNotificationService notificationService)
        {
            _logger = logger;
            _notificationService = notificationService;
        }

        public async Task Handle(DeviceOfflineEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogWarning("[DeviceOfflineEventHandler] : DeviceOfflineEvent handled for DeviceId: {DeviceId}", notification.DeviceId);
            await _notificationService.SendOfflineAlertAsync(notification.DeviceId, cancellationToken);
        }
    }
}
