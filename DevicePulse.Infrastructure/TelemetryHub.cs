using Microsoft.AspNetCore.SignalR;

namespace DevicePulse.Infrastructure
{
    public class TelemetryHub : Hub
    {
        // Optional: methods if you want clients to call API side
        public async Task SubscribeDevice(Guid deviceId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, deviceId.ToString());
        }

        public async Task UnsubscribeDevice(Guid deviceId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, deviceId.ToString());
        }
    }
}
