using DevicePulse.Application.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Application.Features.Devices.Events.Device.FallDetectedEvent
{
    public class FallDetectedEventHandler : INotificationHandler<FallDetectedEvent>
    {
        private readonly ILogger<FallDetectedEventHandler> _logger;
        private readonly IDeviceNotificationService _notificationService;
        public FallDetectedEventHandler(
        ILogger<FallDetectedEventHandler> logger,
        IDeviceNotificationService notificationService)
        {
            _logger = logger;
            _notificationService = notificationService;
        }
        public async Task Handle(FallDetectedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogWarning("[FallDetectedEventHandler] : FallDetectedEvent handled for DeviceId: {DeviceId}", notification.DeviceId);
            await _notificationService.SendFallAlertAsync(notification.DeviceId, cancellationToken);
        }
    }
}
