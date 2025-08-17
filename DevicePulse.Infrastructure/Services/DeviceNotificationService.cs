using DevicePulse.Application.Contracts;
using Microsoft.Azure.Devices;
using Microsoft.Extensions.Logging;
using System;
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

        private async Task SendEventAndBlinkAsync(Guid deviceId, string alertType, string messageBody, object eventData, CancellationToken cancellationToken)
        {
            var eventPayload = new
            {
                AlertType = alertType,
                Message = messageBody,
                EventData = eventData,
                Timestamp = DateTime.UtcNow
            };

            // Send Event Message to Device
            var messageJson = JsonSerializer.Serialize(eventPayload);
            var message = new Message(Encoding.UTF8.GetBytes(messageJson))
            {
                Ack = DeliveryAcknowledgement.Full,
                MessageId = Guid.NewGuid().ToString()
            };

            try
            {
                await BlinkFlashlightAsync(deviceId, 500, alertType, eventData, cancellationToken);

                _logger.LogInformation("[DeviceNotificationService] Sent {AlertType} to device {DeviceId}", alertType, deviceId);
            }
            catch (Exception ex)
            {
                _logger.LogError("[DeviceNotificationService] Failed to send {AlertType} to device {DeviceId}", alertType, deviceId);
            }

 
        }


        private async Task BlinkFlashlightAsync(Guid deviceId, int durationMs, string reason, object eventData, CancellationToken cancellationToken = default)
        {
            try
            {
                var blinkPayload = JsonSerializer.Serialize(new
                {
                    duration = durationMs,
                    reason = reason,
                    eventInfo = eventData
                });


                var methodInvocation = new CloudToDeviceMethod(reason)
                {
                    ResponseTimeout = TimeSpan.FromSeconds(0)
                };
                methodInvocation.SetPayloadJson(blinkPayload);

                var response = await _serviceClient.InvokeDeviceMethodAsync(deviceId.ToString(), methodInvocation, cancellationToken);
                _logger.LogInformation("[DeviceNotificationService] : BlinkFlashlight response: {Status}, payload: {Payload}",
                    response.Status, response.GetPayloadAsJson());
            }
            catch (Microsoft.Azure.Devices.Common.Exceptions.DeviceNotFoundException)
            {
                _logger.LogWarning(
                    "[DeviceNotificationService] Device {DeviceId} is not online. Skipping BlinkFlashlight.",
                    deviceId);
            }
            catch (Microsoft.Azure.Devices.Common.Exceptions.IotHubException ex) when (ex.InnerException is TaskCanceledException)
            {
                _logger.LogWarning(
                    "[DeviceNotificationService] IoT Hub operation was canceled for {DeviceId}. Likely offline or unresponsive device.",
                    deviceId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "[DeviceNotificationService] Unexpected error while invoking BlinkFlashlight on {DeviceId}: {Message}",
                    deviceId, ex.Message);
            }
        }

        // Public alert methods (pass event-specific data)
        public Task SendFallAlertAsync(Guid deviceId, CancellationToken cancellationToken = default)
            => SendEventAndBlinkAsync(deviceId, "FallDetected", "Fall detected on the device.", null, cancellationToken);

        public Task SendBatteryDropAlertAsync(Guid deviceId,  CancellationToken cancellationToken = default)
            => SendEventAndBlinkAsync(deviceId, "BatteryDrop", "Battery dropped by 1% or more.", null, cancellationToken);

        public Task SendBatteryChargeAlertAsync(Guid deviceId, CancellationToken cancellationToken = default)
           => SendEventAndBlinkAsync(deviceId, "BatteryCharging", "Battery charged by 1% or more.", null, cancellationToken);

        public Task SendOfflineAlertAsync(Guid deviceId,  CancellationToken cancellationToken = default)
            => SendEventAndBlinkAsync(deviceId, "Offline", "Device appears to be offline.", null, cancellationToken);


        public Task SendMovementAlertAsync(Guid deviceId, CancellationToken cancellationToken = default)
            => SendEventAndBlinkAsync(deviceId, "DeviceMoved", "Device GPS location changed.", null, cancellationToken);
    }
}
