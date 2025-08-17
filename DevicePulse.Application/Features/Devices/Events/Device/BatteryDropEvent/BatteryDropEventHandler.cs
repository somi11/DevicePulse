using DevicePulse.Application.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Application.Features.Devices.Events.Device.BatteryDropEvent
{
    public class BatteryDropEventHandler : INotificationHandler<BatteryDropEvent>
    {
        private readonly ILogger<BatteryDropEventHandler> _logger;
        private readonly IDeviceNotificationService _notificationService;
        private readonly IClientNotificationService  _notifier;

        public BatteryDropEventHandler(
            ILogger<BatteryDropEventHandler> logger,
            IDeviceNotificationService notificationService,
            IClientNotificationService notifier)
        {
            _logger = logger;
            _notificationService = notificationService;
            _notifier = notifier;
        }

        public async Task Handle(BatteryDropEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogWarning("[BatteryDropEventHandler] : BatteryDropEvent handled for DeviceId: {DeviceId}", notification.DeviceId);
            await _notificationService.SendBatteryDropAlertAsync(notification.DeviceId, cancellationToken);
            // Notify clients via SignalR
            await _notifier.NotifyEventAsync(notification.DeviceId,
                new { Event = "BatteryDrop", Message = "Battery dropped by ≥ 1%." });
        }
    }
}
