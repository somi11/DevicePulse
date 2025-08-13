using DevicePulse.Application.Contracts;
using Microsoft.Azure.Devices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;


namespace DevicePulse.Infrastructure.Services
{
    public class DeviceNotificationService : IDeviceNotificationService
    {
        private readonly ServiceClient _serviceClient;
        private readonly ILogger<DeviceNotificationService> _logger;


        public DeviceNotificationService(string iotHubConnectionString, ILogger<DeviceNotificationService> logger)
        {
            _serviceClient = ServiceClient.CreateFromConnectionString(iotHubConnectionString);
            _logger = logger;
        }
        private async Task SendMessageAsync(Guid deviceId, string alertType, string messageBody, CancellationToken cancellationToken)
        {
            var messageJson = JsonSerializer.Serialize(new
            {
                AlertType = alertType,
                Message = messageBody,
                Timestamp = DateTime.UtcNow
            });

            var message = new Message(Encoding.UTF8.GetBytes(messageJson))
            {
                Ack = DeliveryAcknowledgement.Full,
                MessageId = Guid.NewGuid().ToString()
            };

            try
            {
                await _serviceClient.SendAsync(deviceId.ToString(), message, TimeSpan.FromSeconds(30));
                
                _logger.LogInformation("[DeviceNotificationService] : Sent {AlertType} message to device {DeviceId}", alertType, deviceId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[DeviceNotificationService] : Failed to send {AlertType} message to device {DeviceId}", alertType, deviceId);
            }
        }
   
        public Task SendFallAlertAsync(Guid deviceId, CancellationToken cancellationToken = default)
            => SendMessageAsync(deviceId, "FallDetected", "Fall detected on the device.", cancellationToken);


        public Task SendBatteryDropAlertAsync(Guid deviceId, CancellationToken cancellationToken = default)
            => SendMessageAsync(deviceId, "BatteryDrop", "Battery dropped by 1% or more.", cancellationToken);

        public Task SendOfflineAlertAsync(Guid deviceId, CancellationToken cancellationToken = default)
            => SendMessageAsync(deviceId, "Offline", "Device appears to be offline.", cancellationToken);

        public Task SendMovementAlertAsync(Guid deviceId, CancellationToken cancellationToken = default)
            => SendMessageAsync(deviceId, "DeviceMoved", "Device GPS location changed.", cancellationToken);

    }
}
