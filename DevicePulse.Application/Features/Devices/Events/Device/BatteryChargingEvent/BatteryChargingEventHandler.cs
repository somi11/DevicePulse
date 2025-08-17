using DevicePulse.Application.Contracts;
using DevicePulse.Application.Features.Devices.Events.Device.BatteryDropEvent;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Application.Features.Devices.Events.Device.BatteryChargingEvent
{
    public class BatteryChargingEventHandler : INotificationHandler<BatteryChargingEvent>
    {
        private readonly ILogger<BatteryDropEventHandler> _logger;
        private readonly IDeviceNotificationService _notificationService;

        public BatteryChargingEventHandler(
            ILogger<BatteryDropEventHandler> logger,
            IDeviceNotificationService notificationService)
        {
            _logger = logger;
            _notificationService = notificationService;
        }

        public async Task Handle(BatteryChargingEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogWarning("[BatteryChargingEventHandler] : BatteryChargingEvent handled for DeviceId: {DeviceId}", notification.DeviceId);
            await _notificationService.SendBatteryChargeAlertAsync(notification.DeviceId, cancellationToken);
        }
    }
}
