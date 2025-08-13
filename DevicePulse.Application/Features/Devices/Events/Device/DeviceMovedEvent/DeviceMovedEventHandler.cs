using DevicePulse.Application.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Application.Features.Devices.Events.Device.DeviceMovedEvent
{
    public class DeviceMovedEventHandler : INotificationHandler<DeviceMovedEvent>
    {
        private readonly ILogger<DeviceMovedEventHandler> _logger;
        private readonly IDeviceNotificationService _notificationService;

        public DeviceMovedEventHandler(
            ILogger<DeviceMovedEventHandler> logger,
            IDeviceNotificationService notificationService)
        {
            _logger = logger;
            _notificationService = notificationService;
        }

        public async Task Handle(DeviceMovedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[DeviceMovedEventHandler] : DeviceMovedEvent handled for DeviceId: {DeviceId}", notification.DeviceId);
            await _notificationService.SendMovementAlertAsync(notification.DeviceId, cancellationToken);
        }
    }
}
