using DevicePulse.Application.Contracts;
using DevicePulse.API;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

namespace DevicePulse.Infrastructure.Services
{
    public class ClientNotifierService : IClientNotificationService
    {
        private readonly IHubContext<TelemetryHub> _hubContext;

        public ClientNotifierService(IHubContext<TelemetryHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task NotifyDeviceStatusAsync(Guid deviceId , object deviceData)
        {
            await _hubContext.Clients.Group(deviceId.ToString())
                .SendAsync("ReceivedDeviceStatus", deviceData);
        }
        public async Task NotifyTelemetryAsync(Guid deviceId, object telemetryData)
        {
            await _hubContext.Clients.Group(deviceId.ToString())
                .SendAsync("ReceiveTelemetry", telemetryData);
        }

        public async Task NotifyEventAsync(Guid deviceId, object eventData)
        {
            await _hubContext.Clients.Group(deviceId.ToString())
                .SendAsync("ReceiveEvent", eventData);
        }
    }
}
