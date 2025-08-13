using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Application.Features.Devices.Events.Device.DeviceOfflineEvent
{
    public class DeviceOfflineEvent : INotification
    {
        public Guid DeviceId { get; }

        public DeviceOfflineEvent(Guid deviceId)
        {
            DeviceId = deviceId;
        }
    }
}
