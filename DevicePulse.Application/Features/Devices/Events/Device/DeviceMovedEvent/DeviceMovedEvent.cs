using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePulse.Application.Features.Devices.Events.Device.DeviceMovedEvent
{
    public class DeviceMovedEvent : INotification
    {
        public Guid DeviceId { get; }

        public DeviceMovedEvent(Guid deviceId)
        {
            DeviceId = deviceId;
        }
    }
}
